namespace Dal.Entities;

public class Task
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public long SubjectId { get; set; }

    public Subject Subject { get; set; }

    public long DifficultyId { get; set; }
    
    public Difficulty Difficulty { get; set; }
    
    public string Answer { get; set; }

    // public bool AnswerAsFile { get; set; } = false;
    
    public bool IsExtended { get; set; }
    
    public DateTime CreationDateTime { get; set; } = DateTime.Now;

    public DateTime ExecutionPeriod { get; set; }
    
    public ICollection<SolvedTask> SolvedTasks { get; set; }
    
    // public ICollection<SolvedExtendedTask> SolvedExtendedTasks { get; set; }

    public ICollection<Group> Groups { get; set; }
}