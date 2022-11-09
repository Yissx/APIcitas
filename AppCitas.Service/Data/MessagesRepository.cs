using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.Helpers;
using AppCitas.Service.Interfaces;

namespace AppCitas.Service.Data;

public class MessagesRepository : IMessageRepository
{
    private readonly IMessageRepository _messageRepository;
    private readonly DataContext _context;

    public MessagesRepository(DataContext context)
    {
        _context = context;
    }

    public void AddMessage(Messages message)
    {
        _context.Messages.Add(message);
    }

    public void DeleteMessage(Messages message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Messages> GetMessage(int id)
    {
        return await _context.Messages.FindAsync(id);
    }

    public Task<PagedList<MessagesDto>> GetMessagesForUser()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MessagesDto>> GetMessageThread()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0; //SaveChangesAsync() retorna un entero (registros guardados). Si se incumple la condición > 0 es que no guardó nada y retorna un BadRequest
    }
}
