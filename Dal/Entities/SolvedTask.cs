using Microsoft.EntityFrameworkCore;

namespace Dal.Entities;

public class SolvedTask
{
    public long StudentId { get; set; }

    public Student Student { get; set; }

    public long TaskId { get; set; }

    public Task Task { get; set; }

    public string? Answer { get; set; }

    public FileData FileAnswer { get; set; }
    
    public bool IsChecked { get; set; }

    public float Scores { get; set; }

    public DateTime SolveTime { get; set; }
}