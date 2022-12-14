namespace CustomersService.Models;

public class CustomerIdentity
{
    public int Id { get; set; }
    public Customer Customer { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
