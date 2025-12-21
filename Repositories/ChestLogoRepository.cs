using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class ChestLogoRepository : IChestLogoRepository
{
    private readonly DatabaseConnection _db;

    public ChestLogoRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ChestLogo>> GetAllActiveAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<ChestLogo>(
            "SELECT * FROM ChestLogos WHERE IsActive = 1 ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<IEnumerable<ChestLogo>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<ChestLogo>(
            "SELECT * FROM ChestLogos ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<ChestLogo?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<ChestLogo>(
            "SELECT * FROM ChestLogos WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(ChestLogo chestLogo)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO ChestLogos (Title, Description, ImageUrl, DisplayOrder, IsActive, CreatedAt)
                    VALUES (@Title, @Description, @ImageUrl, @DisplayOrder, @IsActive, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            chestLogo.Title,
            chestLogo.Description,
            chestLogo.ImageUrl,
            chestLogo.DisplayOrder,
            chestLogo.IsActive,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateAsync(ChestLogo chestLogo)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE ChestLogos 
                    SET Title = @Title, Description = @Description, ImageUrl = @ImageUrl, 
                        DisplayOrder = @DisplayOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            chestLogo.Id,
            chestLogo.Title,
            chestLogo.Description,
            chestLogo.ImageUrl,
            chestLogo.DisplayOrder,
            chestLogo.IsActive,
            UpdatedAt = DateTime.UtcNow
        });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM ChestLogos WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}

