using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor , Reception")]
    public class AppointmentsController : Controller
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _applicationDbContext;
        public AppointmentsController(IUnitOfWork unitOfWork, ApplicationDbContext applicationDbContext) 
        {
            this._unitOfWork = unitOfWork;
            this._applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult UpComming()
        {
            return View();
        }
        

        [HttpGet]
        [AjaxOnly]
        public IActionResult AddAutomaticaly(AutomaticAppointmentViewModel appointment)
        {
            if (ModelState.IsValid)
            {
                if (appointment.EndHour > appointment.StartHour)
                {
                    int counter = ((appointment.EndHour.Hour - appointment.StartHour.Hour)*60+
                        (appointment.EndHour.Minute-appointment.StartHour.Minute)) / appointment.Slot;
                    if (counter < 1)
                    {
                        //ModelState.AddModelError("", "Cannot Create appointments with This Values");
                        return BadRequest("Cannot Create appointments with This Values");
                    }
                   
                    for (int i = 0; i<counter; i++)
                    {
                        Appointment model = new Appointment()
                        {
                            Start = appointment.Start,
                            End = appointment.End.AddDays(1),
                            StartTime= appointment.StartHour,
                            EndTime= appointment.StartHour.AddMinutes(appointment.Slot)
                        };
                        _unitOfWork.appointmentRepository.Create(model);
                        appointment.StartHour = appointment.StartHour.AddMinutes(appointment.Slot);
                    }
                    return Json(new { Result = "Ok" });
                }
                else
                {
                    return BadRequest("end hour must be more than start hour");
                }
            }
            else
            {
                return BadRequest("something is wrong");
            }
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult CreateAppointment(Appointment appointment)
        {

            if (ModelState.IsValid)
            {
                if (appointment.EndTime > appointment.StartTime)
                {
                    _unitOfWork.appointmentRepository.Create(appointment);
                    var appointmentJson = new
                    {
                        id = appointment.Id,
                        start = appointment.Start.ToString("yyyy-MM-dd"),
                        end = appointment.End.ToString("yyyy-MM-dd"),
                        startTime = appointment.EndTime,
                        endTime = appointment.StartTime,
                        isReserved = appointment.Patient == null ? false : true
                    };

                    return Ok(appointmentJson);
                }
                else
                {
                    return BadRequest("end time must be more than start time");
                }
                
            }
            else
            {
                return BadRequest("something is wrong");
            }
        }
        [HttpGet]
        [AjaxOnly]
        public IActionResult EditAppointment(Appointment appointment)
        {

            Appointment updatedAppointment = _unitOfWork.appointmentRepository.GetById(appointment.Id)!;

            //check if appointment is already in database
            if(updatedAppointment != null)
            {
                if (ModelState.IsValid)
                {  

                       //check if appoinment start time less than end time
                        if (appointment.EndTime > appointment.StartTime)
                        {
                            updatedAppointment.StartTime = appointment.StartTime;
                            updatedAppointment.EndTime = appointment.EndTime;
                            _unitOfWork.appointmentRepository.Update(updatedAppointment);
                            return Ok(appointment);
                        }
                        else
                        {
                            return BadRequest("end time must be more than start time..!!");
                        }

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

        [HttpGet]
        [AjaxOnly]
        public IActionResult DeleteAppointment(int id)
        {

            Appointment deletedAppointment = _unitOfWork.appointmentRepository.GetById(id)!;
            //checi if appointment is already in database
            if (deletedAppointment != null)
            {
                _unitOfWork.appointmentRepository.Delete(deletedAppointment);
                return Json(new {Result = "Ok"});
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }

        }

        [HttpGet]
        public IActionResult GetAllAppointments()
        {

            var events = _unitOfWork.appointmentRepository.GetAll().Select(x => new
            {
                id = x.Id,
                title = $"{x.StartTime} to {x.EndTime}",
                start = x.Start.ToString("yyyy-MM-dd"),
                end = x.End.ToString("yyyy-MM-dd"),
                appointmentStart = x.StartTime,
                appointmentEnd = x.EndTime,
                isReserved = x.Patient == null ? false : true,
            }).ToList();

            return Json(events);
        }

        [HttpGet]
        [AjaxOnly]
        [AllowAnonymous]
        public IActionResult GetAvaillableAppointments(int patientId)
        {
            var patient = _unitOfWork.patientRepository.GetById(patientId);
            var patientReservedAppointments = _unitOfWork.appointmentRepository.UpComming().Where(x => x.PatientId == patient.Id).Select(x => x.Id).ToList();            
            
            var appointments = _unitOfWork.appointmentRepository.UpComming().Where(x => {

                DateOnly today = DateOnly.FromDateTime(DateTime.Today);
                int result = DateTime.Compare(DateTime.Parse(today.ToString()), DateTime.Parse(x.Start.ToString()));
                return result == 0;
                
            } ).Select(x => new
            {
                id = x.Id,
                title = $"{x.StartTime} to {x.EndTime}",
                start = x.Start.ToString("yyyy-MM-dd"),
                end = x.End.ToString("yyyy-MM-dd"),
                appointmentStart = x.StartTime,
                appointmentEnd = x.EndTime,
                isReserved = x.Patient == null ? false : true,
            }).ToList();


            var data = new
            {
                patient = new { id = patient.Id,
                    fullName = patient.FullName,
                    phoneNumber = patient.PhoneNumber,
                    birthDate = patient.BirthDate,
                    address = patient.Address,
                    gender = patient.Gender,
                    occupation = patient.Occupation,
                    profilePicture = patient.ProfilePicture,
                    upComming = patientReservedAppointments.Count(),
                },
                patientReservedAppointments = patientReservedAppointments,
                appointments
            };
            return Json(data);
        }

        [HttpGet]
        [AjaxOnly]
        [AllowAnonymous]
        public IActionResult GetAvaillableAppointmentsByDate(string dateStr , int patientId)
        {
            var patient = _unitOfWork.patientRepository.GetById(patientId);
            var patientReservedAppointments = _unitOfWork.appointmentRepository.UpComming().Where(x => x.PatientId == patient.Id).Select(x => x.Id).ToList();
            var appointments = _unitOfWork.appointmentRepository.UpComming()
                .Where(x => DateTime.Compare(DateTime.Parse(x.Start.ToString()) , DateTime.Parse(dateStr.ToString())) == 0).Select(x => new
                {
                id = x.Id,
                title = $"{x.StartTime} to {x.EndTime}",
                start = x.Start.ToString("yyyy-MM-dd"),
                end = x.End.ToString("yyyy-MM-dd"),
                appointmentStart = x.StartTime,
                appointmentEnd = x.EndTime,
                isReserved = x.Patient == null ? false : true,
            }).ToList();

            var data = new
            {
                patient = new { 
                    id = patient.Id, 
                    fullname = patient.FullName, 
                    phoneNumber = patient.PhoneNumber, 
                    birthDate = patient.BirthDate,
                    address = patient.Address,
                    gender = patient.Gender,
                    occupation = patient.Occupation,
                    profilePicture = patient.ProfilePicture,
                    upComming = patientReservedAppointments.Count(),
                },
                patientReservedAppointments = patientReservedAppointments,
                appointments
            };
            return Json(data);
        }
        
        [HttpGet]
        [AjaxOnly]
        [AllowAnonymous]
        public IActionResult GetPatientReservation(int patientId)
        {
            var patientReservedAppointments = _unitOfWork.appointmentRepository.UpComming().Where(x => x.PatientId == patientId).ToList();
            var previousReservedAppointments = _unitOfWork.appointmentRepository.PreviousAppointments().Where(x => x.PatientId == patientId).Select(x => x.Id).ToList();

            var appointments = patientReservedAppointments.Select(x => new
            {
                id = x.Id,
                title = $"{x.StartTime} to {x.EndTime}",
                start = x.Start.ToString("yyyy-MM-dd"),
                end = x.End.ToString("yyyy-MM-dd"),
                appointmentStart = x.StartTime,
                appointmentEnd = x.EndTime,
                isReserved = x.Patient == null ? false : true,
            }).ToList();

            var data = new
            {
                patient = new
                {
                    upComming = patientReservedAppointments.Count(),
                    previous = previousReservedAppointments.Count(),
                },
                appointments
            };

            return Json(data);
        }


        [HttpPost]
        [AjaxOnly]
        [AllowAnonymous]
        public IActionResult ReserveAppointment(int appointmentId , int patientId)
        {
            var patient = _unitOfWork.patientRepository.GetById(patientId);
            var appointment = _unitOfWork.appointmentRepository.GetById(appointmentId);
            var appointments = _unitOfWork.appointmentRepository.UpComming().Where(x => x.PatientId == patient.Id);
            if(patient != null && appointment != null)
            {
                foreach (var reservedAppointment in appointments)
                {
                    if(DateTime.Compare(DateTime.Parse(reservedAppointment.Start.ToString()) , DateTime.Parse(appointment.Start.ToString()))  == 0)
                    {
                        return BadRequest("patient has appointment in this day..!!");
                    }
                }
                _unitOfWork.appointmentRepository.ReserveTo(appointment, patient);
                return Ok();
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
        }

        [HttpPost]
        [AjaxOnly]
        [AllowAnonymous]
        public IActionResult CancelAppointment(int appointmentId)
        {
            var appointment = _unitOfWork.appointmentRepository.GetById(appointmentId);
            _unitOfWork.appointmentRepository.Cancel(appointment);
            return Ok();
        }

    }
}
