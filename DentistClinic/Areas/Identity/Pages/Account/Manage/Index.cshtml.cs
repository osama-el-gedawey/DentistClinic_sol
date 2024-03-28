// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DentistClinic.Core.Constants;
using DentistClinic.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DentistClinic.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
            [Required]
            [StringLength(100, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 4)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = Errors.phoneValidation)]
            [Display(Name = "Mobile Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Birth Date")]
            public DateTime Birthdate { get; set; }
            public string? Address { get; set; }
            public string? Gender { get; set; }
            public string? Occupation { get; set; }

            [Display(Name = "Profile Picture")]
            public byte[]? ProfilePicture { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var username =  _userManager.GetUserAsync(User)?.Result?.Patient.FullName;
            var gender =  _userManager.GetUserAsync(User)?.Result?.Patient.Gender;
            var birthdate =  _userManager.GetUserAsync(User)?.Result?.Patient.BirthDate;
            var address =  _userManager.GetUserAsync(User)?.Result?.Patient.Address;
            var occupation =  _userManager.GetUserAsync(User)?.Result?.Patient.Occupation;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

          

            Input = new InputModel
            {
                Username = username,
                PhoneNumber = phoneNumber,
                Gender = gender,
                Birthdate = (DateTime)birthdate,
                Address = address,
                Occupation = occupation,
                ProfilePicture = user.Patient.ProfilePicture
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userName = await _userManager.GetUserNameAsync(user);
            //var email = await _userManager.GetEmailAsync(user);

            if (Input.Username != userName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                if (!setUserNameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set username.";
                    return RedirectToPage();
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }


            if (Input.Birthdate != user.Patient.BirthDate)
            {
                user.Patient.BirthDate = Input.Birthdate;
                IdentityResult result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set birthdate.";
                    return RedirectToPage();
                }
            }

            if (Input.Address != user.Patient.Address)
            {
                user.Patient.Address = Input.Address;
                IdentityResult result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set address.";
                    return RedirectToPage();
                }
            }

            if (Input.Occupation != user.Patient.Occupation)
            {
                user.Patient.Occupation = Input.Occupation;
                IdentityResult result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set occupation.";
                    return RedirectToPage();
                }
            }

            if (Input.Gender != user.Patient.Gender?.ToString())
            {
                user.Patient.Gender = Input.Gender.ToString();
                IdentityResult result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set gender.";
                    return RedirectToPage();
                }
            }

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();

                //check size and extension
                using (var datastream = new MemoryStream())
                {
                    await file.CopyToAsync(datastream);
                    user.Patient.ProfilePicture = datastream.ToArray();
                }

                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
