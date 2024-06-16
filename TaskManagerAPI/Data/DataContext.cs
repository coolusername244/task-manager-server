using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Data;

public class DataContext : DbContext
{
  public DataContext(DbContextOptions options) : base(options)
  {
  }

  public DbSet<AppUser> Users { get; set; }
}
