namespace Dal.Entities;

public class Student
{
    public long UserId { get; set; }

    public User User { get; set; }

    // public string Nickname
    // {
    //     get => User.Nickname;
    //     set => User.Nickname = value;
    // }

    public long? InstitutionId { get; set; }

    public Institution? Institution { get; set; } // наверное не стоит давать студенту ссылку на оо
    
    public bool IsConfirmed { get; set; }

    public ICollection<Group> Groups { get; set; }
    
    public ICollection<SolvedTask> SolvedTasks { get; set; }

    // public ICollection<SolvedTaskWithDetailedAnswer> SolvedTaskWithDetailedAnswers { get; set; }
}