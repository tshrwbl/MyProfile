namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public DateTime CreatedTimestamp { get; set; }
        
    public Byte[] UserPassword { get; set; }
    public Byte[] PasswordSalt { get; set; }

}
