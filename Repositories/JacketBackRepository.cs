using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class JacketBackRepository : IJacketBackRepository
{
    private readonly DatabaseConnection _db;

    public JacketBackRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<JacketBack>> GetAllActiveAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<JacketBack>(
            "SELECT * FROM JacketBacks WHERE IsActive = 1 ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<IEnumerable<JacketBack>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<JacketBack>(
            "SELECT * FROM JacketBacks ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<JacketBack?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<JacketBack>(
            "SELECT * FROM JacketBacks WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(JacketBack jacketBack)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO JacketBacks (Title, Description, ImageUrl, DisplayOrder, IsActive, CreatedAt)
                    VALUES (@Title, @Description, @ImageUrl, @DisplayOrder, @IsActive, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            jacketBack.Title,
            jacketBack.Description,
            jacketBack.ImageUrl,
            jacketBack.DisplayOrder,
            jacketBack.IsActive,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateAsync(JacketBack jacketBack)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE JacketBacks 
                    SET Title = @Title, Description = @Description, ImageUrl = @ImageUrl, 
                        DisplayOrder = @DisplayOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            jacketBack.Id,
            jacketBack.Title,
            jacketBack.Description,
            jacketBack.ImageUrl,
            jacketBack.DisplayOrder,
            jacketBack.IsActive,
            UpdatedAt = DateTime.UtcNow
        });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM JacketBacks WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}










