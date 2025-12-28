using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IJacketBackRepository
{
    Task<IEnumerable<JacketBack>> GetAllActiveAsync();
    Task<IEnumerable<JacketBack>> GetAllAsync();
    Task<JacketBack?> GetByIdAsync(int id);
    Task<int> CreateAsync(JacketBack jacketBack);
    Task<bool> UpdateAsync(JacketBack jacketBack);
    Task<bool> DeleteAsync(int id);
}










