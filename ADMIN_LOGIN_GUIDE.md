# Admin Login Guide

## How to Login to Admin Panel

### Step 1: Ensure Database is Set Up

First, make sure you've run the database schema:

```bash
mysql -u root -p threadifylabdb < Database/schema.sql
```

### Step 2: Default Admin Credentials

The database schema creates a default admin user with these credentials:

- **Username**: `admin`
- **Password**: `ChangeThisPassword123!`

**⚠️ IMPORTANT**: The password hash in the database might not match this password. You need to either:
1. Generate the correct password hash and update the database
2. Or create a new admin user with a known password

### Step 3: Generate Password Hash

The password is hashed using SHA256 and then Base64 encoded. To generate a hash for your password:

**Option A: Using C# Interactive (Recommended)**

1. Open a terminal in your project directory
2. Run:
```bash
dotnet interactive
```

3. Then run this code:
```csharp
using System.Security.Cryptography;
using System.Text;

var password = "YourNewPassword123!";
using var sha256 = SHA256.Create();
var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
Console.WriteLine($"Password: {password}");
Console.WriteLine($"Hash: {hash}");
Console.WriteLine($"\nSQL Update Command:");
Console.WriteLine($"UPDATE AdminUsers SET PasswordHash = '{hash}' WHERE Username = 'admin';");
```

**Option B: Using Online Tools**

1. Go to https://emn178.github.io/online-tools/sha256.html
2. Enter your password and get the SHA256 hash
3. Go to https://www.base64encode.org/
4. Encode the SHA256 hash to Base64
5. Update the database with the Base64 hash

### Step 4: Update Admin Password in Database

Once you have the hash, update the database:

```sql
UPDATE AdminUsers SET PasswordHash = 'your_generated_hash_here' WHERE Username = 'admin';
```

Or create a new admin user:

```sql
INSERT INTO AdminUsers (Username, PasswordHash, Email, CreatedAt) 
VALUES (
    'admin',
    'your_generated_hash_here',
    'admin@threadifylab.com',
    NOW()
);
```

### Step 5: Access Admin Login Page

1. Navigate to: `https://yourdomain.com/Admin/Login`
   - Or locally: `https://localhost:5001/Admin/Login`

2. Enter your credentials:
   - Username: `admin`
   - Password: The password you set in the database

3. Click "Login"

### Step 6: Verify Login

After successful login, you should be redirected to the Admin Dashboard at `/Admin/Index`

## Troubleshooting

### "Invalid username or password" Error

1. **Check if admin user exists:**
   ```sql
   SELECT * FROM AdminUsers WHERE Username = 'admin';
   ```

2. **Verify password hash:**
   - Make sure the hash in the database matches your password
   - The hash must be SHA256(password) → Base64

3. **Check database connection:**
   - Verify your connection string in `appsettings.json`
   - Ensure MySQL is running

### Admin User Doesn't Exist

If the admin user wasn't created, run this SQL:

```sql
-- First, generate a password hash for "admin123" (example)
-- Hash for "admin123": Use the C# code above to generate

INSERT INTO AdminUsers (Username, PasswordHash, Email, CreatedAt) 
VALUES (
    'admin',
    'WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=',  -- This is the hash for "admin123"
    'admin@threadifylab.com',
    NOW()
);
```

Then login with:
- Username: `admin`
- Password: `admin123`

### Quick Test Password Setup

For quick testing, you can use this pre-generated hash:

**Password**: `admin123`
**Hash**: `WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=`

```sql
UPDATE AdminUsers SET PasswordHash = 'WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=' WHERE Username = 'admin';
```

Then login with username `admin` and password `admin123`.

## Security Best Practices

1. **Change default password immediately** after first login
2. **Use strong passwords** (minimum 12 characters, mixed case, numbers, symbols)
3. **Don't share admin credentials**
4. **Regularly update passwords**
5. **Use HTTPS in production**

## Creating Additional Admin Users

To create additional admin users:

```sql
-- Generate hash for your password first, then:
INSERT INTO AdminUsers (Username, PasswordHash, Email, CreatedAt) 
VALUES (
    'newadmin',
    'your_generated_hash_here',
    'newadmin@threadifylab.com',
    NOW()
);
```










