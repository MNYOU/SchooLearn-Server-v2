using System.ComponentModel.DataAnnotations;
using Dal.Validation;

namespace Logic.ApiModels;

public record LoginApiModel
{
    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }
}