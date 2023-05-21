namespace Logic.ApiModels;

public record SolvedExtendedTaskPreview
{
    public long StudentId { get; set; }

    public long TaskId { get; set; }

    public SolvedExtendedTaskPreview()
    {
        
    }

    public SolvedExtendedTaskPreview(long studentId, long taskId)
    {
        StudentId = studentId;
        TaskId = taskId;
    }
}