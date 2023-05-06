using System.ComponentModel.DataAnnotations;

namespace Logic.ApiModels;

public record TaskWithAnswerRequest
{
    [Required]
    public long TaskId { get; set; }

    [Required]
    public string Answer { get; set; }
}