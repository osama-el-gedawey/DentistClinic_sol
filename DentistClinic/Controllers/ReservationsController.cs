using DentistClinic.Core.ViewModels;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
	[Authorize(Roles = "Doctor , Reception")]
	public class ReservationsController : Controller
	{
        private readonly IUnitOfWork _unitOfWork;

        public ReservationsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

		public IActionResult Index()
		{
            List<AppointmentViewModel> vmodel = _unitOfWork.appointmentRepository.GetAll().Where(x => x.Patient != null)
            .Select(x => new AppointmentViewModel
            {
                Id = x.Id,
                Start = x.Start,
                End = x.End,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                PatientId = (int)x.PatientId!,
                Patient = x.Patient!
            }).ToList();

			return View(vmodel);
		}
	}
}
