using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IVectorArtRepository
{
    Task<IEnumerable<VectorArt>> GetAllActiveAsync();
    Task<IEnumerable<VectorArt>> GetAllAsync();
    Task<VectorArt?> GetByIdAsync(int id);
    Task<int> CreateAsync(VectorArt vectorArt);
    Task<bool> UpdateAsync(VectorArt vectorArt);
    Task<bool> DeleteAsync(int id);
}












