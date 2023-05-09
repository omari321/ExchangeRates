using ExchangeRates.Application;
using ExchangeRates.Infrastructure;
using ExchangeRates.Shared.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Host.AddSettingsConfiguration();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.AddDbContext();
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddSwaggerGen();
var app = builder.Build();

//todo rate limiting
//todo cache ing
//todo exception middleware
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rates Api API V1");
        c.OAuthClientId("swagger-client");
        c.DefaultModelExpandDepth(2);
        c.DefaultModelRendering(ModelRendering.Model);
        c.DisplayRequestDuration();
        c.DocExpansion(DocExpansion.None);
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
        c.EnableValidator();
    }); ;
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();