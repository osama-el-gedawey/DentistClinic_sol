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
        [Range(5, int.MaxValue, ErrorMessage = "Minimum payment value is 5 LE")]
        public double Value { get; set; } = 50;
        public string? Note { get; set; }
        public int? PatientId {  get; set; }

    }
}
