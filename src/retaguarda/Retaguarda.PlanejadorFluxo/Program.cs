using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Elsa.Extensions;
using Elsa.Persistence.EFCore.Extensions;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Share DataProtection keys with the main API
var keysPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "..", "data-protection-keys"));
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("Retaguarda");

var services = builder.Services;
var configuration = builder.Configuration;

var connectionString = configuration.GetSection("Elsa:ConnectionStrings")
    .GetValue<string>("DefaultConnection")
    ?? configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No database connection string configured.");

services.AddElsa(elsa => elsa
    .UseIdentity(identity =>
    {
        identity.TokenOptions = options => options.SigningKey = "large-signing-key-for-signing-JWT-tokens-elsa";
        identity.UseAdminUserProvider();
    })
    .UseDefaultAuthentication()
    .UseWorkflowManagement(management => management.UseEntityFrameworkCore(ef =>
    {
        ef.UsePostgreSql(connectionString);
        ef.RunMigrations = builder.Environment.IsDevelopment();
    }))
    .UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(ef =>
    {
        ef.UsePostgreSql(connectionString);
        ef.RunMigrations = builder.Environment.IsDevelopment();
    }))
    .UseScheduling()
    .UseJavaScript()
    .UseLiquid()
    .UseCSharp()
    .UseHttp(http => http.ConfigureHttpOptions = options => configuration.GetSection("Http").Bind(options))
    .UseWorkflowsApi()
    .AddActivitiesFrom<Program>()
    .AddWorkflowsFrom<Program>()
);

services.AddCors(cors => cors.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")));

services.AddRazorPages(options =>
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseCors();
app.UseStaticFiles();
app.UseBlazorFrameworkFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi();
app.UseWorkflows();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();
