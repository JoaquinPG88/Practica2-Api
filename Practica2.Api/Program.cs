using Serilog;
using Practica2.Api;  
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();

// Añadir configuración (appsettings.json, env vars, etc.)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// registra nuestros servicios para inyección
builder.Services.AddSingleton<Services.ExternalServices.ElectronicStoreService>();
builder.Services.AddSingleton<BusinessLogic.Managers.GiftManager>();
builder.Services.AddSingleton<BusinessLogic.Managers.PatientManager>();

// Registrar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Para depuración, imprime el entorno
Console.WriteLine("Environment: " + app.Environment.EnvironmentName);

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configurar pipeline
app.UseRouting();

// Exponer Swagger UI siempre, sin condicionar a Development
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Practica2 API V1");
    c.RoutePrefix = "swagger";  // expuesto en /swagger
});

app.UseAuthorization();

app.MapControllers();

app.Run();
