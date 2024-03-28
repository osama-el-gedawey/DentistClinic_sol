using DentistClinic.Core.Constants;
using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class ReceptionsController : Controller
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _applicationDbContext;

		public ReceptionsController(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager , ApplicationDbContext applicationDbContext)
        {
			this._unitOfWork = unitOfWork;
			this._userManager = userManager;
			this._applicationDbContext = applicationDbContext;
		}
        public async Task<IActionResult> Index()
        {
			IList<ApplicationUser> applicationUsers = await _userManager.GetUsersInRoleAsync(Helper.Roles.Reception.ToString());

			List<ReceptionViewModel> vmodels = applicationUsers.Select(x => new ReceptionViewModel()
			{
				Id = x.Id,
				Username = x.UserName,
				Email = x.Email!,
				PhoneNumber = x.PhoneNumber!
			}).ToList();


			return View(vmodels);
        }


		[AjaxOnly]
		[HttpGet]
		public IActionResult Create()
		{
			ReceptionViewModel viewModal = new ReceptionViewModel();

			return PartialView("_Form", viewModal);
		}

		[AjaxOnly]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ReceptionViewModel model)
		{

			if (ModelState.IsValid)
			{

				ApplicationUser applicationUser = new ApplicationUser()
				{
					UserName = new MailAddress(model.Email).User,
					Email = model.Email,
					PhoneNumber = model.PhoneNumber,
				};

				var result = await _userManager.CreateAsync(applicationUser, model.Password);

				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(applicationUser, Helper.Roles.Reception.ToString());


					ReceptionViewModel viewModel = new ReceptionViewModel()
					{
						Id = applicationUser.Id,
						Username = applicationUser.UserName,
						Email = applicationUser.Email,
						PhoneNumber = applicationUser.PhoneNumber
					};


					return PartialView("_Reception", viewModel);

				}
				else
				{
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return BadRequest("something is wrong..!!");
				}
			}
			else
			{
                return BadRequest("something is wrong..!!");
			}

		}

		[AjaxOnly]
		[HttpGet]
		public async Task<IActionResult> Update(string id)
		{
			ApplicationUser? applicationUser = await _userManager.FindByIdAsync(id);
			if (applicationUser != null)
			{
				ReceptionViewModel viewModal = new ReceptionViewModel()
				{
					Id = applicationUser.Id,
					Username = applicationUser.UserName,
					Email = applicationUser.Email!,
					PhoneNumber = applicationUser.PhoneNumber!,
				};


				return PartialView("_Form", viewModal);

			}
			else
			{
				return BadRequest("something is wrong..!!");
			}

		}


		[AjaxOnly]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(ReceptionViewModel model)
		{

			if (ModelState.IsValid)
			{

				ApplicationUser? applicationUser = await _userManager.FindByIdAsync(model.Id!);
				if (applicationUser != null)
				{

					using var transaction = _applicationDbContext.Database.BeginTransaction();

					try
					{
						string passwordToken = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
						IdentityResult identityResultPassword = await _userManager.ResetPasswordAsync(applicationUser, passwordToken, model.Password);

						string emailToken = await _userManager.GenerateChangeEmailTokenAsync(applicationUser, model.Email);
						IdentityResult identityResultEmail = await _userManager.ChangeEmailAsync(applicationUser, model.Email, emailToken);

						string phoneToken = await _userManager.GenerateChangePhoneNumberTokenAsync(applicationUser, model.PhoneNumber);
						IdentityResult identityResultPhone = await _userManager.ChangePhoneNumberAsync(applicationUser, model.PhoneNumber, phoneToken);


						ReceptionViewModel? viewModal = new ReceptionViewModel()
						{
							Id = applicationUser.Id,
							Username = applicationUser.UserName,
							Email = applicationUser.Email!,
							PhoneNumber = applicationUser.PhoneNumber!,
						};

						if (identityResultPassword.Succeeded && identityResultEmail.Succeeded && identityResultPhone.Succeeded)
						{
							transaction.Commit();
							return PartialView("_Reception", viewModal);
						}
						else
						{
							transaction.Rollback();
							return BadRequest("something is wrong..!!");
						}

						
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						return BadRequest(ex.Message);
					}

				}
				else
				{
					return BadRequest("something is wrong..!!");
				}
			}
			else
			{
				return BadRequest("something is wrong..!!");
			}

		}

		[AjaxOnly]
		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			ApplicationUser? applicationUser = await _userManager.FindByIdAsync(id);
			if (applicationUser != null)
			{
				IdentityResult identityResult =  await _userManager.DeleteAsync(applicationUser);
				if (identityResult.Succeeded)
				{
					return Ok();

				}
				else
				{
					return BadRequest("something is wrong..!!");
				}
			}
			else
			{
				return BadRequest("something is wrong..!!");
			}


		}

		[AjaxOnly]
        public async Task<IActionResult> EmailExisted(ReceptionViewModel model)
		{

			var isExisted = await _userManager.FindByEmailAsync(model.Email);

			bool isValid = (isExisted == null) || (isExisted.Id == model.Id);

			return Json(isValid);

		}

    }
}
