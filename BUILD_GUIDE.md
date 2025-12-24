# Build Guide for Linux Ubuntu Release

This guide explains how to build and package the ThreadifyLab application for deployment on Linux Ubuntu.

## Prerequisites

- .NET 8.0 SDK installed on your development machine
- Access to the project source code

## Build Options

### Option 1: Framework-Dependent Deployment (Recommended)

This creates a smaller package that requires .NET 8.0 Runtime to be installed on the target server.

**Windows (PowerShell):**
```powershell
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish/linux-x64
```

**Linux/Mac:**
```bash
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish/linux-x64
```

**Using the build script:**
```bash
# Linux/Mac
chmod +x build-linux.sh
./build-linux.sh

# Windows PowerShell
.\build-linux.ps1
```

### Option 2: Self-Contained Deployment

This includes the .NET runtime in the package (larger size, ~70-100 MB), but doesn't require .NET to be installed on the server.

```bash
dotnet publish -c Release -r linux-x64 --self-contained true -o ./publish/linux-x64-selfcontained
```

## Build Output

After building, you'll find the following in the `publish/linux-x64` directory:

```
publish/linux-x64/
├── ThreadifyLab.dll          # Main application DLL
├── ThreadifyLab               # Executable (Linux)
├── appsettings.json          # Configuration file
├── appsettings.Development.json
├── wwwroot/                  # Static files (CSS, JS, images)
├── Views/                    # Razor views
└── [other dependencies]
```

## Runtime Identifiers (RID)

For different Linux distributions, you can use:

- `linux-x64` - Most common (Ubuntu, Debian, CentOS, etc.)
- `linux-arm64` - ARM-based systems (Raspberry Pi, AWS Graviton)
- `linux-musl-x64` - Alpine Linux (smaller, but may have compatibility issues)

## Quick Build Commands

### Standard Release Build
```bash
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
```

### Release Build with Single File
```bash
dotnet publish -c Release -r linux-x64 --self-contained false -p:PublishSingleFile=true -o ./publish
```

### Release Build with Trimming (Smaller Size)
```bash
dotnet publish -c Release -r linux-x64 --self-contained false -p:PublishTrimmed=true -o ./publish
```

## Deployment Steps

1. **Build the application** using one of the commands above
2. **Transfer files** to your Ubuntu server:
   ```bash
   scp -r ./publish/* user@your-server:/var/www/threadifylab/
   ```
3. **Install .NET Runtime** on the server (if using framework-dependent):
   ```bash
   sudo apt-get update
   sudo apt-get install -y aspnetcore-runtime-8.0
   ```
4. **Configure** `appsettings.json` with production settings
5. **Set up systemd service** (see DEPLOYMENT.md)
6. **Start the service**:
   ```bash
   sudo systemctl start threadifylab
   ```

## File Size Comparison

- **Framework-dependent**: ~15-25 MB
- **Self-contained**: ~70-100 MB
- **Self-contained + Single File**: ~60-80 MB (single executable)

## Troubleshooting

### Build Fails
- Ensure .NET 8.0 SDK is installed: `dotnet --version`
- Restore packages: `dotnet restore`
- Clean and rebuild: `dotnet clean && dotnet build`

### Runtime Errors on Server
- Verify .NET Runtime is installed: `dotnet --list-runtimes`
- Check file permissions: `chmod +x ThreadifyLab`
- Review logs: `journalctl -u threadifylab -f`

### Missing Dependencies
- For framework-dependent: Install `aspnetcore-runtime-8.0`
- For self-contained: Should work without additional dependencies

## Next Steps

After building, follow the [DEPLOYMENT.md](./DEPLOYMENT.md) guide for server setup and configuration.

