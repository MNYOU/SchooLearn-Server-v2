using System.ComponentModel.DataAnnotations;

namespace Dal.Validation;

public class PasswordAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string password)
        {
            return password.Length > 8 && password.Any(char.IsNumber) && password.Any(char.IsUpper);
        }

        return false;
    }
}