namespace Dal.Entities;

public class Group
{
    // должны быть уникальными: {teacherID, name}
    // то есть толжны быть уникальными в рамках учителя
    // хотя как произодить данную валидацию?
    public long Id { get; set; }
    
    public string Name { get; set; }

    public string? Description { get; set; }
    
    public long InvitationCode { get; set; }

    public long TeacherId { get; set; }
    
    public Teacher Teacher { get; set; }

    public long SubjectId { get; set; }

    public Subject Subject { get; set; }
    
    public ICollection<Student> Students { get; set; }
}