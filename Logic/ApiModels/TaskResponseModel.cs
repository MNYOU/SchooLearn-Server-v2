using Dal.Entities;

namespace Logic.ApiModels;

public class TaskResponseModel
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public SubjectApiModel Subject { get; set; }
    
    public Difficulty Difficulty { get; set; }

    public TeacherApiModel Teacher { get; set; }

    public InstitutionApiModel Institution { get; set; }
    
    public string? Answer { get; set; }

    public bool IsPublic { get; set; }

    public bool IsExtendedTask { get; set; }
    
    public DateTime CreationDateTime { get; set; }

    public DateTime Deadline { get; set; }
}