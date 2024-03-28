using DentistClinic.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "User")]
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Reserve()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Reserve(Appointment appointment)
        {
            return View();
        }
    }
}
