using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class PatchRepository : IPatchRepository
{
    private readonly DatabaseConnection _db;

    public PatchRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Patch>> GetAllActiveAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<Patch>(
            "SELECT * FROM Patches WHERE IsActive = 1 ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<IEnumerable<Patch>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<Patch>(
            "SELECT * FROM Patches ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<Patch?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<Patch>(
            "SELECT * FROM Patches WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(Patch patch)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO Patches (Title, Description, ImageUrl, DisplayOrder, IsActive, CreatedAt)
                    VALUES (@Title, @Description, @ImageUrl, @DisplayOrder, @IsActive, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            patch.Title,
            patch.Description,
            patch.ImageUrl,
            patch.DisplayOrder,
            patch.IsActive,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateAsync(Patch patch)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE Patches 
                    SET Title = @Title, Description = @Description, ImageUrl = @ImageUrl, 
                        DisplayOrder = @DisplayOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            patch.Id,
            patch.Title,
            patch.Description,
            patch.ImageUrl,
            patch.DisplayOrder,
            patch.IsActive,
            UpdatedAt = DateTime.UtcNow
        });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM Patches WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}



