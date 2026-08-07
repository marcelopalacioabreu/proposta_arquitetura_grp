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

// Register static web assets from referenced packages before serving static files
// (follows Elsa docs recommendation: call MapStaticAssets before UseStaticFiles)
try { app.MapStaticAssets(); } catch { }

// Serve Blazor framework files (/_framework) for embedded Blazor WASM (Elsa Studio)
app.UseBlazorFrameworkFiles();
// Serve static files from the NuGet package providing the Blazor WebAssembly runtime
// (blazor.webassembly.js) so the Studio client can load the runtime when requested
// via the proxied prefix (e.g. /planejadorDeFluxo/_framework/...).
try
{
	var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
	var nugetRoot = Path.Combine(userProfile, ".nuget", "packages");
	if (Directory.Exists(nugetRoot))
	{
		var files = Directory.EnumerateFiles(nugetRoot, "blazor.webassembly.js", SearchOption.AllDirectories);
		var first = files.FirstOrDefault();
		if (!string.IsNullOrEmpty(first))
		{
			var frameworkDir = Path.GetDirectoryName(first);
			if (Directory.Exists(frameworkDir))
			{
				var provider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(frameworkDir);
				app.UseStaticFiles(new StaticFileOptions { FileProvider = provider, RequestPath = "/_framework" });
			}
		}
	}
}
catch { }
// Also try to serve Elsa Studio _content assets directly from the Elsa.Studio.Shell package
try
{
	var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
	var nugetRoot = Path.Combine(userProfile, ".nuget", "packages");
	if (Directory.Exists(nugetRoot))
	{
		// Find the Elsa.Studio.Shell package folder (pick the first match)
		var shellPkg = Directory.EnumerateDirectories(nugetRoot, "elsa.studio.shell*", SearchOption.TopDirectoryOnly).OrderByDescending(d => d).FirstOrDefault();
		if (!string.IsNullOrEmpty(shellPkg))
		{
			var staticRoot = Path.Combine(shellPkg, "staticwebassets");
			if (Directory.Exists(staticRoot))
			{
				var provider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(staticRoot);
				// Map the package's static assets under the expected _content path
				app.UseStaticFiles(new StaticFileOptions { FileProvider = provider, RequestPath = "/_content/Elsa.Studio.Shell" });
			}
		}
	}
}
catch { }

// (Removed heuristic NuGet-serving middleware. Rely on MapStaticAssets() and UseStaticFiles() per docs.)

// Serve the Blazor runtime from NuGet cache as a compatibility fallback when static web assets
// do not expose it (helps when Studio is provided via NuGet packages instead of a project reference).
app.MapGet("/_framework/blazor.webassembly.js", async (HttpContext ctx) =>
{
	try
	{
		var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		var nugetRoot = Path.Combine(userProfile, ".nuget", "packages");
		if (Directory.Exists(nugetRoot))
		{
			var candidates = new[] { "microsoft.aspnetcore.app.internal.assets", "microsoft.aspnetcore.components.webassembly" };
			foreach (var c in candidates)
			{
				var dirs = Directory.EnumerateDirectories(nugetRoot, c + "*", SearchOption.TopDirectoryOnly);
				foreach (var dir in dirs)
				{
					var p1 = Path.Combine(dir, "_framework", "blazor.webassembly.js");
					var p2 = Directory.EnumerateFiles(dir, "blazor.webassembly.js", SearchOption.AllDirectories).FirstOrDefault();
					var file = File.Exists(p1) ? p1 : p2;
					if (!string.IsNullOrEmpty(file) && File.Exists(file))
					{
						ctx.Response.ContentType = "application/javascript";
						await ctx.Response.SendFileAsync(file);
						return;
					}
				}
			}
		}
	}
	catch { }
	ctx.Response.StatusCode = 404;
});

// Serve a compatibility file name ElsaStudio.styles.css by returning the package's shell.css
app.MapGet("/_content/Elsa.Studio.Shell/ElsaStudio.styles.css", async (HttpContext ctx) =>
{
	try
	{
		var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		var nugetRoot = Path.Combine(userProfile, ".nuget", "packages");
		if (Directory.Exists(nugetRoot))
		{
			var shellPkg = Directory.EnumerateDirectories(nugetRoot, "elsa.studio.shell*", SearchOption.TopDirectoryOnly).OrderByDescending(d => d).FirstOrDefault();
			if (!string.IsNullOrEmpty(shellPkg))
			{
				var candidate = Path.Combine(shellPkg, "staticwebassets", "css", "shell.css");
				if (!File.Exists(candidate))
				{
					// try other locations
					candidate = Directory.EnumerateFiles(Path.Combine(shellPkg, "staticwebassets"), "shell.css", SearchOption.AllDirectories).FirstOrDefault();
				}

				if (!string.IsNullOrEmpty(candidate) && File.Exists(candidate))
				{
					ctx.Response.ContentType = "text/css";
					await ctx.Response.SendFileAsync(candidate);
					return;
				}
			}
		}
	}
	catch { }
	ctx.Response.StatusCode = 404;
});


#if ENABLE_ELSA
// Map Elsa static web assets before UseStaticFiles so the framework can register package assets
app.MapStaticAssets();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map Elsa endpoints and middleware
app.UseWorkflowsApi();
app.UseWorkflows();

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

// Diagnostic endpoint to help locate static assets in NuGet cache
app.MapGet("/__debug/elsa-static", (HttpContext http) =>
{
	var result = new System.Collections.Generic.List<string>();
	try
	{
		var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		var nugetRoot = Path.Combine(userProfile, ".nuget", "packages");
		result.Add($"NuGetRoot={nugetRoot}");
		if (Directory.Exists(nugetRoot))
		{
			var blazorFiles = Directory.EnumerateFiles(nugetRoot, "blazor.webassembly.js", SearchOption.AllDirectories).Take(20);
			result.Add("blazor.webassembly.js candidates:");
			foreach (var f in blazorFiles) result.Add(f);

			var shellFiles = Directory.EnumerateFiles(nugetRoot, "shell.css", SearchOption.AllDirectories).Take(20);
			result.Add("shell.css candidates:");
			foreach (var f in shellFiles) result.Add(f);
		}
		else
		{
			result.Add("NuGet root not found");
		}
	}
	catch (Exception ex)
	{
		result.Add("error: " + ex.Message);
	}
	return Results.Text(string.Join("\n", result));
});

app.Run();
