// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using DentistClinic.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using DentistClinic.Core.Constants;
using System.Net.Mail;
using Bookify.Web.Core.Const;
using DentistClinic.Services.Interfaces;



namespace DentistClinic.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
		private readonly IUnitOfWork _unitOfWork;
		public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
			IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [Required]
            [StringLength(20, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 3)]
            [Display(Name = "First Name")]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must be letters only")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 3)]
            [Display(Name = "Last Name")]
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must be letters only")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required]
            [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = Errors.phoneValidation)]
            [Display(Name = "Mobile Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = Errors.passwordLengthMSG, MinimumLength = 6)]
            [RegularExpression(@".*[a-z]+.*", ErrorMessage = "Password must has at lease one lowercase char")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = Errors.confirmationPassword)]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Gender")]
            public string Gender { get; set; }


            [Required]
            [Display(Name = "Birth Date")]
            public DateTime Birthdate { get; set; }

            [Required]
            [StringLength(200, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 5)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required]
            [Display(Name = "Occuopation")]
            public string Occuopation { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                
                    if (Input.Birthdate > DateTime.Now)
                    {
                        ModelState.AddModelError(string.Empty, "Birth date cannot be in the future.");
                        return Page();
                    }

                    ApplicationUser applicationUser = new ApplicationUser
                    {
                        UserName = new MailAddress(Input.Email).User,
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        Patient = new Patient 
                        {
                            FirstName = Input.FirstName,
                            LastName = Input.LastName,
                            FullName = Input.FirstName + " " + Input.LastName,
                            Gender = Input.Gender,
                            PhoneNumber = Input.PhoneNumber,
                            BirthDate = Input.Birthdate,
                            Occupation = Input.Occuopation,
                            Address = Input.Address
                        }
                    };
                var result = await _userManager.CreateAsync(applicationUser, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(applicationUser, Helper.Roles.User.ToString());



                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
						var userId = await _userManager.GetUserIdAsync(applicationUser);
						var email = await _userManager.GetEmailAsync(applicationUser);
						var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
						code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
						var callbackUrl = Url.Page(
							"/Account/ConfirmEmail",
							pageHandler: null,
							values: new { area = "Identity", userId = userId, code = code },
							protocol: Request.Scheme);

						var placeholders = new Dictionary<string, string>()
			            {
				            { "header", $"Hey {applicationUser.Patient.FullName}," },
				            { "body", "please verify your email" },
				            { "url", $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
				            { "linkTitle", "Verify Email" }
			            };

						var htmlBody = _unitOfWork.emailBodyBuilder.GetEmailBody(EmailTemplates.Email, placeholders);

						await _unitOfWork.emailSender.SendEmailAsync(email, "Dental Clinic Verification Email", htmlBody);

						return RedirectToPage("ResendEmailConfirmation", new { email = Input.Email});
                    }
                    else
                    {
                        await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
