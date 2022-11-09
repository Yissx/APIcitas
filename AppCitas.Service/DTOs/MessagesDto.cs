using AppCitas.Service.Entities;

namespace AppCitas.Service.DTOs;

public class MessagesDto
{
    public int Id { get; set; }
    public string SenderUsername { get; set; }
    public int SenderId { get; set; }
    public string SenderPhotoUrl { get; set; }

    public int RecipientId { get; set; }
    public string RecipientUsername { get; set; }
    public string RecipientPhotoUrl { get; set; }

    public string Content { get; set; }
    public DateTime? DateRead { get; set; } //puede tomar valor nulo
    public DateTime MessageSent { get; set; } 
}
