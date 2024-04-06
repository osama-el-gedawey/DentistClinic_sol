// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using DentistClinic.Core.Models;
using Bookify.Web.Core.Const;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;

        public ResendEmailConfirmationModel(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
			_unitOfWork = unitOfWork;

		}

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet(string email)
        {
			Input = new()
			{
				Email = email,
			};

            return Page();
		}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "This email not found.");
                return Page();
            }

			var userId = await _userManager.GetUserIdAsync(user);
			var email = await _userManager.GetEmailAsync(user);
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			var callbackUrl = Url.Page(
				"/Account/ConfirmEmail",
				pageHandler: null,
				values: new { area = "Identity", userId = userId, code = code },
				protocol: Request.Scheme);

			var placeholders = new Dictionary<string, string>()
			{
				{ "header", $"Hey {user.Patient.FullName}," },
				{ "body", "please verify your email" },
				{ "url", $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
				{ "linkTitle", "Verify Email" }
			};

			var htmlBody = _unitOfWork.emailBodyBuilder.GetEmailBody(EmailTemplates.Email, placeholders);

			await _unitOfWork.emailSender.SendEmailAsync(email, "Dental Clinic Verification Email", htmlBody);

			ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");

			return Page();
        }
    }
}
