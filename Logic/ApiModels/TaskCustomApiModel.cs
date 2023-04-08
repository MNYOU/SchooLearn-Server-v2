using System.ComponentModel.DataAnnotations;

namespace Logic.ApiModels;

public class TaskCustomApiModel
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public string Subject { get; set; }
    
    [Required]
    public string Difficulty { get; set; }
    
    [Required]
    public string Answer { get; set; }
}