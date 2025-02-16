namespace Assignment1.Models;

public class Orders
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItems> OrderItems { get; set; }
}