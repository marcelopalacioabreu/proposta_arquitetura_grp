using Microsoft.EntityFrameworkCore;
using Retaguarda.Persistencia.MYSQL;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

app.Run();
