namespace Dal.Entities;

public record GroupStudent
{
    public long StudentId { get; set; }

    public Student Student { get; set; }

    public long GroupId { get; set; }

    public Group Group { get; set; }

    public bool IsApproved { get; set; }
}