namespace Logic.ApiModels;

public class TaskFullApiModel
{
    public long Id { get; set; }
    
    public string Name { get; set; }
 
    public string Description { get; set; }

    public string Difficulty { get; set; }
    
    public string Subject { get; set; }
    
    public string? Answer { get; set; }
    
    public bool IsExtended { get; set; }

    public bool IsPublic { get; set; }
    
    public DateTime Deadline { get; set; }
    
    public DateTime CreationDateTime { get; set; }
}