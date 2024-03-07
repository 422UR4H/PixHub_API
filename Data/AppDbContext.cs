using PixHub.Models;
using Microsoft.EntityFrameworkCore;

namespace PixHub.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<User> User { get; set; }
  public DbSet<PixKey> PixKey { get; set; }
  public DbSet<PaymentProvider> PaymentProvider { get; set; }
  public DbSet<PaymentProviderAccount> PaymentProviderAccount { get; set; }
}