using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IBadgeLogoRepository
{
    Task<IEnumerable<BadgeLogo>> GetAllActiveAsync();
    Task<IEnumerable<BadgeLogo>> GetAllAsync();
    Task<BadgeLogo?> GetByIdAsync(int id);
    Task<int> CreateAsync(BadgeLogo badgeLogo);
    Task<bool> UpdateAsync(BadgeLogo badgeLogo);
    Task<bool> DeleteAsync(int id);
}












