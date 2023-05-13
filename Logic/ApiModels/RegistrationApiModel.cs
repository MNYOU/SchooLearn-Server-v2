using System.ComponentModel.DataAnnotations;
using Dal.Enums;

namespace Logic.ApiModels;

public record RegistrationApiModel 
{
    [Required(ErrorMessage = "Поле \"Никнейм\" обязательно")]
    [MinLength(5)]
    [MaxLength(15)]
    public string Nickname { get; init; }
    
    [Required]
    [MinLength(10)]
    // LoginAttribute
    public string Login { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    public Role Role { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; init; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Пароли должны совпадать")]
    public string PasswordConfirm { get; init; }
    
    // или без Required и так все работать будет
    public long? InvitationCode { get; init; }
}