using TaskManagerAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DotNetEnv;

Env.Load();
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Connect to SQLite database
builder.Services.AddDbContext<DataContext>(options =>
{
  options.UseSqlite(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
