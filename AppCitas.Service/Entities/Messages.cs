namespace AppCitas.Service.Entities;

public class Messages
{
    #region  EF Config

    public int Id { get; set; }
    public string SenderUsername { get; set; }
    public int SenderId { get; set; }
    public AppUser Sender { get; set; }
    public int RecipientId { get; set; }
    public string RecipientUsername { get; set; }
    public AppUser Recipient { get; set; }
    #endregion

    #region Message

    public string Content { get; set; }
    public DateTime? DateRead { get; set; } //puede tomar valor nulo
    public DateTime MessageSent { get; set; } = DateTime.Now;
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }

    #endregion
}
