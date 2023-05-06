using System.ComponentModel.DataAnnotations;

namespace Logic.ApiModels;

public class TaskApiModel
{
    [Required(ErrorMessage = "Название задание обязятельно")]
    public string Name { get; set; }
 
    [Required(ErrorMessage = "Описание задания обязятельно")]
    public string Description { get; set; }

    [Required] 
    public string Difficulty { get; set; }
    
    [Required] 
    public string Subject { get; set; }
    
    public bool? IsExtended { get; set; }

    public string? Answer { get; set; }
}