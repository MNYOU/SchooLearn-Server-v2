namespace Dal.Entities;

public record Task
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public long SubjectId { get; set; }

    public Subject Subject { get; set; }

    public long DifficultyId { get; set; }
    
    public Difficulty Difficulty { get; set; }
    
    public string? Answer { get; set; }

    public bool IsPublic { get; set; }

    public bool IsExtended { get; set; }

    public DateTime CreationDateTime { get; set; }

    public DateTime Deadline { get; set; }

    public long InstitutionId { get; set; }

    public Institution Institution { get; set; }

    public long TeacherId { get; set; }

    public Teacher Teacher { get; set; }

    public ICollection<SolvedTask> SolvedTasks { get; set; }
    
    // public ICollection<SolvedExtendedTask> SolvedExtendedTasks { get; set; }

    public ICollection<Group> Groups { get; set; }
}