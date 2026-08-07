using System.Text.Json;
using Elsa.Studio.Authentication.ElsaIdentity.BlazorWasm.Extensions;
using Elsa.Studio.Authentication.ElsaIdentity.HttpMessageHandlers;
using Elsa.Studio.Authentication.ElsaIdentity.UI.Extensions;
using Elsa.Studio.Authentication.OpenIdConnect.BlazorWasm.Extensions;
using Elsa.Studio.Authentication.OpenIdConnect.HttpMessageHandlers;
using Elsa.Studio.Contracts;
using Elsa.Studio.Core.BlazorWasm.Extensions;
using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Localization.BlazorWasm.Extensions;
using Elsa.Studio.Localization.Models;
using Elsa.Studio.Models;
using Elsa.Studio.Options;
using Elsa.Studio.Shell;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Designer.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.RootComponents.RegisterCustomElsaStudioElements();

var authProvider = configuration["Authentication:Provider"];
if (string.IsNullOrWhiteSpace(authProvider))
    authProvider = "ElsaIdentity";

Type authenticationHandler;

if (authProvider.Equals("ElsaIdentity", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddElsaIdentity();
    builder.Services.AddElsaIdentityUI();
    authenticationHandler = typeof(ElsaIdentityAuthenticatingApiHttpMessageHandler);
}
else if (authProvider.Equals("OpenIdConnect", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddOpenIdConnectAuth(options =>
    {
        configuration.GetSection("Authentication:OpenIdConnect").Bind(options);
    });
    authenticationHandler = typeof(OidcAuthenticatingApiHttpMessageHandler);
}
else
{
    throw new InvalidOperationException($"Unsupported Authentication:Provider value '{authProvider}'.");
}

var localizationConfig = new LocalizationConfig
{
    ConfigureLocalizationOptions = options => configuration.GetSection("Localization").Bind(options)
};

builder.Services.AddCore();
builder.Services.AddShell();
builder.Services.AddRemoteBackend(new()
{
    ConfigureHttpClientBuilder = options => options.AuthenticationHandler = authenticationHandler
});

builder.Services.AddDashboardModule();
builder.Services.AddWorkflowsModule();
builder.Services.AddLocalizationModule(localizationConfig);

var app = builder.Build();

await app.UseElsaLocalization();

var js = app.Services.GetRequiredService<IJSRuntime>();
var clientConfig = await js.InvokeAsync<JsonElement>("getClientConfig");
var apiUrl = clientConfig.GetProperty("apiUrl").GetString() ?? throw new InvalidOperationException("No API URL configured.");
app.Services.GetRequiredService<IOptions<BackendOptions>>().Value.Url = new(apiUrl);

var startupTaskRunner = app.Services.GetRequiredService<IStartupTaskRunner>();
await startupTaskRunner.RunStartupTasksAsync();

await app.RunAsync();
