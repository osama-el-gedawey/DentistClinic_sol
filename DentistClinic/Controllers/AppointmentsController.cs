﻿using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

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
        

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddAutomaticaly(AutomaticAppointmentViewModel appointments)
        {
            if (ModelState.IsValid)
            {

                if (appointments.EndHour > appointments.StartHour)
                {
                    if (appointments.Start.Count() < 1)
                    {
                        return BadRequest("You must select Date to assign");
                    }

                    foreach (var day in appointments.Start)
                    {
						int counter = ((appointments.EndHour.Hour - appointments.StartHour.Hour) * 60 +
	                        (appointments.EndHour.Minute - appointments.StartHour.Minute)) / appointments.Slot;
						if (counter < 1)
						{
							//ModelState.AddModelError("", "Cannot Create appointments with This Values");
							return BadRequest("Cannot Create appointments with This Values");
						}
                        List<Appointment> appointmentsAtDay = _unitOfWork.appointmentRepository.GetAll()
                            .Where(app => app.Start == day).ToList();
                        var appointmentStartHour = appointments.StartHour;

						for (int i = 0; i < counter; i++)
						{
							Appointment model = new Appointment()
							{
                                Start = day,
                                End = day.AddDays(1),
                                StartTime = appointmentStartHour,
                                EndTime = appointmentStartHour.AddMinutes(appointments.Slot)
                            };
                            bool flag = true;
                            foreach (Appointment app in appointmentsAtDay)
                            {
                                if ((model.StartTime >= app.StartTime && model.StartTime < app.EndTime) ||
                                    (model.EndTime > app.StartTime && model.EndTime <= app.EndTime) ||
                                    (model.StartTime < app.StartTime && model.EndTime > app.StartTime))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                _unitOfWork.appointmentRepository.Create(model);
                                appointmentStartHour = appointmentStartHour.AddMinutes(appointments.Slot);
                            }
                            else
                            {
                                appointmentStartHour = appointmentStartHour.AddMinutes(appointments.Slot);
                            }

                        }

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
            List<Appointment> appointmentsAtDay = _unitOfWork.appointmentRepository.GetAll()
                .Where(app => app.Start == appointment.Start).ToList();
            if (ModelState.IsValid)
            {
                if (appointment.EndTime > appointment.StartTime)
                {   
                    foreach(Appointment app in appointmentsAtDay)
                    {
                        if((appointment.StartTime >= app.StartTime &&  appointment.StartTime< app.EndTime)||
                            (appointment.EndTime > app.StartTime && appointment.EndTime<= app.EndTime)||
                            (appointment.StartTime < app.StartTime&&appointment.EndTime> app.StartTime))
                        {
                            return BadRequest("There is another appointement at same time interval");
                        }
                    }
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
                            if (updatedAppointment.StartTime == appointment.StartTime && updatedAppointment.EndTime == appointment.EndTime)
                            {
                                return BadRequest("Select new value to edit..!!");
                            }
                            updatedAppointment.StartTime = appointment.StartTime;
                            updatedAppointment.EndTime = appointment.EndTime;
                            _unitOfWork.appointmentRepository.Update(updatedAppointment);

                            if(updatedAppointment.Patient != null)
                            {
                                //Send Notification To Patient
                                Notification notification = new Notification()
                                {
                                    Date = DateTime.Now,
                                    Title = "Appointment",
                                    Description = $"Your appoinement at '{updatedAppointment.Start}' has been adjustment to be from '{updatedAppointment.StartTime}' to '{updatedAppointment.EndTime}'",
                                    PatientId = (int)updatedAppointment.PatientId!
                                };

                                _unitOfWork.notificationRepository.Create(notification);
                            }


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

                if (deletedAppointment.Patient != null)
                {
                    //Send Notification To Patient
                    Notification notification = new Notification()
                    {
                        Date = DateTime.Now,
                        Title = "Appointment",
                        Description = $"Sorry, your appoinement at '{deletedAppointment.Start}' has been cancelled , please reserve another appoitment",
                        PatientId = (int)deletedAppointment.PatientId!
                    };

                    _unitOfWork.notificationRepository.Create(notification);
                }

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
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            var appointments = _unitOfWork.appointmentRepository.UpComming()
                .Where(x => x.Start==today&&(x.PatientId==null || x.PatientId == patient.Id)).Select(x => new
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
                .Where(x => (DateTime.Compare(DateTime.Parse(x.Start.ToString()) , DateTime.Parse(dateStr.ToString())) == 0) && (x.PatientId==null || x.PatientId == patient.Id)).Select(x => new
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

				if (appointment.Patient == null)
				{
					_unitOfWork.appointmentRepository.ReserveTo(appointment, patient);
					return Ok();
				}
                else
                {
					return BadRequest("sorry, this appointment just been reserved..!!");
				}

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
