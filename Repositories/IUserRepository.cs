using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByVerificationTokenAsync(string token);
    Task<int> CreateAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> VerifyEmailAsync(int userId);
    Task<bool> UpdateLastLoginAsync(int userId);
    Task<bool> UpdatePasswordAsync(int userId, string passwordHash);
}









