using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class PaymentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }


        [AjaxOnly]
        [HttpGet]
        public IActionResult Create(int id)
        {
            PaymentViewModel viewModal = new PaymentViewModel();
            viewModal.PatientId = id;
            return PartialView("_Form", viewModal);
        }


        [AjaxOnly]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(PaymentViewModel payment)
        {
            //code
            if (ModelState.IsValid)
            {
                Patient patient = _unitOfWork.patientRepository.GetById((int)payment.PatientId!);
                if(patient == null) {
                    return BadRequest("something is wrong..!!");
                }
                if (payment.Type == "Pay"|| payment.Type == "Adjustment -ve") {
                    payment.Value = -payment.Value;
                }

                PaymentRecord model = new PaymentRecord();
                model.Date = payment.Date;
                model.Type = payment.Type;
                model.Value = payment.Value;
                model.Note = payment.Note;
                model.PatientId = patient.Id;

                _unitOfWork.paymentsRepository.Create(model);
                patient.CurentBalance += payment.Value;
                _unitOfWork.patientRepository.Update(patient);
                PaymentViewModel vmodel = new PaymentViewModel()
                {
                    Id = payment.Id,
                    Date = payment.Date,
                    Type = payment.Type,
                    Value = payment.Value,
                    Note = payment.Note,
                    PatientId = payment.PatientId
                };


                return PartialView("_Payment" , vmodel);

            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
        }
    }
}
