using System.ComponentModel.DataAnnotations;

namespace Dal.Validation;

public class WebAddressAttribute: ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        if (value is string address)
        {
            if (address.StartsWith("http://") || address.StartsWith("https://"))
                return true;
        }

        return false;
    }
}