using ThreadifyLab.Models;

namespace ThreadifyLab.Repositories;

public interface IContactRepository
{
    Task<IEnumerable<ContactMessage>> GetAllAsync();
    Task<ContactMessage?> GetByIdAsync(int id);
    Task<int> CreateAsync(ContactMessage message);
    Task<bool> MarkAsReadAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<int> GetUnreadCountAsync();
}

