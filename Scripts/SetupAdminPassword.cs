//using System.Security.Cryptography;
//using System.Text;

//// Simple utility to generate admin password hash
//// Usage: dotnet run --project ThreadifyLab.csproj

//Console.WriteLine("=== Admin Password Hash Generator ===\n");

//Console.Write("Enter password for admin user: ");
//var password = Console.ReadLine();

//if (string.IsNullOrEmpty(password))
//{
//    Console.WriteLine("Password cannot be empty!");
//    Environment.Exit(1);
//}

//// Generate hash using the same method as AdminService
//using var sha256 = SHA256.Create();
//var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//var hash = Convert.ToBase64String(hashedBytes);

//Console.WriteLine("\n=== Generated Hash ===");
//Console.WriteLine($"Password: {password}");
//Console.WriteLine($"Hash: {hash}");

//Console.WriteLine("\n=== SQL Commands ===");
//Console.WriteLine("Option 1: Update existing admin user:");
//Console.WriteLine($"UPDATE AdminUsers SET PasswordHash = '{hash}' WHERE Username = 'admin';");
//Console.WriteLine("\nOption 2: Insert new admin user:");
//Console.WriteLine($"INSERT INTO AdminUsers (Username, PasswordHash, Email, CreatedAt) VALUES ('admin', '{hash}', 'admin@threadifylab.com', NOW());");

//Console.WriteLine("\n=== Next Steps ===");
//Console.WriteLine("1. Run the SQL command in your MySQL database");
//Console.WriteLine("2. Login at /Admin/Login with username 'admin' and your password");










