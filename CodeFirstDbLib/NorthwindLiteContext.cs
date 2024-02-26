using Microsoft.EntityFrameworkCore;

namespace CodeFirstDbLib;
public class NorthwindLiteContext : DbContext
{
  public NorthwindLiteContext(DbContextOptions<NorthwindLiteContext> options) : base(options) { }
  public NorthwindLiteContext() { }

  public DbSet<Customer> Customers { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<OrderDetail> OrderDetails { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<Shipment> Shipments { get; set; }   

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (optionsBuilder.IsConfigured) return;
    string connectionString = "server=(LocalDB)\\mssqllocaldb;attachdbfilename=C:\\Users\\jakob\\Documents\\Schule\\Pos\\C#\\121_CodeFirst\\NorthwindLite.mdf; database=NorthwindLite;integrated security=True;MultipleActiveResultSets=True;";
    optionsBuilder.UseSqlServer(connectionString);
  }
}
