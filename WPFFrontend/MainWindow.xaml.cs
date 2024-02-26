using CodeFirstDbLib;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace WPFFrontend;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private NorthwindLiteContext db;
    //this is perfect the way it is
    private int shipmentCounter = 1;
    private Employee selectedEmployeeOuter;

    public MainWindow() => InitializeComponent();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        db = new();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        window.Title = "Db Created";

        InsertTestData();
        PopulateTreeView();
        PopulateProducts();
        PopulateEmployeeAutocomplete("");
    }

    private void PopulateEmployeeAutocomplete(string filter)
    {
        lbxEmployeeAutocomplete.ItemsSource = db.Employees.Where(e => e.LastName.Contains(filter)).ToList();
    }

    private void PopulateProducts()
    {
        var products = db.Products.ToList();
        lbxProducts.ItemsSource = products;
    }

    private void PopulateShipmentGrid()
    {
        grdShipment.Items.Clear();

        if (selectedEmployeeOuter == null) return;

        foreach (var shipment in selectedEmployeeOuter.Shipments)
        {
            foreach (var order in shipment.Orders)
            {
                var customer = db.Customers.Find(order.CustomerId);
                var row = new ShipmentRow(EmployeeName: selectedEmployeeOuter.FullName, Order: order.Description, CustomerName: customer.Name, SequenceNr: shipment.SequenceNr, DeliveryDate: "", PlanDate: shipment.PlanDate.ToString("dd.MM.yyyy"));
                grdShipment.Items.Add(row);
            }
        }
    }

    private void PopulateTreeView()
    {
        //ChatGPT und der Tag gehört dir
        trvCustomers.Items.Clear();
        var rootNode = new TreeViewItem { Header = "Kunden" };
        trvCustomers.Items.Add(rootNode);

        var customers = db.Customers.Include(c => c.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.Product).ToList();
        foreach (var c in customers)
        {
            var customerNode = new TreeViewItem { Header = c.Name };
            rootNode.Items.Add(customerNode);
            foreach (var o in c.Orders)
            {
                var orderNode = new TreeViewItem { Header = $"{o.Description} vom {o.OrderDate:dd.MM.yyyy}", Tag = o };
                customerNode.Items.Add(orderNode);
                foreach (var od in o.OrderDetails)
                {
                    var odNode = new TreeViewItem { Header = $"{od.Amount} {od.Product.Description} zu je {od.Product.Price}€" };
                    orderNode.Items.Add(odNode);
                }
            }
        }
    }

    private void lbxEmployeeAutocomplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedEmployee = lbxEmployeeAutocomplete.SelectedItem as Employee;

        if (selectedEmployee == null) return;

        txtEmployee.Text = selectedEmployee.FullName;
        selectedEmployeeOuter = selectedEmployee;

        PopulateShipmentGrid();
    }

    private void txtEmployee_TextChanged(object sender, TextChangedEventArgs e)
    {
        grdShipment.Items.Clear();
        selectedEmployeeOuter = null;
        var filter = txtEmployee.Text;
        PopulateEmployeeAutocomplete(filter);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var productName = txtProductName.Text;

        if (string.IsNullOrWhiteSpace(productName)) return;

        var rnd = new Random();
        var newProduct = new Product { Description = productName, Price = rnd.Next(1, 100), Weight = rnd.Next(1, 100) };

        db.Products.Add(newProduct);
        db.SaveChanges();

        PopulateProducts();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        DateTime date;
        var planDate = txtDate.Text;
        var isDateValid = DateTime.TryParse(planDate, out date);

        if (!isDateValid || selectedEmployeeOuter == null) return;

        var newShipment = new Shipment { PlanDate = date, EmployeeId = selectedEmployeeOuter.Id, SequenceNr = shipmentCounter };
        var shipment = db.Shipments.Add(newShipment).Entity;
        db.SaveChanges();

        shipmentCounter++;

        var selectedOrdersItem = trvCustomers.SelectedItem as TreeViewItem;
        if (selectedOrdersItem == null || selectedOrdersItem.Tag == null) return;

        var selectedOrder = selectedOrdersItem.Tag as Order;
        if (selectedOrder == null) return;

        db.Orders.Find(selectedOrder.Id).ShipmentId = shipment.Id;
        db.SaveChanges();

        PopulateShipmentGrid();
    }

    private void trvCustomers_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {

        var selectedOrdersItem = trvCustomers.SelectedItem as TreeViewItem;
        if (selectedOrdersItem == null || selectedOrdersItem.Tag == null) return;

        var selectedOrder = selectedOrdersItem.Tag as Order;
        if (selectedOrder == null) return;

        window.Title = selectedOrder.ShipmentId + "aaaaaaaaaaaaaaaaaa";
    }

    public record ShipmentRow(string EmployeeName, string Order, string CustomerName, int SequenceNr, string DeliveryDate, string PlanDate);

    private void InsertTestData()
    {
        Customer c1 = new Customer { Name = "Firma Berger", Latitude = 48.3352, Longitude = 14.5324 };
        Customer c2 = new Customer { Name = "Fam. Lehner", Latitude = 48.5136, Longitude = 14.1902 };
        db.Customers.AddRange(c1, c2);
        db.SaveChanges();

        Product p1 = new Product { Weight = 43, Price = 19, Description = "Platten A" };
        Product p2 = new Product { Weight = 46, Price = 22, Description = "Platten C" };
        Product p3 = new Product { Weight = 52, Price = 31, Description = "Platten B" };
        Product p4 = new Product { Weight = 2, Price = 10, Description = "Isolierung B" };
        Product p5 = new Product { Weight = 2, Price = 11, Description = "Isolierung C" };
        Product p6 = new Product { Weight = 1, Price = 8, Description = "Isolierung D" };
        Product p7 = new Product { Weight = 3, Price = 12, Description = "Isolierung A" };
        db.Products.AddRange(p1, p2, p3, p4, p5, p6, p7);
        db.SaveChanges();

        Employee e1 = new Employee { FirstName = "Hansi", LastName = "Huber" };
        Employee e2 = new Employee { FirstName = "Susi", LastName = "Maier" };
        Employee e3 = new Employee { FirstName = "Fritzi", LastName = "Müller" };
        Employee e4 = new Employee { FirstName = "Franzi", LastName = "Hehenberger" };
        Employee e5 = new Employee { FirstName = "Pauli", LastName = "Gruber" };
        Employee e6 = new Employee { FirstName = "Elfi", LastName = "Gerber" };
        Employee e7 = new Employee { FirstName = "Maxi", LastName = "Moser" };
        db.Employees.AddRange(e1, e2, e3, e4, e5, e6, e7);
        db.SaveChanges();

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
        db.Orders.AddRange(o1, o2, o3);
        db.SaveChanges();

        OrderDetail od1 = new OrderDetail { Amount = 15, OrderId = 1, ProductId = 1 };
        OrderDetail od2 = new OrderDetail { Amount = 20, OrderId = 1, ProductId = 2 };
        OrderDetail od3 = new OrderDetail { Amount = 30, OrderId = 3, ProductId = 3 };
        OrderDetail od4 = new OrderDetail { Amount = 60, OrderId = 2, ProductId = 4 };
        OrderDetail od5 = new OrderDetail { Amount = 20, OrderId = 2, ProductId = 5 };
        OrderDetail od6 = new OrderDetail { Amount = 20, OrderId = 2, ProductId = 6 };
        db.OrderDetails.AddRange(od1, od2, od3, od4, od5, od6);
        db.SaveChanges();

    }
}
