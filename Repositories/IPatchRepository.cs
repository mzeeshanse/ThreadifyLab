using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IPatchRepository
{
    Task<IEnumerable<Patch>> GetAllActiveAsync();
    Task<IEnumerable<Patch>> GetAllAsync();
    Task<Patch?> GetByIdAsync(int id);
    Task<int> CreateAsync(Patch patch);
    Task<bool> UpdateAsync(Patch patch);
    Task<bool> DeleteAsync(int id);
}









