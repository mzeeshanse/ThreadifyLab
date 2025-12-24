//using System.Security.Cryptography;
//using System.Text;

//// Quick script to generate admin password hash
//// This matches the AdminService.HashPassword method

//Console.WriteLine("=== Admin Password Hash Generator ===\n");

//// Default password from schema
//var defaultPassword = "ChangeThisPassword123!";

//// Generate hash using SHA256 then Base64 (same as AdminService)
//using var sha256 = SHA256.Create();
//var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(defaultPassword));
//var hash = Convert.ToBase64String(hashedBytes);

//Console.WriteLine($"Default Password: {defaultPassword}");
//Console.WriteLine($"Generated Hash: {hash}");
//Console.WriteLine("\n=== SQL Command ===");
//Console.WriteLine("Run this in MySQL to set the admin password:");
//Console.WriteLine($"UPDATE AdminUsers SET PasswordHash = '{hash}' WHERE Username = 'admin';");
//Console.WriteLine("\nOr if admin user doesn't exist:");
//Console.WriteLine($"INSERT INTO AdminUsers (Username, PasswordHash, Email, CreatedAt) VALUES ('admin', '{hash}', 'admin@threadifylab.com', NOW());");

//Console.WriteLine("\n=== Login Credentials ===");
//Console.WriteLine("Username: admin");
//Console.WriteLine($"Password: {defaultPassword}");
//Console.WriteLine("\n⚠️ IMPORTANT: Change this password after first login!");









