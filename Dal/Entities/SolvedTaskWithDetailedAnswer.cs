namespace Dal.Entities;

public class SolvedTaskWithDetailedAnswer
{
    public long StudentId { get; set; }

    public Student Student { get; set; }

    public long TaskId { get; set; }

    public TaskWithDetailedAnswer Task { get; set; }

    public byte[] Answer { get; set; }

    public DateTime SolveTime { get; set; }
}