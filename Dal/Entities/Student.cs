namespace Dal.Entities;

public class Student
{
    public long UserId { get; set; }

    public User User { get; set; }

    public long? InstitutionId { get; set; }

    public Institution? Institution { get; set; } // наверное не стоит давать студенту ссылку на оо
    
    public bool IsConfirmed { get; set; }

    public ICollection<GroupStudent> GroupsStudent { get; set; }
    
    public ICollection<SolvedTask> SolvedTasks { get; set; }

    // public ICollection<SolvedExtendedTask> SolvedExtendedTasks { get; set; }
}