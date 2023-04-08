using Microsoft.EntityFrameworkCore;

namespace Dal.Entities;

[Keyless]
public class SolvedTask
{
    public long StudentId { get; set; }

    public Student Student { get; set; }

    public long TaskId { get; set; }

    public Task Task { get; set; }

    public string Answer { get; set; }

    public DateTime SolveTime { get; set; }
}