namespace API.DTOs;
using System.ComponentModel.DataAnnotations;

public class RegisterDTO
{
    [Required]    
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}

