using DentistClinic.Core.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
	public class ReceptionViewModel
	{
		public string? Id {  get; set; }
		public string? Username { get; set; } = null!;

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		[Remote("EmailExisted" , "Receptions" , AdditionalFields = "Id" , ErrorMessage = "Email is Already Exsisted..!!")]
		public string Email { get; set; } = null!;

		[Required]
		[RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = Errors.phoneValidation)]
		[Display(Name = "Mobile Number")]
		public string PhoneNumber { get; set; } = null!;

		[Required]
		[StringLength(100, ErrorMessage = Errors.passwordLengthMSG, MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
        [RegularExpression(@".*[a-z]+.*", ErrorMessage = "Password must has at lease one lowercase char")]
        public string Password { get; set; } = null!;

	}
}
