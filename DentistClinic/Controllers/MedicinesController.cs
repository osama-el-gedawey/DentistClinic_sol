using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class MedicinesController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public MedicinesController(IUnitOfWork unitOfWork)
		{
			this._unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			List<MedicineViewModel> vmodel = _unitOfWork.medicineRepository.GetAll().Select(x => new MedicineViewModel
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
			MedicineViewModel viewModal = new MedicineViewModel();

			return PartialView("_Form", viewModal);
		}

		[AjaxOnly]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(MedicineViewModel model)
		{

			if (ModelState.IsValid)
			{
				Medicine medicine = new Medicine();

				medicine.Name = model.Name;
				medicine.Type = model.Type;

				_unitOfWork.medicineRepository.Create(medicine);

				model.Id = medicine.Id;
				return PartialView("_Medicine", model);
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
			Medicine medicine = _unitOfWork.medicineRepository.GetById(id);

			if (medicine != null)
			{
				MedicineViewModel viewModal = new MedicineViewModel
				{
					Id = medicine.Id,
					Name = medicine.Name,
					Type = medicine.Type,
					IsDeleted = medicine.IsDeleted,
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
		public IActionResult Update(MedicineViewModel model)
		{
			if (ModelState.IsValid)
			{
				Medicine medicine = _unitOfWork.medicineRepository.GetById((int)model.Id!);

				if (medicine != null)
				{

					medicine.Name = model.Name;
					medicine.Type = model.Type;

					_unitOfWork.medicineRepository.Update(medicine);
					model.IsDeleted = medicine.IsDeleted;
					return PartialView("_Medicine", model);
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
			Medicine medicine = _unitOfWork.medicineRepository.GetById(id);

			if (medicine != null)
			{
				medicine.IsDeleted = !medicine.IsDeleted;
				_unitOfWork.medicineRepository.Update(medicine);

				MedicineViewModel viewModel = new MedicineViewModel();
				viewModel.Name = medicine.Name;
				viewModel.Type = medicine.Type;
				viewModel.IsDeleted = medicine.IsDeleted;

				return Ok(viewModel);
			}
			else
			{
				return NotFound();
			}
		}
	}
}
