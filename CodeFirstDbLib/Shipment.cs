namespace CodeFirstDbLib;
public class Shipment
{
  public int Id { get; set; }
  public DateTime DeliveryDate { get; set; }
  public DateTime PlanDate { get; set; }
  public int SequenceNr { get; set; }
  public int EmployeeId { get; set; }
  public Employee Employee { get; set; } = null!;
  public List<Order> Orders { get; set; } = [];
}
