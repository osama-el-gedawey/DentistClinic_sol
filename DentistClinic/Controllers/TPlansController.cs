using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class TPlansController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public TPlansController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [AjaxOnly]
        [HttpGet]
        public IActionResult Create(int id)
        {
            TreatmentPlansViewModel viewModal = new TreatmentPlansViewModel();
            viewModal.PatientId = id;



            return PartialView("_Form", viewModal);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TreatmentPlansViewModel model)
        {


            if (ModelState.IsValid)
            {
                int result = DateTime.Compare(DateTime.Parse(model.EndDate.ToString()!), DateTime.Parse(model.StartDate.ToString()!));

                if(result >= 0)
                {
                   
                    Tplans tplan = new Tplans()
                    {
                        Name = model.Name,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        Notes = model.Notes,
                        PatientId = model.PatientId,

                    };

                    _unitOfWork.treatmentPlansRepository.Create(tplan);


                    foreach (var tooth in model.Teeth)
                    {
                        Tooth toothModel = new Tooth() { TPlanId = tplan.Id, Name = tooth };
                        _unitOfWork.toothRepository.Create(toothModel);
                        //tplan.TPlans_Teeth.Add(new TPlans_Teeth { ToothId = tooth });
                    }


                    TreatmentPlansViewModel vmodel = new TreatmentPlansViewModel()
                    {
                        Id = tplan.Id,
                        PatientId = tplan.PatientId,
                        Name = tplan.Name,
                        StartDate = tplan.StartDate,
                        EndDate = tplan.EndDate,
                        Notes = tplan.Notes,
                        Teeth = tplan.Teeth.Select(x => x.Name).ToList()
                    };

                    return PartialView("_TPlan", vmodel);
                }
                else
                {
                    return BadRequest("end date must be more than start date");
                }
                
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
            Tplans model = _unitOfWork.treatmentPlansRepository.GetById(id);
            if(model != null)
            {
                TreatmentPlansViewModel viewModal = new TreatmentPlansViewModel();
                viewModal.Id = model.Id;
                viewModal.PatientId = model.PatientId;
                viewModal.Name = model.Name;
                viewModal.StartDate = model.StartDate;
                viewModal.EndDate = model.EndDate;
                viewModal.Notes = model.Notes;
                viewModal.Teeth = model.Teeth.Select(x => x.Name).ToList();
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
        public IActionResult Update(TreatmentPlansViewModel model)
        {

            if (ModelState.IsValid)
            {
                int result = DateTime.Compare(DateTime.Parse(model.EndDate.ToString()!), DateTime.Parse(model.StartDate.ToString()!));

                if(result >= 0)
                {
                    Tplans tplan = _unitOfWork.treatmentPlansRepository.GetById((int)model.Id!);
                    tplan.Name = model.Name;
                    tplan.StartDate = model.StartDate;
                    tplan.EndDate = model.EndDate;
                    tplan.Notes = model.Notes;
                    tplan.PatientId = model.PatientId;

                    _unitOfWork.treatmentPlansRepository.DeleteAll(tplan.Teeth);

                    foreach (var tooth in model.Teeth)
                    {
                        tplan.Teeth.Add(new Tooth { TPlanId = tplan.Id , Name = tooth });
                    }

                    _unitOfWork.treatmentPlansRepository.Update(tplan);

                    TreatmentPlansViewModel vmodel = new TreatmentPlansViewModel()
                    {
                        Id = tplan.Id,
                        PatientId = tplan.PatientId,
                        Name = tplan.Name,
                        StartDate = tplan.StartDate,
                        EndDate = tplan.EndDate,
                        Notes = tplan.Notes,
                        Teeth = tplan.Teeth.Select(x => x.Name).ToList()
                    };

                    return PartialView("_TPlan", vmodel);

                }
                else
                {
                    return BadRequest("end date must be more than start date..!!");
                }
                

            }
            else
            {
                return BadRequest("something is wrong..!!");
            }

        }

        [AjaxOnly]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            Tplans model = _unitOfWork.treatmentPlansRepository.GetById(id);
            if(model != null)
            {
                TreatmentPlansViewModel viewModal = new TreatmentPlansViewModel();
                viewModal.Id = model.Id;
                viewModal.PatientId = model.PatientId;
                viewModal.Name = model.Name;
                viewModal.StartDate = model.StartDate;
                viewModal.EndDate = model.EndDate;
                viewModal.Notes = model.Notes;
                viewModal.Teeth = model.Teeth.Select(x => x.Name).ToList();
                return PartialView("_TPlanDetails", viewModal);
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
            
            
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Tplans model = _unitOfWork.treatmentPlansRepository.GetById(id);
            if (model != null)
            {
                _unitOfWork.treatmentPlansRepository.Delete(model);
                return Ok();
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }


        }

    }
}
