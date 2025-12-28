using Dapper;
using ThreadifyLab.Data;
using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseConnection _db;

    public UserRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Id = @Id", new { Id = id });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Email = @Email", new { Email = email });
    }

    public async Task<User?> GetByVerificationTokenAsync(string token)
    {
        using var connection = _db.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE EmailVerificationToken = @Token AND EmailVerificationTokenExpiry > NOW()",
            new { Token = token });
    }

    public async Task<int> CreateAsync(User user)
    {
        using var connection = _db.GetConnection();
        var sql = @"INSERT INTO Users (FirstName, LastName, Email, PasswordHash, Phone, IsEmailVerified, 
                    EmailVerificationToken, EmailVerificationTokenExpiry, CreatedAt, IsActive)
                    VALUES (@FirstName, @LastName, @Email, @PasswordHash, @Phone, @IsEmailVerified,
                    @EmailVerificationToken, @EmailVerificationTokenExpiry, @CreatedAt, @IsActive);
                    SELECT LAST_INSERT_ID();";
        return await connection.QuerySingleAsync<int>(sql, new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.PasswordHash,
            user.Phone,
            IsEmailVerified = false,
            user.EmailVerificationToken,
            user.EmailVerificationTokenExpiry,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        });
    }

    public async Task<bool> UpdateAsync(User user)
    {
        using var connection = _db.GetConnection();
        var sql = @"UPDATE Users 
                    SET FirstName = @FirstName, LastName = @LastName, Email = @Email, 
                        Phone = @Phone, IsEmailVerified = @IsEmailVerified,
                        EmailVerificationToken = @EmailVerificationToken,
                        EmailVerificationTokenExpiry = @EmailVerificationTokenExpiry
                    WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Phone,
            user.IsEmailVerified,
            user.EmailVerificationToken,
            user.EmailVerificationTokenExpiry
        });
        return rowsAffected > 0;
    }

    public async Task<bool> VerifyEmailAsync(int userId)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "UPDATE Users SET IsEmailVerified = 1, EmailVerificationToken = NULL, EmailVerificationTokenExpiry = NULL WHERE Id = @Id",
            new { Id = userId });
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateLastLoginAsync(int userId)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "UPDATE Users SET LastLoginAt = @LastLoginAt WHERE Id = @Id",
            new { Id = userId, LastLoginAt = DateTime.UtcNow });
        return rowsAffected > 0;
    }

    public async Task<bool> UpdatePasswordAsync(int userId, string passwordHash)
    {
        using var connection = _db.GetConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "UPDATE Users SET PasswordHash = @PasswordHash WHERE Id = @Id",
            new { Id = userId, PasswordHash = passwordHash });
        return rowsAffected > 0;
    }
}










