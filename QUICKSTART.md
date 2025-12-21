# Quick Start Guide

## Initial Setup (5 minutes)

### 1. Database Setup

```bash
# Create database
mysql -u root -p < Database/schema.sql
```

### 2. Configure Connection String

Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ThreadifyLabDB;User=root;Password=yourpassword;Port=3306;CharSet=utf8mb4;"
  }
}
```

### 3. Generate Admin Password Hash

**Option A: Using C# Interactive (Recommended)**
```bash
dotnet interactive
```
Then run:
```csharp
using System.Security.Cryptography;
using System.Text;
var password = "YourSecurePassword123!";
using var sha256 = SHA256.Create();
var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
Console.WriteLine($"UPDATE AdminUsers SET PasswordHash = '{hash}' WHERE Username = 'admin';");
```

**Option B: Online Tool**
1. Go to https://emn178.github.io/online-tools/sha256.html
2. Enter your password and get SHA256 hash
3. Go to https://www.base64encode.org/
4. Encode the SHA256 hash to Base64
5. Update the database:
```sql
UPDATE AdminUsers SET PasswordHash = 'your_base64_hash' WHERE Username = 'admin';
```

### 4. Run the Application

```bash
dotnet restore
dotnet run
```

Visit: https://localhost:5001

### 5. Login to Admin Panel

- URL: https://localhost:5001/Admin/Login
- Username: `admin`
- Password: The password you set in step 3

## First Steps After Login

1. **Change Admin Password** (if you used the default)
2. **Add Your Services** - Go to Admin > Services > Add New Service
3. **Add Portfolio Items** - Go to Admin > Portfolio > Add New Portfolio Item
4. **Customize Content** - Update About page, contact info, etc.

## Default Sample Data

The database schema includes sample services and portfolio items. You can:
- Edit them in the admin panel
- Delete them and add your own
- Keep them as examples

## Next Steps

- See [README.md](README.md) for full documentation
- See [DEPLOYMENT.md](DEPLOYMENT.md) for Ubuntu deployment instructions
- Customize colors in `wwwroot/css/site.css` (CSS variables in `:root`)

## Troubleshooting

**Can't connect to database?**
- Check MySQL is running: `sudo systemctl status mysql`
- Verify connection string in `appsettings.json`
- Test connection: `mysql -u root -p ThreadifyLabDB`

**Admin login not working?**
- Verify password hash in database matches your password
- Check database: `SELECT Username, PasswordHash FROM AdminUsers;`
- Regenerate hash and update database

**Port already in use?**
- Change port in `Properties/launchSettings.json`
- Or use: `dotnet run --urls "http://localhost:5002"`

