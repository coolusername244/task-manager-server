using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Extensions
{
  public static class ApplicationServiceExtension
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
      // Connect to SQLite database
      services.AddDbContext<DataContext>(options =>
      {
        options.UseSqlite(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
      });
      services.AddCors();
      services.AddScoped<ITokenService, TokenService>();

      return services;
    }
  }
}