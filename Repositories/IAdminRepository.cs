using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IAdminRepository
{
    Task<AdminUser?> GetByUsernameAsync(string username);
    Task<bool> UpdateLastLoginAsync(int id);
}

