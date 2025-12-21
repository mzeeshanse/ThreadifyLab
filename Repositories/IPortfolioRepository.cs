using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IPortfolioRepository
{
    Task<IEnumerable<PortfolioItem>> GetAllActiveAsync();
    Task<IEnumerable<PortfolioItem>> GetAllAsync();
    Task<IEnumerable<PortfolioItem>> GetByCategoryAsync(string category);
    Task<PortfolioItem?> GetByIdAsync(int id);
    Task<int> CreateAsync(PortfolioItem item);
    Task<bool> UpdateAsync(PortfolioItem item);
    Task<bool> DeleteAsync(int id);
}

