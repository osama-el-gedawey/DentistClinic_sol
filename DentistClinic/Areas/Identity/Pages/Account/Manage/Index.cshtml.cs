// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using DentistClinic.Core.Constants;
using DentistClinic.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DentistClinic.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "User")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //1)allowed extension
        public IEnumerable<string> AllowedExtensions { get; set; } = new List<string>() { ".jpg", ".png", ".jpeg" };
        //2)maxsize file
        public int MaxFileSize { get; set; } = 2097152;
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
            [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = Errors.phoneValidation)]
            [Display(Name = "Mobile Number")]
            public string PhoneNumber { get; set; }

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

            [Display(Name = "Profile Picture")]
            public byte[]? ProfilePicture { get; set; }

            public string? Email { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var firstName =  _userManager.GetUserAsync(User)?.Result?.Patient.FirstName;
            var lastName =  _userManager.GetUserAsync(User)?.Result?.Patient.LastName;
            var gender =  _userManager.GetUserAsync(User)?.Result?.Patient.Gender;
            var birthdate =  _userManager.GetUserAsync(User)?.Result?.Patient.BirthDate;
            var address =  _userManager.GetUserAsync(User)?.Result?.Patient.Address;
            var occupation =  _userManager.GetUserAsync(User)?.Result?.Patient.Occupation;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);



            Input = new InputModel
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Gender = gender,
                Birthdate = (DateTime)birthdate,
                Address = address,
                Occuopation = occupation,
                ProfilePicture = user.Patient.ProfilePicture,
                Email = user.Email
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


            if (ModelState.IsValid)
            {
                try
                {
                    user.Patient.FirstName = Input.FirstName;
                    user.Patient.LastName = Input.LastName;
                    user.Patient.FullName = $"{Input.FirstName} {Input.LastName}";
                    user.Patient.Address = Input.Address;
                    user.Patient.PhoneNumber = Input.PhoneNumber;
                    user.PhoneNumber = Input.PhoneNumber;
                    user.Patient.BirthDate = Input.Birthdate;
                    user.Patient.Gender = Input.Gender;
                    user.Patient.Occupation = Input.Occuopation;

                    //check image
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();

                        //check if allowedExtensions
                        var extension = Path.GetExtension(file.FileName);
                        if (AllowedExtensions.Contains(extension))
                        {
                            if (file.Length < MaxFileSize) //check size
                            {
                                //check size and extension
                                using (var datastream = new MemoryStream())
                                {
                                    await file.CopyToAsync(datastream);
                                    user.Patient.ProfilePicture = datastream.ToArray();
                                }
                            }
                            else
                            {
                                StatusMessage = "Error, image must be less than 2 mb";
                                return RedirectToPage();
                            }

                        }
                        else
                        {
                            
                           StatusMessage = "Error, allowed extensions is jpg , jpeg , png";
                           return RedirectToPage();
                        }


                    }

                    IdentityResult result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {

                        await _signInManager.RefreshSignInAsync(user);
                        StatusMessage = "Your profile has been updated";
                        return RedirectToPage();

                    }
                    else
                    {
                        StatusMessage = "Error, Something is wrong..!!";
                        return RedirectToPage();
                    }
                }
                catch (Exception ex)
                {

                    StatusMessage = "Error, " + ex.Message;
                    return RedirectToPage();
                }


            }
            else
            {
                await LoadAsync(user);
                return Page();
            }
        }
    }
}
