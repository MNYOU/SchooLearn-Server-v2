namespace Dal.Entities;

public class Teacher
{
    public long UserId { get; set; }

    public User User { get; set; } // один ко одному

    public long InstitutionId { get; set; }
    
    public Institution Institution { get; set; } // один ко многим
    
    // public ICollection<Subject> Subjects { get; set; }
    
    public ICollection<Group> Groups { get; set; }
}