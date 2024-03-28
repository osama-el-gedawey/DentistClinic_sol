﻿using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.Models
{
    public class Analysis
    {
        public int Id { get; set; }
		[MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [Required]
        public string Name { get; set; } = null!;
        [MinLength(4, ErrorMessage = "Type must be at least 3 characters long.")]
		[Required]
		public string Type { get; set; } = null!;
		public bool IsDeleted { get; set; } = false;
        public virtual ICollection<AnalysisPrescription>? AnalysisPrescriptions { get; set; }
    }
}
