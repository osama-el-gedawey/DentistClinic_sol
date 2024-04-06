using DentistClinic.Core.Models;
using DentistClinic.CustomeValidation;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DentistClinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this._unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult ConfirmNotification(int id)
        {

            Notification notification = _unitOfWork.notificationRepository.GetById(id);
            notification.IsSeened = true;
            _unitOfWork.notificationRepository.Update(notification);

            var hasNotification = _unitOfWork.notificationRepository.GetAll().Any(x => (x.IsSeened == false) && (x.PatientId == notification.PatientId));

            return Ok(new {hasNotification});

        }
    }
}
