using CodeFirstDbLib;

using System.Windows;

namespace WPFFrontend;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  private NorthwindLiteContext db;

  public MainWindow() => InitializeComponent();

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    db = new();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    window.Title = "Db Created";


  }


  private void InsertTestData()
  {
    Customer c1 = new Customer { Name = "Firma Berger", Latitude = 48.3352, Longitude = 14.5324 };
    Customer c2 = new Customer { Name = "Fam. Lehner", Latitude = 48.5136, Longitude = 14.1902 };

    Product p1 = new Product { Weight = 43, Price = 19, Description = "Platten A" };
    Product p2 = new Product { Weight = 46, Price = 22, Description = "Platten C" };
    Product p3 = new Product { Weight = 52, Price = 31, Description = "Platten B" };
    Product p4 = new Product { Weight = 2, Price = 10, Description = "Isolierung B" };
    Product p5 = new Product { Weight = 2, Price = 11, Description = "Isolierung C" };
    Product p6 = new Product { Weight = 1, Price = 8, Description = "Isolierung D" };
    Product p7 = new Product { Weight = 3, Price = 12, Description = "Isolierung A" };

    Order o1 = new Order
    {
      Description = "Plattenlieferung 1",
      OrderDate = DateTime.ParseExact("11.02.2021", "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
      CustomerId = 1
    };
    Order o2 = new Order
    {
      Description = "Hausisolierung 32",
      OrderDate = DateTime.ParseExact("12.02.2021", "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
      CustomerId = 2
    };
    Order o3 = new Order
    {
      Description = "Hausisolierung 33",
      OrderDate = DateTime.ParseExact("05.02.2021", "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
      CustomerId = 2
    };

    OrderDetail od1 = new OrderDetail { Amount = 15, OrderId = 1, ProductId = 1 };
    OrderDetail od2 = new OrderDetail { Amount = 20, OrderId = 1, ProductId = 2 };
    OrderDetail od3 = new OrderDetail { Amount = 30, OrderId = 3, ProductId = 3 };
    OrderDetail od4 = new OrderDetail { Amount = 60, OrderId = 2, ProductId = 4 };
    OrderDetail od5 = new OrderDetail { Amount = 20, OrderId = 2, ProductId = 5 };
    OrderDetail od6 = new OrderDetail { Amount = 20, OrderId = 2, ProductId = 6 };

    Employee e1 = new Employee { FirstName = "Hansi", LastName = "Huber" };
    Employee e2 = new Employee { FirstName = "Susi", LastName = "Maier" };
    Employee e3 = new Employee { FirstName = "Fritzi", LastName = "Müller" };
    Employee e4 = new Employee { FirstName = "Hansi", LastName = "Huber" };
    Employee e5 = new Employee { FirstName = "Hansi", LastName = "Huber" };
    Employee e6 = new Employee { FirstName = "Hansi", LastName = "Huber" };

  }
}
