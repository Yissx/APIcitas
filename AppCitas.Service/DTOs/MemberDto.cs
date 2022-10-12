using AppCitas.Service.Entities;

namespace AppCitas.Service.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string PhotoUrl { get; set; }
    
    public DateTime Age { get; set; }
    public string KnownnAs { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LasActive { get; set; } 
    public string Gender { get; set; }
    public string Introduction { get; set; }
    public string LookingFor { get; set; }
    public string Interests { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public ICollection<Photo> Photos { get; set; }
}
