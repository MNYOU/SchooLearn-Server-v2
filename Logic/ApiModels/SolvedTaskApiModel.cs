namespace Logic.ApiModels;

public class SolvedTaskApiModel
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Subject { get; set; }

    public string Difficulty { get; set; }

    public bool IsExtended { get; set; }
    
    public DateTime CreationDate { get; set; }

    public DateTime ExecutionPeriod { get; set; }

    public string? ReceivedAnswer { get; set; }

    public long Scores { get; set; }
}