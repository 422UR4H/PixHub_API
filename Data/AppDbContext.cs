using MakeMeAPix.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeMeAPix.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<TestModel> Test { get; set; }
}