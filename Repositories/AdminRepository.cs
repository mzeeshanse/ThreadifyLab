using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly DatabaseConnection _db;

    public AdminRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<AdminUser?> GetByUsernameAsync(string username)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<AdminUser>(
            "SELECT * FROM AdminUsers WHERE Username = @Username", new { Username = username });
    }

    public async Task<bool> UpdateLastLoginAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "UPDATE AdminUsers SET LastLoginAt = @LastLoginAt WHERE Id = @Id",
            new { Id = id, LastLoginAt = DateTime.UtcNow });
        return rowsAffected > 0;
    }
}

