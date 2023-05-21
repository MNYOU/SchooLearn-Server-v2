namespace Dal.Entities;

public class Subject
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
    
    public ICollection<Group> Groups { get; set; }

    public ICollection<Task> Tasks { get; set; }
}