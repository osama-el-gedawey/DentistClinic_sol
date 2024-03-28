using DentistClinic.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.Models
{
	public class ContactMsg
	{
		public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3 or more char")]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = Errors.phoneValidation)]
        public string Phone { get; set; } = null!;
        [Required]
        [MinLength(10, ErrorMessage = "Message must be 10 or more char")]
        public string Message { get; set; } = null!;
		public bool IsConfirmed { get; set; }=false;

	}
}
