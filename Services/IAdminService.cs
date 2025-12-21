namespace ThreadifyLab.Services;

public interface IAdminService
{
    Task<bool> ValidateCredentialsAsync(string username, string password);
}

