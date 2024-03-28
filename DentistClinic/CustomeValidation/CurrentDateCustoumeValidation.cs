using System.ComponentModel.DataAnnotations;

namespace DentistClinic.CustomeValidation
{
    public class CurrentDateCustoumeValidation:ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            if ((DateOnly)value >= today) return true;
            return false;
        }
    }
}
