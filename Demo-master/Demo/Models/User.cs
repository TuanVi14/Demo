using System.Security.Cryptography;
using System.Text;

namespace Demo.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    // Comma-separated roles, e.g. "Admin,Manager"
    public string Roles { get; set; }

    public IEnumerable<string> GetRoleList()
    {
        if (string.IsNullOrWhiteSpace(Roles)) yield break;
        foreach (var r in Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            yield return r;
    }

    public static string HashPassword(string password)
    {
        if (password == null) return string.Empty;
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
