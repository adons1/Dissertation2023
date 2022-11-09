using System.Security.Cryptography;
using System.Text;

namespace Core.Cryptography;

public class SHA256
{
    public static string Hash(string plainText)
    {
        using (var crypt = System.Security.Cryptography.SHA256.Create())
        {
            return Convert.ToBase64String(crypt.ComputeHash(Encoding.UTF8.GetBytes(plainText)));
        }
    }
}
