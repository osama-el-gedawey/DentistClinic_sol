using DentistClinic.CustomeValidation;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
    public class AutomaticAppointmentViewModel
    {
        [CurrentDateCustoumeValidation(ErrorMessage = "Date must be more than today date")]
        [Required]
        public DateOnly Start { get; set; }

        [CurrentDateCustoumeValidation(ErrorMessage = "Date must be more than today date")]
        [Required]
        public DateOnly End { get; set; }
        [Required]
        public TimeOnly StartHour { get; set; }
        [Required]
        public TimeOnly EndHour { get; set; }
        [Required]
        [Range(10, 120, ErrorMessage = "Duration between 10 min and 120 min")]
        public int Slot { get; set; }
    }
}
