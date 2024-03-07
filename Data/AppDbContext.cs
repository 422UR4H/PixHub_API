using PixHub.Models;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<TestModel> Test { get; set; }
}