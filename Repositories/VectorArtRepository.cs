using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class VectorArtRepository : IVectorArtRepository
{
    private readonly DatabaseConnection _db;

    public VectorArtRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<VectorArt>> GetAllActiveAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<VectorArt>(
            "SELECT * FROM VectorArts WHERE IsActive = 1 ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<IEnumerable<VectorArt>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<VectorArt>(
            "SELECT * FROM VectorArts ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<VectorArt?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<VectorArt>(
            "SELECT * FROM VectorArts WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(VectorArt vectorArt)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO VectorArts (Title, Description, ImageUrl, DisplayOrder, IsActive, CreatedAt)
                    VALUES (@Title, @Description, @ImageUrl, @DisplayOrder, @IsActive, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            vectorArt.Title,
            vectorArt.Description,
            vectorArt.ImageUrl,
            vectorArt.DisplayOrder,
            vectorArt.IsActive,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateAsync(VectorArt vectorArt)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE VectorArts 
                    SET Title = @Title, Description = @Description, ImageUrl = @ImageUrl, 
                        DisplayOrder = @DisplayOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            vectorArt.Id,
            vectorArt.Title,
            vectorArt.Description,
            vectorArt.ImageUrl,
            vectorArt.DisplayOrder,
            vectorArt.IsActive,
            UpdatedAt = DateTime.UtcNow
        });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM VectorArts WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}












