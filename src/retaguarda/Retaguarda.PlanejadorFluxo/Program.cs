using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Elsa;
using Elsa.Extensions;
using Elsa.Persistence.EFCore;
using Elsa.Persistence.EFCore.PostgreSql;
using Elsa.Persistence.EFCore.Extensions;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

// Load optional config
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// DataProtection: persist keys to shared workspace folder so API and planner share keys
var keysPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "..", "data-protection-keys"));
builder.Services.AddDataProtection()
	.PersistKeysToFileSystem(new DirectoryInfo(keysPath))
	.SetApplicationName("Retaguarda");

// JWT from cookie support
var jwtKey = builder.Configuration["Jwt:Key"] ?? "change_this_secret_for_dev";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "Retaguarda";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
if (keyBytes.Length < 32)
{
	using var sha = System.Security.Cryptography.SHA256.Create();
	keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(jwtKey));
}

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwtIssuer,
			ValidateAudience = false,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
		};

		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]) && context.Request.Cookies.ContainsKey("access_token"))
				{
					context.Token = context.Request.Cookies["access_token"];
				}
				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddHttpClient("Elsa", client =>
{
	var baseUrl = builder.Configuration["Elsa:BaseUrl"] ?? "http://localhost:4500";
	client.BaseAddress = new Uri(baseUrl);
	client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddControllers();

builder.Services.AddRazorPages();

// Register Elsa services if Elsa connection is configured
var elsaConnection = builder.Configuration.GetSection("Elsa:ConnectionStrings")?.GetValue<string>("DefaultConnection");
// The Elsa registration is disabled by default to avoid API version mismatches while
// package versions are being aligned. Define the compilation symbol ENABLE_ELSA
// to enable these lines after confirming package compatibility.
#if ENABLE_ELSA
if (!string.IsNullOrEmpty(elsaConnection))
{
	// Configure Elsa management and runtime with EF Core (Postgres)
	builder.Services.AddElsa(elsa =>
	{
		elsa.UseWorkflowManagement(management =>
		{
			management.UseEntityFrameworkCore(ef =>
			{
				ef.UsePostgreSql(elsaConnection);
				ef.RunMigrations = builder.Environment.IsDevelopment();
			});
		});

		elsa.UseWorkflowRuntime(runtime =>
		{
			runtime.UseEntityFrameworkCore(ef =>
			{
				ef.UsePostgreSql(elsaConnection);
				ef.RunMigrations = builder.Environment.IsDevelopment();
			});
		});

		// Expose Elsa workflow APIs and enable HTTP activities
		elsa.UseWorkflowsApi();
		elsa.UseHttp();
	});

	// Map Elsa endpoints at runtime
	// Note: the middleware call is done after building the app (app.UseWorkflowsApi())
}
#endif

// Register the PlanejadorFluxo DbContext if a connection string is available
var planejadorConnection = builder.Configuration.GetSection("Elsa:ConnectionStrings")?.GetValue<string>("DefaultConnection")
	?? builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(planejadorConnection))
{
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddDbContext<Retaguarda.PlanejadorFluxo.PlanejadorFluxoDbContext>(options =>
		options.UseNpgsql(planejadorConnection));

	// Note: we register the PlanejadorFluxoDbContext for EF and DI; do not map to IApplicationDbContext here.
}

var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map Elsa endpoints and static assets (Studio WASM)
#if ENABLE_ELSA
app.UseWorkflowsApi();
app.UseWorkflows();
app.MapStaticAssets();

// Serve the Elsa Studio host page from /studio when Studio is hosted in this app.
// This maps any /studio/* path to the _Host.cshtml Blazor WASM host page.
app.MapFallbackToPage("/studio/{**path}", "/_Host");
app.MapFallbackToPage("/studio", "/_Host");
#endif

app.MapGet("/", () => "Retaguarda.PlanejadorFluxo running");
app.MapControllers();

// Simple painel page
app.MapGet("/painel", async (HttpContext http, IConfiguration config) =>
{
	var elsaBase = config["Elsa:BaseUrl"] ?? "http://localhost:4500";
	var html = $"<html><body style='margin:0;padding:0;'><iframe src='/planejadorDeFluxo/' style='width:100%;height:100vh;border:0;'></iframe></body></html>";
	http.Response.ContentType = "text/html; charset=utf-8";
	await http.Response.WriteAsync(html);
});

app.Run();
