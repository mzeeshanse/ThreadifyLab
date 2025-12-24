using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class CapLogoRepository : ICapLogoRepository
{
    private readonly DatabaseConnection _db;

    public CapLogoRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CapLogo>> GetAllActiveAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<CapLogo>(
            "SELECT * FROM CapLogos WHERE IsActive = 1 ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<IEnumerable<CapLogo>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<CapLogo>(
            "SELECT * FROM CapLogos ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<CapLogo?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<CapLogo>(
            "SELECT * FROM CapLogos WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(CapLogo capLogo)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO CapLogos (Title, Description, ImageUrl, DisplayOrder, IsActive, CreatedAt)
                    VALUES (@Title, @Description, @ImageUrl, @DisplayOrder, @IsActive, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            capLogo.Title,
            capLogo.Description,
            capLogo.ImageUrl,
            capLogo.DisplayOrder,
            capLogo.IsActive,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateAsync(CapLogo capLogo)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE CapLogos 
                    SET Title = @Title, Description = @Description, ImageUrl = @ImageUrl, 
                        DisplayOrder = @DisplayOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            capLogo.Id,
            capLogo.Title,
            capLogo.Description,
            capLogo.ImageUrl,
            capLogo.DisplayOrder,
            capLogo.IsActive,
            UpdatedAt = DateTime.UtcNow
        });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM CapLogos WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}









