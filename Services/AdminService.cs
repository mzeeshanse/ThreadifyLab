using System.Security.Cryptography;
using System.Text;
using ThreadifyLab.Repositories;

namespace ThreadifyLab.Services;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;

    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var admin = await _adminRepository.GetByUsernameAsync(username);
        if (admin == null) return false;

        var passwordHash = HashPassword(password);
        // --todo
        // if (admin.PasswordHash != passwordHash) return false;

        await _adminRepository.UpdateLastLoginAsync(admin.Id);
        return true;
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

