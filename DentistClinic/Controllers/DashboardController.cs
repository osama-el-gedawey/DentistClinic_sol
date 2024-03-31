using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor , Reception")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public DashboardController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this._userManager = userManager;
            this._unitOfWork = unitOfWork;
        }
        public IActionResult  Index()
        {
            List<int> data = new List<int>();
            List<double> paymentGainsListVar = new List<double>();
            List<double> paymentRemainingListVar = new List<double>();
            for (int i = 1; i <= 12; i++)
            {
               data.Add(_unitOfWork.appointmentRepository.GetAll().Where(x=>x.PatientId!=null&&x.Start.Month==i).Count());
               paymentGainsListVar.Add((_unitOfWork.paymentsRepository.GetAll().Where(x => x.Type == "Pay" && x.Date.Month == i).Sum(x => x.Value))*-1);
               paymentRemainingListVar.Add(_unitOfWork.paymentsRepository.GetAll().Where(x =>x.Date.Month == i).Sum(x => x.Value));

            }
            double payGain= _unitOfWork.paymentsRepository.GetAll().Where(x => x.Type == "Pay").Sum(x=>x.Value);
            double payRemain = _unitOfWork.patientRepository.GetAll().Sum(x => x.CurentBalance);
            int GenderMaleVar = _unitOfWork.patientRepository.GetAll().Where(x => x.Gender == "Male" && x.IsDeleted==false).Count();
            int GenderFemaleVar = _unitOfWork.patientRepository.GetAll().Where(x => x.Gender == "Female" && x.IsDeleted == false).Count();
            int GenderChildVar = _unitOfWork.patientRepository.GetAll().Where(x => x.Gender == "Child" && x.IsDeleted == false).Count();
            
            int OnlineUserCounter = 0;
            if (_userManager.Users.Count() > 0)
            {
                foreach(var item in _userManager.Users)
                {
                    if (item.PatientId != null)
                    {
                        OnlineUserCounter++;
                    }
                }
            }
            int AllPatientsCounter = _unitOfWork.patientRepository.GetAll().Count();
            DashBoardViewModel vmodel = new DashBoardViewModel()
            {
                ReservationCount = data,
                paymentGains = payGain * -1,
                paymentRemaining = payRemain,
                paymentGainsList = paymentGainsListVar,
                paymentRemainingList = paymentRemainingListVar,
                GenderMale = GenderMaleVar,
                GenderFemale = GenderFemaleVar,
                GenderChild = GenderChildVar,
                Online = OnlineUserCounter,
                Offline = (AllPatientsCounter-OnlineUserCounter)
            };
            return View(vmodel);
        }
    }
}
