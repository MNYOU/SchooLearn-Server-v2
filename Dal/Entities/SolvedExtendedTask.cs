namespace Dal.Entities;

[Obsolete]
public class SolvedExtendedTask
{
    public long StudentId { get; set; }

    public Student Student { get; set; }

    public long TaskId { get; set; }

    public Task Task { get; set; }
    
    public byte[] AnswerAsFile { get; set; }

    public bool IsChecked { get; set; }

    public byte FinalGrade { get; set; }

    // public string FileName { get; set; }

    public string ContentType { get; set; } = "application/pdf";

    public DateTime SolveTime { get; set; }
}