using LinqToDB.Mapping;

namespace OauthAuthorizationService.Models;

[Table("Services")]
public class Service
{
    [Column("ID"), NotNull]
    public Guid Id { get; set; }
    [Column("Alias"), NotNull]
    public string Alias { get; set; }
    [Column("Password"), NotNull]
    public string Password { get; set; }
}
