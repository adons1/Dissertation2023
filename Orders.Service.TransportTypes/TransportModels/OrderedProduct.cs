namespace Orders.TransportTypes.TransportModels;

public class OrderedProduct
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
}
