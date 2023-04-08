using System.ComponentModel.DataAnnotations;

namespace Dal.Validation;

public class TINAttribute: ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }

        if (value is long TIN)
        {
            var TINStr = TIN.ToString();
            if (TINStr.Length is 10 or 12)
            {
                return true;
            }
        }

        return false;
    }
}