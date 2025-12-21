using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly DatabaseConnection _db;

    public ServiceRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Service>> GetAllActiveAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<Service>(
            "SELECT * FROM Services WHERE IsActive = 1 ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<IEnumerable<Service>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<Service>(
            "SELECT * FROM Services ORDER BY DisplayOrder ASC, Id ASC");
    }

    public async Task<Service?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<Service>(
            "SELECT * FROM Services WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(Service service)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO Services (Title, Description, Icon, DisplayOrder, IsActive, CreatedAt)
                    VALUES (@Title, @Description, @Icon, @DisplayOrder, @IsActive, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            service.Title,
            service.Description,
            service.Icon,
            service.DisplayOrder,
            service.IsActive,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateAsync(Service service)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE Services 
                    SET Title = @Title, Description = @Description, Icon = @Icon, 
                        DisplayOrder = @DisplayOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            service.Id,
            service.Title,
            service.Description,
            service.Icon,
            service.DisplayOrder,
            service.IsActive,
            UpdatedAt = DateTime.UtcNow
        });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM Services WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}

