namespace Dal.Entities;

public class Difficulty
{
    public long Id { get; set; }

    public string Name { get; set; }

    public byte Worth { get; set; }

    public ICollection<Task> Tasks { get; set; }
}