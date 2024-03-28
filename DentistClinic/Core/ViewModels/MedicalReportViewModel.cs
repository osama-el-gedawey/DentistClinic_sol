using DentistClinic.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
    public class MedicalReportViewModel
    {
        public int? Id { get; set; }
        [Required]
        [Display(Name = "Report Title")]
        public string Name { get; set; } = null!;
        [Display(Name = "Begining of Disease")]
        public DateOnly? StartDate { get; set; }
        [Display(Name = "Date of Recovered")]
        public DateOnly? EndDate { get; set; }
        public string? Notes { get; set; }
        public  List<IFormFile> MedicalHistoryImages { get; set; } = new List<IFormFile>();
        public List<byte[]> Documentations { get; set; } = new List<byte[]>();
        public int PatientId { get; set; }
    }
}
