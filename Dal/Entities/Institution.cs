namespace Dal.Entities;

public class Institution
{
    public long Id { get; set; }
    
    public string Name { get; set; }

    public long TIN { get; set; } // length == 10 || 12

    public string WebAddress { get; set; }
    
    public bool IsConfirmed { get; set; }
    
    public string? PrimaryInvitationCode { get; set; }
    
    public string? InvitationCodeForTeachers { get; set; }
    
    // public long? AdminId { get; set; }

    public Admin? Admin { get; set; }

    public ICollection<Teacher> Teachers { get; set; }

    public ICollection<Student> Students { get; set; }
}