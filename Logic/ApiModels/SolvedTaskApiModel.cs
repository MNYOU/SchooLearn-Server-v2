namespace Logic.ApiModels;

public class SolvedTaskApiModel
{
    public long Id { get; set; }
    
    public string Name { get; set; }
 
    public string Description { get; set; }

    public string Difficulty { get; set; }
    
    public string Subject { get; set; }
    
    public string? ReceivedAnswer { get; set; }

    public bool IsExtended { get; set; }

    public bool IsPublic { get; set; }
    
    public DateTime Deadline { get; set; }
    
    public DateTime CreationDateTime { get; set; }
    
    public float Scores { get; set; }
}