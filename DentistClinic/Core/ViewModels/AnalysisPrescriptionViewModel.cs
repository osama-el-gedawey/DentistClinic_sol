using DentistClinic.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
    public class AnalysisPrescriptionViewModel
    {
        public int? Id { get; set; }
        public string? Comment { get; set; }
        public string? Cause { get; set; }
        [Required]
        public int AnalysisId { get; set; }
        public Analysis? Analysis { get; set; }
        public int PrescriptionId { get; set; }
        public List<AnalysisPrescriptionImage> AnalysisPrescriptionImage { get; set; } = new List<AnalysisPrescriptionImage>();
        public List<IFormFile> Documentations { get; set; } = new List<IFormFile>();

        public List<SelectListItem> AnalysisSelect { get; set; } = new List<SelectListItem>();
    }
}
