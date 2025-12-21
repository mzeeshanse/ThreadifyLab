//// Quick script to generate password hash for admin user
//// Run with: dotnet script Scripts/GeneratePasswordHash.cs "YourPassword"

//using System.Security.Cryptography;
//using System.Text;

//if (args.Length == 0)
//{
//    Console.WriteLine("Usage: dotnet script GeneratePasswordHash.cs \"YourPassword\"");
//    Environment.Exit(1);
//}

//var password = args[0];
//using var sha256 = SHA256.Create();
//var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//var hash = Convert.ToBase64String(hashedBytes);

//Console.WriteLine("\n=== Password Hash Generator ===");
//Console.WriteLine($"Password: {password}");
//Console.WriteLine($"Hash (Base64): {hash}");
//Console.WriteLine("\n=== SQL Update Statement ===");
//Console.WriteLine($"UPDATE AdminUsers SET PasswordHash = '{hash}' WHERE Username = 'admin';");
//Console.WriteLine("\n=== Or for new user ===");
//Console.WriteLine($"INSERT INTO AdminUsers (Username, PasswordHash, Email, CreatedAt) VALUES ('admin', '{hash}', 'admin@threadifylab.com', NOW());");

