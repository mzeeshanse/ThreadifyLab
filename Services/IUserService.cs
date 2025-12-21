using ThreadifyLab.Models;

namespace ThreadifyLab.Services;

public interface IUserService
{
    Task<(bool Success, string Message, User? User)> RegisterAsync(User user, string password);
    Task<(bool Success, string Message, User? User)> LoginAsync(string email, string password);
    Task<bool> VerifyEmailAsync(string token);
    Task<string> GenerateVerificationTokenAsync();
    Task<bool> ResendVerificationEmailAsync(string email);
}

