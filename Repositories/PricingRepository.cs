using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class PricingRepository : IPricingRepository
{
    private readonly DatabaseConnection _db;

    public PricingRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Pricing>> GetAllActiveAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<Pricing>(
            "SELECT * FROM Pricings WHERE IsActive = 1 ORDER BY Category ASC, DisplayOrder ASC, Id ASC");
    }

    public async Task<IEnumerable<Pricing>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<Pricing>(
            "SELECT * FROM Pricings ORDER BY Category ASC, DisplayOrder ASC, Id ASC");
    }

    public async Task<IEnumerable<Pricing>> GetByCategoryAsync(string category)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<Pricing>(
            "SELECT * FROM Pricings WHERE Category = @Category AND IsActive = 1 ORDER BY DisplayOrder ASC, Id ASC",
            new { Category = category });
    }

    public async Task<Pricing?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<Pricing>(
            "SELECT * FROM Pricings WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(Pricing pricing)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO Pricings (ServiceName, ServiceType, Price, Description, Icon, Category, DisplayOrder, IsActive, CreatedAt)
                    VALUES (@ServiceName, @ServiceType, @Price, @Description, @Icon, @Category, @DisplayOrder, @IsActive, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            pricing.ServiceName,
            pricing.ServiceType,
            pricing.Price,
            pricing.Description,
            pricing.Icon,
            pricing.Category,
            pricing.DisplayOrder,
            pricing.IsActive,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> UpdateAsync(Pricing pricing)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE Pricings 
                    SET ServiceName = @ServiceName, ServiceType = @ServiceType, Price = @Price, 
                        Description = @Description, Icon = @Icon, Category = @Category, 
                        DisplayOrder = @DisplayOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            pricing.Id,
            pricing.ServiceName,
            pricing.ServiceType,
            pricing.Price,
            pricing.Description,
            pricing.Icon,
            pricing.Category,
            pricing.DisplayOrder,
            pricing.IsActive,
            UpdatedAt = DateTime.UtcNow
        });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM Pricings WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}

