using Dal.Entities;

namespace Logic.ApiModels;

public class TaskResponseModel
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Subject Subject { get; set; }

    public Difficulty Difficulty { get; set; }
    
    public bool IsExtendedTask { get; set; }
    
    public DateTime CreationDateTime { get; set; }

    public DateTime ExecutionPeriod { get; set; }
}