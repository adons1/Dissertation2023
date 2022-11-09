namespace CustomersService.TransportTypes.TransportModels;

public class RegisterCustomer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime Birthday { get; set; }
    public double Account { get; set; }
}
