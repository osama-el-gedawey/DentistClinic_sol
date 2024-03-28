using DentistClinic.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
    public class MedicinePrescriptionViewModel
    {
        public int? Id { get; set; }
        [Required]
        [Range(0.25 , int.MaxValue , ErrorMessage = "Please enter a value bigger than 0")]
        public double? Dose { get; set; }
        [Required]
        public int? Hours { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Required]
        public int? Days { get; set; }
        [Required]
        [Display(Name = "Medicine Name")]
        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }
        public int PrescriptionId { get; set; }
        public List<SelectListItem> MedicinesSelect { get; set; } = new List<SelectListItem>();
    }
}
