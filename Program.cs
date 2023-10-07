using Azure.Core.Diagnostics;
using Azure.Identity;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// From: https://stackoverflow.com/questions/62817337/azure-keyvault-azure-identity-credentialunavailableexception-defaultazurecrede/64341779#64341779
//AZURE_CLIENT_ID - service principal's app id
//AZURE_TENANT_ID - id of the principal's Azure Active Directory tenant
//AZURE_CLIENT_SECRET - one of the service principal's client secrets



// From: Create an ASP.NET Core web app with Azure App Configuration
// This is green underlined as a warning because the returned value COULD be null.
//string connectionString = builder.Configuration.GetConnectionString("AppConfig");
//builder.Configuration.AddAzureAppConfiguration(connectionString);
//builder.Configuration.AddAzureKeyVault(
//    new Uri($"https://gp-media-keyvault.vault.azure.net/"),
//    new DefaultAzureCredential());

// Suggested by Copilot
//builder.Services.AddAzureAppConfiguration(options =>
//{
//    options.Connect(connectionString)
//        .ConfigureKeyVault(kv =>
//        {
//            kv.SetCredential(new DefaultAzureCredential());
//        });
//});


// Setup a listener to monitor logged events.
using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();

// From: Create an ASP.NET Core web app with Azure Key Vault
var keyVaultUrl = new Uri(builder.Configuration.GetSection("KeyVaultUrl2").Value!);
var azureCredential = new DefaultAzureCredential();
//DefaultAzureCredentialOptions
azureCredential.GetTokenAsync(new Azure.Core.TokenRequestContext(new[] { "https://gp-media-keyvault.vault.azure.net/" })).GetAwaiter().GetResult();
builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);
var connStr = builder.Configuration.GetConnectionString("KeyVaultMediaDBConnectionString");
var connStr2 = builder.Configuration.GetConnectionString("AppConfig");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
