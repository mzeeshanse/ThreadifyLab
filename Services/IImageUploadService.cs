namespace ThreadifyLab.Services;

public interface IImageUploadService
{
    Task<string> UploadImageAsync(IFormFile imageFile, string folderName = "images");
    void DeleteImage(string imagePath);
    bool IsValidImageFile(IFormFile file);
}

