using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.Helpers;

namespace AppCitas.Service.Interfaces;

public interface IMessageRepository
{
    void AddMessage(Messages message);
    void DeleteMessage(Messages message);
    Task<Messages> GetMessage(int id);
    Task<PagedList<MessagesDto>> GetMessagesForUser();
    Task<IEnumerable<MessagesDto>> GetMessageThread();
    Task<bool> SaveAllAsync();
}
