namespace OauthAuthorizationService.Models;

public class TokenModel
{
    public Guid IssuerId { get; set; }
    public Guid AccepterId { get; set; }
    public DateTime IssueDate { get; set; }
    public string Token { get; set; }
}
