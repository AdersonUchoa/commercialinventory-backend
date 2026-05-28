using Asp.Versioning.ApiExplorer;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using WebAPI.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDomain();
builder.Services.AddWebApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(c =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"CommercialInventory API {description.GroupName.ToUpperInvariant()}");
        }
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("CommercialInventoryCors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if(app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<CommercialInventoryDbContext>>();
    try
    {
        logger.LogInformation("Verificando e aplicando Migrations pendentes no banco de dados.");

        var context = services.GetRequiredService<CommercialInventoryDbContext>();
        context.Database.Migrate();

        logger.LogInformation("Migrations aplicadas com sucesso.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocorreu um erro ao tentar aplicar as migrations.");
    }
}

app.Run();
