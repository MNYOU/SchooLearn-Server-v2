namespace Dal.Entities;

public class Institution
{
    // коды должны генерироваться сразу или после подтверждения
    // второй вариант оптимальнее
    
    public long Id { get; set; }
    
    public string Name { get; set; }

    public long TIN { get; set; } // length == 10 || 12

    public string WebAddress { get; set; }
    
    public bool IsConfirmed { get; set; }
    
    public long? PrimaryInvitationCode { get; set; }
    
    public long? InvitationCodeForTeachers { get; set; }
    
    public long? AdminId { get; set; }

    public Admin? Admin { get; set; }

    public ICollection<Teacher> Teachers { get; set; }

    public ICollection<Student> Students { get; set; }
}