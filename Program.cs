using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// >>> MODIFICAR ESTA SECCIÓN PARA LEER DE LA CONFIGURACIÓN <<<
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(connectionString)
);
// >>> FIN DE LA MODIFICACIÓN <<<

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

app.MapControllers();

if (args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TodoContext>();
    db.Database.Migrate();
}

app.Run();