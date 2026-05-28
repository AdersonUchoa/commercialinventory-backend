var builder = WebApplication.CreateBuilder(args);

// TODO: Add services to the container.
//builder.Services.AddApplication();
//builder.Services.AddInfrastructure(builder.Configuration);
//builder.Services.AddDomain();
//builder.Services.AddWebApi(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CommercialInventory API v1");
        c.RoutePrefix = "swagger";
    });
}

// TODO: Configure CORS
//app.UseCors("CommercialInventoryCors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
