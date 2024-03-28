using DentistClinic.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
    public class ContactMsgViewModel
    {
        public int Id { get; set; }
        [Required]
		[StringLength(40, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 3)]
		public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
		public string Email { get; set; } = null!;
        [Required]
        [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = Errors.phoneValidation)]
        public string Phone { get; set; } = null!;
        [Required]
		[StringLength(500, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 3)]
		public string Message { get; set; } = null!;
        public bool IsConfirmed { get; set; } = false;
    }
}
