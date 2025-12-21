using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly DatabaseConnection _db;

    public PortfolioRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PortfolioItem>> GetAllActiveAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<PortfolioItem>(
            "SELECT * FROM PortfolioItems WHERE IsActive = 1 ORDER BY DisplayOrder ASC, Id DESC");
    }

    public async Task<IEnumerable<PortfolioItem>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<PortfolioItem>(
            "SELECT * FROM PortfolioItems ORDER BY DisplayOrder ASC, Id DESC");
    }

    public async Task<IEnumerable<PortfolioItem>> GetByCategoryAsync(string category)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<PortfolioItem>(
            "SELECT * FROM PortfolioItems WHERE Category = @Category AND IsActive = 1 ORDER BY DisplayOrder ASC, Id DESC",
            new { Category = category });
    }

    public async Task<PortfolioItem?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<PortfolioItem>(
            "SELECT * FROM PortfolioItems WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(PortfolioItem item)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO PortfolioItems (Title, Description, ImageUrl, Category, DisplayOrder, IsActive, CreatedAt)
                    VALUES (@Title, @Description, @ImageUrl, @Category, @DisplayOrder, @IsActive, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            item.Title,
            item.Description,
            item.ImageUrl,
            item.Category,
            item.DisplayOrder,
            item.IsActive,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateAsync(PortfolioItem item)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE PortfolioItems 
                    SET Title = @Title, Description = @Description, ImageUrl = @ImageUrl, 
                        Category = @Category, DisplayOrder = @DisplayOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            item.Id,
            item.Title,
            item.Description,
            item.ImageUrl,
            item.Category,
            item.DisplayOrder,
            item.IsActive,
            UpdatedAt = DateTime.UtcNow
        });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM PortfolioItems WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}

