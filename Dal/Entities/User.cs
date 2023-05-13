using Dal.Enums;

namespace Dal.Entities;

public class User
{
    public long Id { get; set; }
    
    public string Nickname { get; set; }
    
    public string Login { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public Role Role { get; set; }

    public long? InstitutionId { get; set; }

    public Institution? Institution { get; set; }
}