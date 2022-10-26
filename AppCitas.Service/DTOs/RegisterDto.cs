﻿using System.ComponentModel.DataAnnotations;

namespace AppCitas.Service.DTOs;

public class RegisterDto
{
    [Required] public string Username { get; set; }
    [Required] public string KnownAs { get; set; }
    [Required] public string Gender { get; set; }
    [Required] public DateTime DateOfBirth { get; set; }
    [Required] public string City { get; set; }
    [Required] public string Country { get; set; }   
    
    [Required]  //El campo siguiente es requerido, no nullable o algo así
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; set; }
}
