using System.Security.Cryptography;
using System.Text;

namespace ThreadifyLab.Services;

public class ImageUploadService : IImageUploadService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

    public ImageUploadService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile, string folderName = "images")
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            throw new ArgumentException("No file provided");
        }

        if (!IsValidImageFile(imageFile))
        {
            throw new ArgumentException("Invalid image file. Allowed formats: JPG, JPEG, PNG, GIF, WEBP. Max size: 5MB");
        }

        // Create folder path: wwwroot/images/[folderName]
        var uploadFolder = Path.Combine(_environment.WebRootPath, folderName);
        
        // Ensure directory exists (works on both Windows and Linux)
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        // Generate unique filename to avoid conflicts
        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        var fileName = GenerateUniqueFileName() + extension;
        var filePath = Path.Combine(uploadFolder, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        // Return relative path that can be used in ImageUrl
        // Path separator will be handled correctly by ASP.NET Core
        return $"/{folderName}/{fileName}";
    }

    public void DeleteImage(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return;
        }

        // Remove leading slash if present
        var relativePath = imagePath.TrimStart('/');
        var fullPath = Path.Combine(_environment.WebRootPath, relativePath);

        // Normalize paths for cross-platform compatibility (Windows/Linux)
        var normalizedFullPath = Path.GetFullPath(fullPath);
        var normalizedWebRootPath = Path.GetFullPath(_environment.WebRootPath);

        // Only delete if file exists and is within wwwroot
        if (File.Exists(normalizedFullPath) && normalizedFullPath.StartsWith(normalizedWebRootPath, StringComparison.Ordinal))
        {
            try
            {
                File.Delete(normalizedFullPath);
            }
            catch
            {
                // Log error if needed, but don't throw
                // File might be in use or already deleted
            }
        }
    }

    public bool IsValidImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return false;
        }

        // Check file size
        if (file.Length > _maxFileSize)
        {
            return false;
        }

        // Check extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
        {
            return false;
        }

        // Check content type
        var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        if (!allowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            return false;
        }

        return true;
    }

    private string GenerateUniqueFileName()
    {
        // Generate a unique filename using timestamp and random bytes
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var randomBytes = new byte[4];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        var randomString = BitConverter.ToString(randomBytes).Replace("-", "").ToLowerInvariant();
        return $"{timestamp}_{randomString}";
    }
}

