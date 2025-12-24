namespace ThreadifyLab.Services;

public interface IEmailService
{
    Task<bool> SendVerificationEmailAsync(string email, string name, string verificationToken);
    Task<bool> SendPasswordResetEmailAsync(string email, string name, string resetToken);
}









