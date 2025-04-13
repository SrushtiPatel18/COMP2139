namespace Assignment1.Models;

public class OrderItems
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Orders Order { get; set; }
    public int ProductId { get; set; }
    public Products Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}