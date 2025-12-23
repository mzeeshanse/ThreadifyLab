using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface ICapLogoRepository
{
    Task<IEnumerable<CapLogo>> GetAllActiveAsync();
    Task<IEnumerable<CapLogo>> GetAllAsync();
    Task<CapLogo?> GetByIdAsync(int id);
    Task<int> CreateAsync(CapLogo capLogo);
    Task<bool> UpdateAsync(CapLogo capLogo);
    Task<bool> DeleteAsync(int id);
}



