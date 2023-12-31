using Azure.Core.Diagnostics;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using MediaCollectionMVC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// From: https://stackoverflow.com/questions/62817337/azure-keyvault-azure-identity-credentialunavailableexception-defaultazurecrede/64341779#64341779
//AZURE_CLIENT_ID - service principal's app id
//AZURE_TENANT_ID - id of the principal's Azure Active Directory tenant
//AZURE_CLIENT_SECRET - one of the service principal's client secrets

// let's find out what environment we're in:
string stringEnvironment = builder.Environment.EnvironmentName.ToString();


switch (stringEnvironment)
{
    case "Development":
        // Scope of MediaDbContext is Scoped by default.
        // Scoped means the context object lifetime coincides with the web request life time,
        // and the Dispose method will be called automatically at the end of the web request.
        builder.Services.AddDbContext<MediaDbContext>(opt
            => opt.UseSqlServer(builder.Configuration.GetConnectionString("CALIFORNIASTConnectionString")));

        //CALIFORNIASTConnectionString
        break;
    case "Production":
        // Add Azure Key Vault to retrieve the connection string.
        var keyVaultUrl = new Uri(builder.Configuration.GetSection("KeyVaultUrl").Value!);
        var azureCredential = new DefaultAzureCredential();
        builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);
        var cs = builder.Configuration.GetSection("KeyVaultMediaDBConnectionString").Value;
        builder.Services.AddDbContext<MediaDbContext>(opt
            => opt.UseSqlServer(cs));
         break;
    default:
        break;
}



// Add logging:setup a listener to monitor logged events.
using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();

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
