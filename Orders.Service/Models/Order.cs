namespace Orders.Service.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}
