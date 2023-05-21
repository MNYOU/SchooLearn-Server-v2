using System.ComponentModel.DataAnnotations;
using Dal.Validation;

namespace Logic.ApiModels;

public class InstitutionApiRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [TIN]
    public long TIN { get; set; } // 10 - 12 цифр
    
    [Required]
    [WebAddress]
    public string WebAddress { get; set; }
} 