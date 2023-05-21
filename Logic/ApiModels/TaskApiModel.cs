using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;

namespace Logic.ApiModels;

public class TaskApiModel
{
    [Range(0, long.MaxValue, ErrorMessage = "Id должен быть больше нуля")]
    public long? Id { get; set; }
    
    [Required(ErrorMessage = "Название задание обязятельно")]
    public string Name { get; set; }
 
    [Required(ErrorMessage = "Описание задания обязятельно")]
    public string Description { get; set; }

    [Required] 
    public string Difficulty { get; set; }
    
    [Required] 
    public string Subject { get; set; }
    
    public string? Answer { get; set; }
    
    public bool IsExtended { get; set; }

    public bool IsPublic { get; set; }
    
    public DateTime Deadline { get; set; }
    
    public DateTime? CreationDateTime { get; set; }
}