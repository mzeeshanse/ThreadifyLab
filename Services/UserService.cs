using System.Security.Cryptography;
using System.Text;
using ThreadifyLab.Models;
using ThreadifyLab.Repositories;

namespace ThreadifyLab.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IEmailService emailService, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<(bool Success, string Message, User? User)> RegisterAsync(User user, string password)
    {
        // Check if email already exists
        var existingUser = await _userRepository.GetByEmailAsync(user.Email);
        if (existingUser != null)
        {
            return (false, "An account with this email already exists.", null);
        }

        // Validate password
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            return (false, "Password must be at least 6 characters long.", null);
        }

        // Hash password
        user.PasswordHash = HashPassword(password);

        // Generate verification token
        user.EmailVerificationToken = await GenerateVerificationTokenAsync();
        user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24);

        // Create user
        var userId = await _userRepository.CreateAsync(user);
        user.Id = userId;

        // Send verification email
        var emailSent = await _emailService.SendVerificationEmailAsync(
            user.Email,
            $"{user.FirstName} {user.LastName}",
            user.EmailVerificationToken);

        if (!emailSent)
        {
            _logger.LogWarning($"Failed to send verification email to {user.Email}, but user was created.");
        }

        return (true, "Registration successful! Please check your email to verify your account.", user);
    }

    public async Task<(bool Success, string Message, User? User)> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return (false, "Invalid email or password.", null);
        }

        if (!user.IsActive)
        {
            return (false, "Your account has been deactivated. Please contact support.", null);
        }

        var passwordHash = HashPassword(password);
        if (user.PasswordHash != passwordHash)
        {
            return (false, "Invalid email or password.", null);
        }

        if (!user.IsEmailVerified)
        {
            return (false, "Please verify your email address before logging in. Check your inbox for the verification email.", null);
        }

        await _userRepository.UpdateLastLoginAsync(user.Id);

        return (true, "Login successful.", user);
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        var user = await _userRepository.GetByVerificationTokenAsync(token);
        if (user == null)
        {
            return false;
        }

        return await _userRepository.VerifyEmailAsync(user.Id);
    }

    public async Task<string> GenerateVerificationTokenAsync()
    {
        await Task.CompletedTask;
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }

    public async Task<bool> ResendVerificationEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        if (user.IsEmailVerified)
        {
            return false;
        }

        // Generate new token
        user.EmailVerificationToken = await GenerateVerificationTokenAsync();
        user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24);
        await _userRepository.UpdateAsync(user);

        // Send email
        return await _emailService.SendVerificationEmailAsync(
            user.Email,
            $"{user.FirstName} {user.LastName}",
            user.EmailVerificationToken);
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}



