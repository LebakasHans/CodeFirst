namespace CodeFirstDbLib;
public class Employee
{
  public int Id { get; set; }
  public string FirstName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string FullName => $"{FirstName} {LastName}";
  public List<Shipment> Shipments { get; set; } = [];
}
