using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
    public class ContactsController : Controller
	{
        private readonly IUnitOfWork _unitOfWork;

        public ContactsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Doctor")]
        public IActionResult Index()
		{
            List<ContactMsgViewModel> vmodel = _unitOfWork.contactRepository.GetAll().Select(x => new ContactMsgViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Message = x.Message,
                IsConfirmed = x.IsConfirmed,
            }).ToList();

            return View(vmodel);
        }
        [Authorize(Roles = "Doctor")]
        public IActionResult Details(int id)
		{

			ContactMsg model = _unitOfWork.contactRepository.GetById(id);
			if(model != null)
			{
				model.IsConfirmed = true;
				_unitOfWork.contactRepository.Update(model);

				ContactMsgViewModel contactMsgViewModel = new ContactMsgViewModel()
				{
					Id = model.Id,
					Name = model.Name,
					Email = model.Email,
					Phone = model.Phone,
					Message = model.Message,
					IsConfirmed = model.IsConfirmed,
				};
				return View(contactMsgViewModel);
			}
			else
			{
				return BadRequest("something is wrong..!!");
			}

		}

        [Authorize(Roles = "Doctor")]
        [AjaxOnly]
		[HttpPost]
		public IActionResult Delete(int id)
		{
			ContactMsg model = _unitOfWork.contactRepository.GetById(id);
			if (model != null)
			{
				_unitOfWork.contactRepository.Delete(model);
				return Ok();
			}
			else
			{
				return BadRequest("something is wrong..!!");
			}
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Create(ContactMsg msg)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.contactRepository.Create(msg);
				return RedirectToAction("Index", "Home");
			}
			return View(msg);
		}

	}
}
