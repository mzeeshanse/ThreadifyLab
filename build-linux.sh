#!/bin/bash

# Build script for Linux Ubuntu release
# This script builds the ThreadifyLab application for Linux deployment

echo "Building ThreadifyLab for Linux Ubuntu..."

# Clean previous builds
echo "Cleaning previous builds..."
dotnet clean

# Restore packages
echo "Restoring NuGet packages..."
dotnet restore

# Build and publish for Linux x64 (Framework-dependent deployment)
# This requires .NET 8.0 Runtime to be installed on the target server
echo "Publishing for Linux x64 (Framework-dependent)..."
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish/linux-x64

# Alternative: Self-contained deployment (includes .NET runtime, larger size)
# Uncomment the line below if you want a self-contained deployment
# dotnet publish -c Release -r linux-x64 --self-contained true -o ./publish/linux-x64-selfcontained

echo ""
echo "Build completed successfully!"
echo "Output directory: ./publish/linux-x64"
echo ""
echo "To deploy:"
echo "1. Transfer the contents of ./publish/linux-x64 to your Ubuntu server"
echo "2. Ensure .NET 8.0 Runtime is installed on the server"
echo "3. Configure appsettings.json with your production settings"
echo "4. Set up systemd service (see DEPLOYMENT.md)"
echo ""


