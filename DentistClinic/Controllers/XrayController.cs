using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Services.Interfaces;
using DentistClinic.Services.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class XrayController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public XrayController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<XraysViewModel> vmodel = _unitOfWork.xrayRepository.GetAll().Select(x => new XraysViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                IsDeleted = x.IsDeleted
            }).ToList();

			return View(vmodel);
        }

		[AjaxOnly]
		[HttpGet]
		public IActionResult Create()
		{
			XraysViewModel viewModal = new XraysViewModel();

			return PartialView("_Form", viewModal);
		}

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(XraysViewModel model)
        {

            if (ModelState.IsValid)
            {
                Xray xray = new Xray();

                xray.Name = model.Name;
                xray.Type = model.Type;

                _unitOfWork.xrayRepository.Create(xray);

				model.Id = xray.Id;
                return PartialView("_Xray" , model);
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }

        }


		[AjaxOnly]
		[HttpGet]
		public IActionResult Update(int id)
		{
            Xray xray = _unitOfWork.xrayRepository.GetById(id);

            if(xray != null)
            {
				XraysViewModel viewModal = new XraysViewModel
				{
					Id = xray.Id,
					Name = xray.Name,
					Type = xray.Type,
					IsDeleted = xray.IsDeleted,
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
		public IActionResult Update(XraysViewModel model)
		{
			if (ModelState.IsValid)
			{
				Xray xray = _unitOfWork.xrayRepository.GetById((int)model.Id!);

				if (xray != null)
				{

					xray.Name = model.Name;
					xray.Type = model.Type;

					_unitOfWork.xrayRepository.Update(xray);
                    model.IsDeleted = xray.IsDeleted;
					return PartialView("_Xray", model);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public IActionResult ToggleStatus(int id)
        {
            Xray xray = _unitOfWork.xrayRepository.GetById(id);

            if (xray != null)
            {
                xray.IsDeleted = !xray.IsDeleted;
                _unitOfWork.xrayRepository.Update(xray);

                XraysViewModel viewModel = new XraysViewModel();
                viewModel.Name = xray.Name;
                viewModel.Type = xray.Type;
                viewModel.IsDeleted = xray.IsDeleted;

                return Ok(viewModel);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
