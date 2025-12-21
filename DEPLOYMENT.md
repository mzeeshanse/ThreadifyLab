# Deployment Guide for ThreadifyLab on Ubuntu

This guide will help you deploy ThreadifyLab to an Ubuntu server.

## Prerequisites

- Ubuntu 20.04 LTS or later
- Root or sudo access
- Domain name pointing to your server (threadifylab.com)
- MySQL Server installed

## Step 1: Install .NET 8.0 Runtime

```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET 8.0 Runtime
sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-8.0
```

## Step 2: Install MySQL

```bash
sudo apt-get update
sudo apt-get install -y mysql-server
sudo mysql_secure_installation
```

## Step 3: Create Database

```bash
# Login to MySQL
sudo mysql -u root -p

# Create database and user
CREATE DATABASE ThreadifyLabDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'threadifylab'@'localhost' IDENTIFIED BY 'your_secure_password';
GRANT ALL PRIVILEGES ON ThreadifyLabDB.* TO 'threadifylab'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

## Step 4: Import Database Schema

```bash
# Copy schema file to server (or create it)
mysql -u threadifylab -p ThreadifyLabDB < Database/schema.sql
```

## Step 5: Build and Publish Application

On your development machine:

```bash
# Restore packages
dotnet restore

# Publish for Linux
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
```

## Step 6: Transfer Files to Server

```bash
# Using SCP (from your local machine)
scp -r ./publish/* user@your-server:/var/www/threadifylab/

# Or use SFTP, rsync, or any file transfer method
```

## Step 7: Configure Application

On the server, edit `/var/www/threadifylab/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ThreadifyLabDB;User=threadifylab;Password=your_secure_password;Port=3306;CharSet=utf8mb4;"
  }
}
```

## Step 8: Create Systemd Service

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
Environment=ASPNETCORE_URLS=http://localhost:5000

[Install]
WantedBy=multi-user.target
```

Enable and start the service:

```bash
sudo systemctl daemon-reload
sudo systemctl enable threadifylab
sudo systemctl start threadifylab
sudo systemctl status threadifylab
```

## Step 9: Install and Configure Nginx

```bash
sudo apt-get install -y nginx
```

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

## Step 10: Configure SSL with Let's Encrypt

```bash
sudo apt-get install -y certbot python3-certbot-nginx
sudo certbot --nginx -d threadifylab.com -d www.threadifylab.com
```

Certbot will automatically configure Nginx for HTTPS.

## Step 11: Set File Permissions

```bash
sudo chown -R www-data:www-data /var/www/threadifylab
sudo chmod -R 755 /var/www/threadifylab
```

## Step 12: Configure Firewall

```bash
sudo ufw allow 22/tcp
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw enable
```

## Troubleshooting

### Check Application Logs
```bash
sudo journalctl -u threadifylab -f
```

### Check Nginx Logs
```bash
sudo tail -f /var/log/nginx/error.log
```

### Test Database Connection
```bash
mysql -u threadifylab -p ThreadifyLabDB -e "SELECT COUNT(*) FROM Services;"
```

### Restart Services
```bash
sudo systemctl restart threadifylab
sudo systemctl restart nginx
```

## Maintenance

### Update Application
1. Build new version locally
2. Transfer files to server
3. Restart service: `sudo systemctl restart threadifylab`

### Backup Database
```bash
mysqldump -u threadifylab -p ThreadifyLabDB > backup_$(date +%Y%m%d).sql
```

### Restore Database
```bash
mysql -u threadifylab -p ThreadifyLabDB < backup_20240101.sql
```

## Security Checklist

- [ ] Changed default admin password
- [ ] Using strong MySQL password
- [ ] SSL certificate installed and working
- [ ] Firewall configured
- [ ] Regular backups scheduled
- [ ] .NET Core and system packages updated
- [ ] File permissions set correctly
- [ ] Connection string uses secure credentials

