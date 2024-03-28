using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class AnalysisController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalysisController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

		public IActionResult Index()
		{
			List<AnalysisViewModel> vmodel = _unitOfWork.analysisRepository.GetAll().Select(x => new AnalysisViewModel
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
			AnalysisViewModel viewModal = new AnalysisViewModel();

			return PartialView("_Form", viewModal);
		}

		[AjaxOnly]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(AnalysisViewModel model)
		{

			if (ModelState.IsValid)
			{
				Analysis analysis = new Analysis();

				analysis.Name = model.Name;
				analysis.Type = model.Type;

				_unitOfWork.analysisRepository.Create(analysis);

				model.Id = analysis.Id;
				return PartialView("_Analysis", model);
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
			Analysis analysis = _unitOfWork.analysisRepository.GetById(id);

			if (analysis != null)
			{
				AnalysisViewModel viewModal = new AnalysisViewModel
				{
					Id = analysis.Id,
					Name = analysis.Name,
					Type = analysis.Type,
					IsDeleted = analysis.IsDeleted,
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
		public IActionResult Update(AnalysisViewModel model)
		{
			if (ModelState.IsValid)
			{
				Analysis analysis = _unitOfWork.analysisRepository.GetById((int)model.Id!);

				if (analysis != null)
				{

					analysis.Name = model.Name;
					analysis.Type = model.Type;

					_unitOfWork.analysisRepository.Update(analysis);
					model.IsDeleted = analysis.IsDeleted;
					return PartialView("_Analysis", model);
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
			Analysis analysis = _unitOfWork.analysisRepository.GetById(id);

			if (analysis != null)
			{
				analysis.IsDeleted = !analysis.IsDeleted;
				_unitOfWork.analysisRepository.Update(analysis);

				AnalysisViewModel viewModel = new AnalysisViewModel();
				viewModel.Name = analysis.Name;
				viewModel.Type = analysis.Type;
				viewModel.IsDeleted = analysis.IsDeleted;

				return Ok(viewModel);
			}
			else
			{
				return NotFound();
			}
		}
	}
}
