using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
    public class PaymentViewModel
    {
        public int? Id { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public string Type { get; set; } = null!;
        [Required]
        public double Value { get; set; }
        public string? Note { get; set; }
        public int? PatientId {  get; set; }

    }
}
