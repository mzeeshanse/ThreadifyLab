using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllActiveAsync();
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service?> GetByIdAsync(int id);
    Task<int> CreateAsync(Service service);
    Task<bool> UpdateAsync(Service service);
    Task<bool> DeleteAsync(int id);
}

