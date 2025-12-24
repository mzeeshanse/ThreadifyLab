# PowerShell build script for Linux Ubuntu release
# This script builds the ThreadifyLab application for Linux deployment

Write-Host "Building ThreadifyLab for Linux Ubuntu..." -ForegroundColor Green

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean

# Restore packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore

# Build and publish for Linux x64 (Framework-dependent deployment)
# This requires .NET 8.0 Runtime to be installed on the target server
Write-Host "Publishing for Linux x64 (Framework-dependent)..." -ForegroundColor Yellow
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish/linux-x64

# Alternative: Self-contained deployment (includes .NET runtime, larger size)
# Uncomment the line below if you want a self-contained deployment
# dotnet publish -c Release -r linux-x64 --self-contained true -o ./publish/linux-x64-selfcontained

Write-Host ""
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host "Output directory: ./publish/linux-x64" -ForegroundColor Cyan
Write-Host ""
Write-Host "To deploy:" -ForegroundColor Yellow
Write-Host "1. Transfer the contents of ./publish/linux-x64 to your Ubuntu server"
Write-Host "2. Ensure .NET 8.0 Runtime is installed on the server"
Write-Host "3. Configure appsettings.json with your production settings"
Write-Host "4. Set up systemd service (see DEPLOYMENT.md)"
Write-Host ""

