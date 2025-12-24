# Ubuntu Server Setup Steps

This guide covers the steps needed on your Ubuntu server after you've placed the application files at `/var/www/threadifylab.com/html/`.

## Step 1: Install .NET 8.0 Runtime

If you used framework-dependent deployment, install the .NET runtime:

```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Update package list
sudo apt-get update

# Install .NET 8.0 Runtime
sudo apt-get install -y aspnetcore-runtime-8.0

# Verify installation
dotnet --version
```

## Step 2: Install MySQL (if not already installed)

```bash
sudo apt-get update
sudo apt-get install -y mysql-server

# Secure MySQL installation
sudo mysql_secure_installation
```

## Step 3: Create Database and User

```bash
# Login to MySQL
sudo mysql -u root -p

# In MySQL prompt, run:
CREATE DATABASE threadifylabdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'threadifylabuser'@'localhost' IDENTIFIED BY 'your_secure_password';
GRANT ALL PRIVILEGES ON threadifylabdb.* TO 'threadifylabuser'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

## Step 4: Import Database Schema

```bash
# If you have the schema.sql file, import it:
mysql -u threadifylabuser -p threadifylabdb < /path/to/schema.sql

# Or create tables manually using the schema.sql file content
```

## Step 5: Configure appsettings.json

Edit the configuration file:

```bash
sudo nano /var/www/threadifylab.com/html/appsettings.json
```

Update the connection string and other settings:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=threadifylabdb;User=threadifylabuser;Password=your_secure_password;Port=3306;CharSet=utf8mb4;"
  },
  "EmailSettings": {
    "SmtpHost": "smtp.zoho.com",
    "SmtpPort": "587",
    "SmtpUser": "zeeshan@threadifylab.com",
    "SmtpPassword": "your_email_password",
    "FromEmail": "zeeshan@threadifylab.com",
    "FromName": "ThreadifyLab"
  },
  "AppSettings": {
    "BaseUrl": "https://threadifylab.com"
  }
}
```

## Step 6: Set File Permissions

```bash
# Set ownership to www-data user
sudo chown -R www-data:www-data /var/www/threadifylab.com/html

# Set directory permissions
sudo chmod -R 755 /var/www/threadifylab.com/html

# Make the executable file executable
sudo chmod +x /var/www/threadifylab.com/html/ThreadifyLab
```

## Step 7: Create Systemd Service

Create the service file:

```bash
sudo nano /etc/systemd/system/threadifylab.service
```

Add the following content:

```ini
[Unit]
Description=ThreadifyLab Website
After=network.target mysql.service

[Service]
Type=notify
User=www-data
WorkingDirectory=/var/www/threadifylab.com/html
ExecStart=/usr/bin/dotnet /var/www/threadifylab.com/html/ThreadifyLab.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=threadifylab
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=ASPNETCORE_URLS=http://localhost:5000

[Install]
WantedBy=multi-user.target
```

**Note:** If you used self-contained deployment, change ExecStart to:
```ini
ExecStart=/var/www/threadifylab.com/html/ThreadifyLab
```

## Step 8: Enable and Start the Service

```bash
# Reload systemd daemon
sudo systemctl daemon-reload

# Enable service to start on boot
sudo systemctl enable threadifylab

# Start the service
sudo systemctl start threadifylab

# Check status
sudo systemctl status threadifylab
```

## Step 9: Install and Configure Nginx

```bash
# Install Nginx
sudo apt-get install -y nginx

# Create Nginx configuration
sudo nano /etc/nginx/sites-available/threadifylab.com
```

Add the following configuration:

```nginx
server {
    listen 80;
    server_name threadifylab.com www.threadifylab.com;

    # Redirect HTTP to HTTPS (after SSL setup)
    # return 301 https://$server_name$request_uri;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Real-IP $remote_addr;
    }

    # Serve static files directly (optional, for better performance)
    location ~* \.(css|js|jpg|jpeg|png|gif|ico|svg|woff|woff2|ttf|eot)$ {
        root /var/www/threadifylab.com/html/wwwroot;
        expires 30d;
        add_header Cache-Control "public, immutable";
    }
}
```

Enable the site:

```bash
# Create symbolic link
sudo ln -s /etc/nginx/sites-available/threadifylab.com /etc/nginx/sites-enabled/

# Remove default site (optional)
sudo rm /etc/nginx/sites-enabled/default

# Test Nginx configuration
sudo nginx -t

# Reload Nginx
sudo systemctl reload nginx
```

## Step 10: Configure Firewall

```bash
# Allow SSH (important - don't lock yourself out!)
sudo ufw allow 22/tcp

# Allow HTTP
sudo ufw allow 80/tcp

# Allow HTTPS
sudo ufw allow 443/tcp

# Enable firewall
sudo ufw enable

# Check status
sudo ufw status
```

## Step 11: Set Up SSL Certificate (Let's Encrypt)

```bash
# Install Certbot
sudo apt-get install -y certbot python3-certbot-nginx

# Obtain SSL certificate
sudo certbot --nginx -d threadifylab.com -d www.threadifylab.com

# Certbot will automatically configure Nginx for HTTPS
# It will also set up auto-renewal
```

## Step 12: Verify Everything Works

```bash
# Check application is running
sudo systemctl status threadifylab

# Check Nginx is running
sudo systemctl status nginx

# View application logs
sudo journalctl -u threadifylab -f

# Test database connection
mysql -u threadifylabuser -p threadifylabdb -e "SHOW TABLES;"

# Test website
curl http://localhost:5000
```

## Troubleshooting

### Application won't start

```bash
# Check logs
sudo journalctl -u threadifylab -n 50

# Check if port 5000 is in use
sudo netstat -tlnp | grep 5000

# Verify file permissions
ls -la /var/www/threadifylab.com/html/
```

### Database connection errors

```bash
# Test MySQL connection
mysql -u threadifylabuser -p threadifylabdb

# Check MySQL is running
sudo systemctl status mysql

# Verify connection string in appsettings.json
cat /var/www/threadifylab.com/html/appsettings.json | grep ConnectionString
```

### Nginx 502 Bad Gateway

```bash
# Check if application is running
sudo systemctl status threadifylab

# Check application logs
sudo journalctl -u threadifylab -f

# Verify port in systemd service matches Nginx proxy_pass
```

### Permission denied errors

```bash
# Fix ownership
sudo chown -R www-data:www-data /var/www/threadifylab.com/html

# Fix permissions
sudo chmod -R 755 /var/www/threadifylab.com/html
sudo chmod +x /var/www/threadifylab.com/html/ThreadifyLab
```

## Quick Commands Reference

```bash
# Restart application
sudo systemctl restart threadifylab

# Restart Nginx
sudo systemctl restart nginx

# View application logs
sudo journalctl -u threadifylab -f

# View Nginx logs
sudo tail -f /var/log/nginx/error.log

# Check service status
sudo systemctl status threadifylab
sudo systemctl status nginx
sudo systemctl status mysql
```

## Next Steps

1. Access your website at `https://threadifylab.com`
2. Login to admin panel at `https://threadifylab.com/Admin/Login`
3. Set up regular database backups
4. Monitor logs regularly
5. Keep .NET runtime and system packages updated

## Security Checklist

- [ ] Changed default admin password
- [ ] Using strong MySQL password
- [ ] SSL certificate installed and working
- [ ] Firewall configured
- [ ] File permissions set correctly
- [ ] Connection string uses secure credentials
- [ ] Regular backups scheduled

