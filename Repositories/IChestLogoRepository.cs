using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IChestLogoRepository
{
    Task<IEnumerable<ChestLogo>> GetAllActiveAsync();
    Task<IEnumerable<ChestLogo>> GetAllAsync();
    Task<ChestLogo?> GetByIdAsync(int id);
    Task<int> CreateAsync(ChestLogo chestLogo);
    Task<bool> UpdateAsync(ChestLogo chestLogo);
    Task<bool> DeleteAsync(int id);
}










