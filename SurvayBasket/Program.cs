using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SurvayBasket;
using SurvayBasket.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependacies(builder.Configuration);

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration.GetConnectionString("Redis"));


builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
    
    
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();
app.MapHealthChecks("health", new HealthCheckOptions
{

    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});



app.Run();
