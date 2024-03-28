using DentistClinic.Core.Models;
using DentistClinic.CustomeValidation;

namespace DentistClinic.Core.ViewModels
{
    public class AppointmentViewModel
    {
        public int? Id { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}
