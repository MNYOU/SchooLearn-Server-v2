namespace Dal.Entities;

public record Difficulty
{
    public long Id { get; set; }

    public string Name { get; set; }

    public byte Scores { get; set; }

    public ICollection<Task> Tasks { get; set; }
}