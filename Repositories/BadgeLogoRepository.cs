using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class BadgeLogoRepository : IBadgeLogoRepository
{
    private readonly DatabaseConnection _db;

    public BadgeLogoRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<BadgeLogo>> GetAllActiveAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<BadgeLogo>(
            "SELECT * FROM BadgeLogos WHERE IsActive = 1 ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<IEnumerable<BadgeLogo>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<BadgeLogo>(
            "SELECT * FROM BadgeLogos ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<BadgeLogo?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<BadgeLogo>(
            "SELECT * FROM BadgeLogos WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(BadgeLogo badgeLogo)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO BadgeLogos (Title, Description, ImageUrl, DisplayOrder, IsActive, CreatedAt)
                    VALUES (@Title, @Description, @ImageUrl, @DisplayOrder, @IsActive, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            badgeLogo.Title,
            badgeLogo.Description,
            badgeLogo.ImageUrl,
            badgeLogo.DisplayOrder,
            badgeLogo.IsActive,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateAsync(BadgeLogo badgeLogo)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE BadgeLogos 
                    SET Title = @Title, Description = @Description, ImageUrl = @ImageUrl, 
                        DisplayOrder = @DisplayOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            badgeLogo.Id,
            badgeLogo.Title,
            badgeLogo.Description,
            badgeLogo.ImageUrl,
            badgeLogo.DisplayOrder,
            badgeLogo.IsActive,
            UpdatedAt = DateTime.UtcNow
        });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM BadgeLogos WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}

