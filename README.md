# ThreadifyLab - Professional Embroidery Digitizing Website

A modern, responsive ASP.NET Core 8.0 MVC website for ThreadifyLab digitizing business, featuring a dark mode design and comprehensive admin panel.

## Features

- **Responsive Dark Mode Design**: Beautiful dark theme optimized for all devices
- **SEO Optimized**: Meta tags, semantic HTML, and proper structure
- **Admin Panel**: Full content management system for services, portfolio, and messages
- **MySQL Database**: Using Dapper for data access (no Entity Framework)
- **Contact Form**: Customer inquiry management system
- **Portfolio Showcase**: Display your best work
- **Service Pages**: Showcase your digitizing services

## Technology Stack

- ASP.NET Core 8.0 MVC
- MySQL Database
- Dapper ORM
- HTML5, CSS3, JavaScript
- Font Awesome Icons

## Prerequisites

- .NET 8.0 SDK
- MySQL Server 8.0 or later
- Ubuntu Server (for deployment)

## Installation

### 1. Database Setup

```bash
# Login to MySQL
mysql -u root -p

# Run the schema file
mysql -u root -p < Database/schema.sql
```

### 2. Configure Connection String

Edit `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ThreadifyLabDB;User=youruser;Password=yourpassword;Port=3306;CharSet=utf8mb4;"
  }
}
```

### 3. Update Admin Password

The default admin credentials are:
- Username: `admin`
- Password: `ChangeThisPassword123!`

**IMPORTANT**: Change the admin password immediately after first login!

To generate a new password hash:

**Option 1: Using C# Interactive**
```csharp
using System.Security.Cryptography;
using System.Text;
var password = "YourNewPassword123!";
using var sha256 = SHA256.Create();
var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
Console.WriteLine(hash);
```

**Option 2: Update directly in MySQL**
After creating the admin user, you can update the password hash using the AdminService method or by running a simple C# script.

**Option 3: Use online SHA256 + Base64 encoder**
1. Hash your password with SHA256
2. Encode the result to Base64
3. Update the AdminUsers table: `UPDATE AdminUsers SET PasswordHash = 'your_hash' WHERE Username = 'admin';`

### 4. Run the Application

```bash
dotnet restore
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## Deployment on Ubuntu

### 1. Publish the Application

```bash
dotnet publish -c Release -o /var/www/threadifylab
```

### 2. Create Systemd Service

Create `/etc/systemd/system/threadifylab.service`:

```ini
[Unit]
Description=ThreadifyLab Website
After=network.target mysql.service

[Service]
Type=notify
User=www-data
WorkingDirectory=/var/www/threadifylab
ExecStart=/usr/bin/dotnet /var/www/threadifylab/ThreadifyLab.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=threadifylab
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

### 3. Configure Nginx

Create `/etc/nginx/sites-available/threadifylab`:

```nginx
server {
    listen 80;
    server_name threadifylab.com www.threadifylab.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

Enable the site:
```bash
sudo ln -s /etc/nginx/sites-available/threadifylab /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

### 4. SSL Certificate (Let's Encrypt)

```bash
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d threadifylab.com -d www.threadifylab.com
```

### 5. Start the Service

```bash
sudo systemctl enable threadifylab
sudo systemctl start threadifylab
sudo systemctl status threadifylab
```

## Project Structure

```
ThreadifyLab/
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs
│   ├── ContactController.cs
│   └── AdminController.cs
├── Models/              # Data Models
│   ├── Service.cs
│   ├── PortfolioItem.cs
│   ├── ContactMessage.cs
│   └── AdminUser.cs
├── Repositories/        # Dapper Repositories
│   ├── ServiceRepository.cs
│   ├── PortfolioRepository.cs
│   ├── ContactRepository.cs
│   └── AdminRepository.cs
├── Services/            # Business Logic
│   └── AdminService.cs
├── Views/               # Razor Views
│   ├── Home/
│   ├── Admin/
│   └── Shared/
├── wwwroot/             # Static Files
│   ├── css/
│   └── js/
├── Database/            # Database Schema
│   └── schema.sql
└── Program.cs           # Application Entry Point
```

## Admin Panel

Access the admin panel at `/Admin/Login`

Features:
- Dashboard with statistics
- Manage Services (CRUD)
- Manage Portfolio Items (CRUD)
- View and manage Contact Messages
- Secure authentication

## Customization

### Adding New Services

1. Login to admin panel
2. Navigate to Services
3. Click "Add New Service"
4. Fill in the form (use Font Awesome icon classes)

### Adding Portfolio Items

1. Login to admin panel
2. Navigate to Portfolio
3. Click "Add New Portfolio Item"
4. Upload images to your server or use external URLs
5. Fill in the details

### Changing Colors

Edit `wwwroot/css/site.css` and modify the CSS variables in `:root`:

```css
:root {
    --primary-color: #6366f1;
    --secondary-color: #8b5cf6;
    --accent-color: #ec4899;
    /* ... */
}
```

## Security Notes

- Change default admin password immediately
- Use strong passwords for MySQL
- Keep .NET Core updated
- Regularly backup the database
- Use HTTPS in production
- Review and update connection strings

## Support

For issues or questions, please contact: info@threadifylab.com

## License

Copyright © 2024 ThreadifyLab. All rights reserved.

