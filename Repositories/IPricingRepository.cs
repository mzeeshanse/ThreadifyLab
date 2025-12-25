using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IPricingRepository
{
    Task<IEnumerable<Pricing>> GetAllActiveAsync();
    Task<IEnumerable<Pricing>> GetAllAsync();
    Task<IEnumerable<Pricing>> GetByCategoryAsync(string category);
    Task<Pricing?> GetByIdAsync(int id);
    Task<int> CreateAsync(Pricing pricing);
    Task<bool> UpdateAsync(Pricing pricing);
    Task<bool> DeleteAsync(int id);
}

