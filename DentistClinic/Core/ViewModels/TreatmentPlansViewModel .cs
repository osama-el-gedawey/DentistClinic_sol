using DentistClinic.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
    public class TreatmentPlansViewModel
    {
        public int? Id { get; set; }
        [Required]
        [Display(Name = "TPlan Title")]
        public string Name { get; set; } = null!;
        [Display(Name = "Begining of TPlane")]
        public DateOnly StartDate { get; set; }
        [Display(Name = "End of TPlane")]
        public DateOnly EndDate { get; set; }
        public string? Notes { get; set; }
        public int PatientId { get; set; }
        public List<string> Teeth { get; set; } = new List<string>();

        [Required]
        [Display(Name = "Select Teeth")]
        public List<int> SelectedTeeth { get; set; } = new List<int>();
        public List<SelectListItem> AllTeeth { get; set; } = new List<SelectListItem>();
    }
}
