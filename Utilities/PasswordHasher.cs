using System.Security.Cryptography;
using System.Text;

namespace ThreadifyLab.Utilities;

public static class PasswordHasher
{
    /// <summary>
    /// Generates a SHA256 hash of the password and returns it as a Base64 string.
    /// This matches the hashing method used in AdminService.
    /// </summary>
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    /// <summary>
    /// Console utility to generate password hashes.
    /// Run: dotnet run --project ThreadifyLab.csproj -- hash "yourpassword"
    /// </summary>
    public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "hash" && args.Length > 1)
        {
            var hash = HashPassword(args[1]);
            Console.WriteLine($"Password: {args[1]}");
            Console.WriteLine($"Hash: {hash}");
            Console.WriteLine($"SQL: INSERT INTO AdminUsers (Username, PasswordHash, Email, CreatedAt) VALUES ('admin', '{hash}', 'admin@threadifylab.com', NOW());");
        }
        else
        {
            Console.WriteLine("Usage: dotnet run -- hash \"yourpassword\"");
        }
    }
}

