using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Logic.ApiModels;

public record TaskWithFileAnswerRequest
{
    [Required]
    public long TaskId { get; set; }

    [Required]
    public IFormFile FileAnswer { get; set; }
}