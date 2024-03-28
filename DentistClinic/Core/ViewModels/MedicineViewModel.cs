using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
	public class MedicineViewModel
	{
		public int? Id { get; set; }
		[MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
		[Display(Name = "Medicine Name")]
		[Required]
		public string Name { get; set; } = null!;
		[Display(Name = "Medicine Type")]
		[Required]
		public string Type { get; set; } = null!;
		public bool IsDeleted { get; set; } = false;
	}
}
