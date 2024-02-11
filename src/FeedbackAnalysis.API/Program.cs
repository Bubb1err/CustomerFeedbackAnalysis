using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FeedbackAnalysis.API.Middlewares;
using FeedbackAnalysis.Application;
using FeedbackAnalysis.Contracts;
using FeedbackAnalysis.Infrastructure;
using FeedbackAnalysis.Persistance;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

SecretClientOptions options = new SecretClientOptions()
{
    Retry = { Delay = TimeSpan.FromSeconds(2), MaxDelay = TimeSpan.FromSeconds(16), MaxRetries = 5, Mode = RetryMode.Exponential }
};

string clientId = builder.Configuration.GetValue<string>("ClientID");
string clientSecret = builder.Configuration.GetValue<string>("ClientSecret");
string tenantId = builder.Configuration.GetValue<string>("TenantID");
string keyvaultLink = builder.Configuration.GetValue<string>("VaultURL");

ClientSecretCredential csc = new ClientSecretCredential(tenantId, clientId, clientSecret);
var client = new SecretClient(new Uri(keyvaultLink), csc, options);
builder.Configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());

builder.Services
    .AddPersistance(builder.Configuration)
    .AddInfrastructure()
    .AddApplication()
    .AddContracts();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
