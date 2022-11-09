namespace CustomersService.TransportTypes.TransportModels;

public class TokenModel
{
    public Guid ClientId { get; set; }
    public DateTime IssueDate { get; set; }
    public string Token { get; set; }
}
