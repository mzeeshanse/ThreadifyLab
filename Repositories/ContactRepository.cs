using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly DatabaseConnection _db;

    public ContactRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ContactMessage>> GetAllAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QueryAsync<ContactMessage>(
            "SELECT * FROM ContactMessages ORDER BY CreatedAt DESC");
    }

    public async Task<ContactMessage?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<ContactMessage>(
            "SELECT * FROM ContactMessages WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(ContactMessage message)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO ContactMessages (Name, Email, Phone, Subject, Message, IsRead, CreatedAt)
                    VALUES (@Name, @Email, @Phone, @Subject, @Message, @IsRead, @CreatedAt);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            message.Name,
            message.Email,
            message.Phone,
            message.Subject,
            message.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "UPDATE ContactMessages SET IsRead = 1 WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM ContactMessages WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<int> GetUnreadCountAsync()
    {
        using var connection = _db.GetConnection();
        return await connection.QuerySingleAsync<int>(
            "SELECT COUNT(*) FROM ContactMessages WHERE IsRead = 0");
    }
}

