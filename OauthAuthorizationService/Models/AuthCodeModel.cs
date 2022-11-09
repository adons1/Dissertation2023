using Core;

namespace OauthAuthorizationService.Models;

public class AuthCodeModel
{
    public Guid IssuerId { get; set; }
    public Guid AccepterId { get; set; }
    public DateTime IssueDate { get; set; }
    public string Code { get; set; }
}