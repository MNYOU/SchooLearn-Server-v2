namespace Dal.Entities;

public class Teacher
{
    public long UserId { get; set; }

    public User User { get; set; } // один ко одному

    public long InstitutionId { get; set; }
    
    public Institution Institution { get; set; } // один ко многим
    
    public List<Subject> Subjects { get; set; }
    
    public List<Group> Groups { get; set; }
}