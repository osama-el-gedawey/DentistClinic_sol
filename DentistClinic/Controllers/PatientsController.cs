using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor , Reception")]
    public class PatientsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PatientsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork , ApplicationDbContext applicationDbContext , SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._unitOfWork = unitOfWork;
            this._applicationDbContext = applicationDbContext;
            this._signInManager = signInManager;
        }

        public IActionResult Index()
        {
            List<PatientViewModel> vmodels = _unitOfWork.patientRepository.GetAll().Where(x => x.IsDeleted)
                .Select(x => new PatientViewModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                    Gender = x.Gender,
                    BirthDate = x.BirthDate,
                    IsDeleted = x.IsDeleted,

                }).ToList();
            return View(vmodels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //create a new patient 
            PatientViewModel vModel = new PatientViewModel();
            return View(vModel);
        }

        [HttpPost]
        public IActionResult Create(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                //create new patient
                Patient patient = new Patient();
                patient.FirstName = model.FirstName;
                patient.LastName = model.LastName;
                patient.FullName = model.FirstName + " " + model.LastName;
                patient.PhoneNumber = model.PhoneNumber;
                patient.Address = model.Address;
                patient.Gender = model.Gender;
                patient.BirthDate = model.BirthDate;
                patient.Occupation = model.Occupation;

                _unitOfWork.patientRepository.Create(patient);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
           
          
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if(_signInManager.IsSignedIn(User))
            {
                var applicationUser = await _userManager.GetUserAsync(User) ?? new ApplicationUser();
                if (await _userManager.IsInRoleAsync(applicationUser, "User") && applicationUser.PatientId != id)
                {
                    return LocalRedirect("~/Identity/Account/AccessDenied");
                }
            }


            //get patient 
            Patient model = _unitOfWork.patientRepository.GetById(id);
            PatientViewModel vmodel = new PatientViewModel()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Gender = model.Gender,
                BirthDate = model.BirthDate,
                Occupation = model.Occupation,
                ProfilePicture = model.ProfilePicture,
                ChiefComplainPatients = model.ChiefComplainPatients,
                Tplans = model.Tplans.Select(x => new TreatmentPlansViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Notes = x.Notes,
                    PatientId = x.PatientId,
                    Teeth = x.Teeth.Select(x => x.Name).ToList(),
                }).ToList(),
                Appointments = model.Appointments.Select(x => new AppointmentViewModel
                {
                    Id = x.Id,
                    Start = x.Start,
                    End = x.End,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    PatientId = (int)x.PatientId!
                }).ToList(),
                PaymentRecords = model.PaymentRecords.Select(x => new PaymentViewModel
                {
                    Id = x.Id,
                    Date = x.Date,
                    Type = x.Type,
                    Value = x.Value,
                    Note = x.Note,
                    PatientId = x.Id

                }).ToList(),
                MedicalHistories = model.MedicalHistories.Select(x => new MedicalReportViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Notes = x.Notes,
                    Documentations = x.MedicalHistoryImages.Select(x => x.Image).ToList(),
                    PatientId = x.PatientId
                }).ToList(),
                Prescriptions = model.Prescriptions.Select(x => new PrescriptionViewModel
                {
                    Id = x.Id,
                    Date = x.Date,
                    Notes = x.Notes,
                    patient = x.Patient!,

                }).ToList(),
                CurentBalance = model.CurentBalance
            };

            List<PaymentRecord> payments = _unitOfWork.paymentsRepository.GetByPatientId(id);

            if(payments.Count() > 0)
            {
                double gainPayments = 0;
                foreach (var payment in payments)
                {
                    if(payment.Type == "Pay")
                    {
                        gainPayments += payment.Value;
                    }
                }

                gainPayments = gainPayments * -1;
                vmodel.GainPayment = gainPayments;
            }


            return View(vmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public IActionResult ToggleStatus(int id)
        {
            Patient patient = _unitOfWork.patientRepository.GetById(id);

            if (patient != null)
            {
                patient.IsDeleted = !patient.IsDeleted;

                _unitOfWork.patientRepository.Update(patient);

                if (patient.IsDeleted)
                {
                    return Ok("Patient Has been Deleted");
                }
                else
                {
                    return Ok("Patient Has been Restored");
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
        public async Task<IActionResult> Delete(int id)
        {
            Patient patient = _unitOfWork.patientRepository.GetById(id);
            ApplicationUser applicationUser = _userManager.Users.FirstOrDefault(x => x.PatientId == patient.Id)!;
            using var transaction = _applicationDbContext.Database.BeginTransaction();

            try
            {
                if (patient != null)
                {
                    _unitOfWork.patientRepository.Delete(patient);
                    if (applicationUser != null)
                    {
                        IdentityResult identityResult = await _userManager.DeleteAsync(applicationUser);
                        if (identityResult.Succeeded)
                        {
                            transaction.Commit();
                            return Ok();
                        }
                        else
                        {
                            return BadRequest("something is wrong..!!");
                        }
                    }
                    else
                    {
                        transaction.Commit();
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest("something is wrong..!!");
                }


            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex.Message);
            }
                        
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetPatients()
        {

            //--------------------------------------
            var start = int.Parse(Request.Form["start"]);
            var length = int.Parse(Request.Form["length"]);
            var orderColumnIndex = Request.Form["order[0][column]"];
            var orderColumnName = Request.Form[$"columns[{orderColumnIndex}][name]"];
            var orderColumnDir = Request.Form["order[0][dir]"];
            var searchValue = Request.Form["search[value]"];
            //---------------------------------------


            var patients = _unitOfWork.patientRepository.GetAll().Where(x => !x.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                patients = patients.Where(x => x.FullName.Contains(searchValue) || x.PhoneNumber.Contains(searchValue));

            
            patients = patients.OrderBy($"{orderColumnName} {orderColumnDir}");

            IEnumerable<PatientViewModel> vmodel = patients.Select(x => new PatientViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Gender = x.Gender,
                BirthDate = x.BirthDate,
                Occupation = x.Occupation,
                Address = x.Address,
                IsDeleted = x.IsDeleted,
                ProfilePicture = x.ProfilePicture
            });

            var data = vmodel.Skip(start).Take(length).ToList();

            var recordsTotal = patients.Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);

        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetDeletedPatients()
        {

            //--------------------------------------
            var start = int.Parse(Request.Form["start"]);
            var length = int.Parse(Request.Form["length"]);
            var orderColumnIndex = Request.Form["order[0][column]"];
            var orderColumnName = Request.Form[$"columns[{orderColumnIndex}][name]"];
            var orderColumnDir = Request.Form["order[0][dir]"];
            var searchValue = Request.Form["search[value]"];
            //---------------------------------------


            var patients = _unitOfWork.patientRepository.GetAll().Where(x => x.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                patients = patients.Where(x => x.FullName.Contains(searchValue) || x.PhoneNumber.Contains(searchValue));


            patients = patients.OrderBy($"{orderColumnName} {orderColumnDir}");

            IEnumerable<PatientViewModel> vmodel = patients.Select(x => new PatientViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Gender = x.Gender,
                BirthDate = x.BirthDate,
                Occupation = x.Occupation,
                Address = x.Address,
                IsDeleted = x.IsDeleted,
                ProfilePicture = x.ProfilePicture
            });

            var data = vmodel.Skip(start).Take(length).ToList();

            var recordsTotal = patients.Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);

        }

    }
}
