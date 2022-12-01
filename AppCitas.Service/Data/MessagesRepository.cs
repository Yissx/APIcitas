using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.Helpers;
using AppCitas.Service.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.Service.Data;

public class MessagesRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public MessagesRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
        return await _context.Messages
            .Include(u => u.Sender)
            .Include(u => u.Recipient)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedList<MessagesDto>> GetMessagesForUser(MessageParams messagesParams)
    {
        var query = _context.Messages
            .OrderByDescending(m => m.MessageSent)
            .AsQueryable();

        query = messagesParams.Container.ToLower() switch
        {
            "inbox" => query.Where(u => u.Recipient.UserName.Equals(messagesParams.Username)
                && u.RecipientDeleted == false),
            "outbox" => query.Where(u => u.Sender.UserName.Equals(messagesParams.Username)
                && u.SenderDeleted == false),
            _ => query.Where(u => u.Recipient.UserName.Equals(messagesParams.Username) 
                && u.RecipientDeleted == false
                && u.DateRead == null)
        };

        var messages = query.ProjectTo<MessagesDto>(_mapper.ConfigurationProvider);
        return await PagedList<MessagesDto>
            .CreateAsync(messages, messagesParams.PageNumber, messagesParams.PageSize);
    }

    public async Task<IEnumerable<MessagesDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        var messages = await _context.Messages
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            .Where(m => m.Recipient.UserName.Equals(currentUsername) && m.RecipientDeleted == false
                    && m.Sender.UserName.Equals(recipientUsername)
                    || m.Recipient.UserName.Equals(recipientUsername)
                    && m.Sender.UserName.Equals(currentUsername) && m.SenderDeleted == false)
            .OrderBy(m => m.MessageSent)
            .ToListAsync();

        var unreadMessages = messages
            .Where(m => m.DateRead == null
                    && m.Recipient.UserName.Equals(currentUsername)).ToList();

        if (unreadMessages.Any())
        {
            foreach(var message in unreadMessages)
            {
                message.DateRead = DateTime.Now;
            }
            await _context.SaveChangesAsync();
        }
        return _mapper.Map<IEnumerable<MessagesDto>>(messages);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0; //SaveChangesAsync() retorna un entero (registros guardados). Si se incumple la condición > 0 es que no guardó nada y retorna un BadRequest
    }

}