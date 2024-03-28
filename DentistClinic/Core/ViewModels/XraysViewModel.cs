using DentistClinic.Core.Constants;
using DentistClinic.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
	public class XraysViewModel
	{
		public int? Id { get; set; }
		[Required]
        [StringLength(100, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 3)]
        [Display(Name = "X-Rays Name")]
		public string Name { get; set; } = null!;

		[Required]
		[Display(Name = "X-Rays Type")]
		public string Type { get; set; } = null!;
		public bool IsDeleted { get; set; } = false;
	}
}
