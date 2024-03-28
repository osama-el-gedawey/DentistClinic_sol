using DentistClinic.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
    public class XrayPrescriptionViewModel
    {
        public int? Id { get; set; }
        public string? Comment { get; set; }
        public string? Cause { get; set; }
        [Required]
        public int XrayId { get; set; }
        public Xray? Xray { get; set; }
        public int PrescriptionId { get; set; }
        public List<XrayPrescriptionImage> XrayPrescriptionImages { get; set; } = new List<XrayPrescriptionImage>();
        public List<IFormFile> Documentations { get; set; } = new List<IFormFile>();

        public List<SelectListItem> XraysSelect { get; set; } = new List<SelectListItem>();
    }
}
