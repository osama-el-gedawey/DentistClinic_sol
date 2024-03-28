using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentistClinic.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class PrescriptionController : Controller
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //1)allowed extension
        public IEnumerable<string> AllowedExtensions { get; set; } = new List<string>() { ".jpg", ".png", ".jpeg" };
        //2)maxsize file
        public int MaxFileSize { get; set; } = 2097152;

        public PrescriptionController(IUnitOfWork unitOfWork, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._unitOfWork = unitOfWork;
            this._applicationDbContext = applicationDbContext;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public IActionResult Index()
		{
			return View();
		}


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {

            Prescription prescription = _unitOfWork.prescriptionRepository.GetById(id);

            if (_signInManager.IsSignedIn(User))
            {
                var applicationUser = await _userManager.GetUserAsync(User) ?? new ApplicationUser();
                if (await _userManager.IsInRoleAsync(applicationUser, "User") && applicationUser.PatientId != prescription.PatientId)
                {
                    return LocalRedirect("~/Identity/Account/AccessDenied");
                }
            }

            

            if (prescription != null)
            {

                ////Medicines
                //var medicines = _unitOfWork.medicineRepository.GetAll().ToList();
                //var analysis = _unitOfWork.analysisRepository.GetAll().ToList();
                //var xrays = _unitOfWork.xrayRepository.GetAll().ToList();

                PrescriptionViewModel vmodel = new PrescriptionViewModel()
                {
                    Id = prescription.Id,
                    Date = prescription.Date,
                    Notes = prescription.Notes,
                    patient = prescription.Patient!,
                    //   MedicinesSelect = new SelectList(medicines, "Id", "Name").ToList(),
                    //AnalysisSelect = new SelectList(analysis, "Id", "Name").ToList(),
                    //XraysSelect = new SelectList(xrays, "Id", "Name").ToList(),
                    MedicinePrescriptions = prescription.MedicinePrescriptions!.Select(x => new MedicinePrescriptionViewModel
                    {
                        Id = x.Id,
                        Dose = x.Dose,
                        Days = x.Days,
                        Hours = x.Hours,
                        Medicine = x.Medicine,
                        MedicineId = x.MedicineId

                    }).ToList() ?? new List<MedicinePrescriptionViewModel>(),

                    AnalysisPrescriptions = prescription.AnalysisPrescriptions!.Select(x => new AnalysisPrescriptionViewModel
                    {
                        Id = x.Id,
                        Comment = x.Comment,
                        Cause = x.Cause,
                        PrescriptionId = x.PrescriptionId,
                        AnalysisId = x.AnalysisId,
                        Analysis = x.Analysis,
                        AnalysisPrescriptionImage = x.AnalysisPrescriptionImages?.ToList() ?? new List<AnalysisPrescriptionImage>()

                    }).ToList() ?? new List<AnalysisPrescriptionViewModel>(),

                    XrayPrescriptions = prescription.XrayPrescriptions!.Select(x => new XrayPrescriptionViewModel
                    {
                        Id = x.Id,
                        Comment = x.Comment,
                        Cause = x.Cause,
                        PrescriptionId = x.PrescriptionId,
                        XrayId = x.XrayId,
                        Xray = x.Xray,
                        XrayPrescriptionImages = x.XrayPrescriptionImages?.ToList() ?? new List<XrayPrescriptionImage>()

                    }).ToList() ?? new List<XrayPrescriptionViewModel>(),

                };

                return View(vmodel);

            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
        }


        [HttpPost]
		[ValidateAntiForgeryToken]
		[AjaxOnly]
        public IActionResult Create(int id)
        {

			Patient patient = _unitOfWork.patientRepository.GetById(id);

			if (patient != null)
			{

				var today = DateOnly.FromDateTime(DateTime.Now);
				bool isExisted = patient.Prescriptions.Any(x => x.Date == today);

				if (isExisted) //patient has prescription in current day
				{
					return BadRequest("patient has prescription in current day!!");
                }

                Prescription prescription = new Prescription()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    PatientId = id,
                };


                _unitOfWork.prescriptionRepository.Create(prescription);

				PrescriptionViewModel vmodel = new PrescriptionViewModel()
				{
					Id = prescription.Id,
					Date = prescription.Date,
					Notes = prescription.Notes,
				};


				return PartialView("_Prescription", vmodel);


            }
			else
			{
				return BadRequest("something is wrong..!!");
			}

        }

        [AjaxOnly]
        public IActionResult Delete(int id)
        {

            Prescription prescription = _unitOfWork.prescriptionRepository.GetById(id);

            if (prescription != null)
            {
				_unitOfWork.prescriptionRepository.Delete(prescription);


                return Ok();


            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateComment(PrescriptionViewModel model)
        {
            Prescription prescription = _unitOfWork.prescriptionRepository.GetById((int)model.Id!);
            prescription.Notes = model.Notes;
            _unitOfWork.prescriptionRepository.Update(prescription);
            return RedirectToAction("Details", "Patients", new {id = prescription.PatientId});
        }

        [AjaxOnly]
        [HttpGet]
        public IActionResult CreateMedicine(int id)
        {
            //Medicines
            var medicines = _unitOfWork.medicineRepository.GetAll().Where(x => !x.IsDeleted)
                .Select(x => new { Id = x.Id, Name =$"{x.Name} : {x.Type}"}).ToList();
            MedicinePrescriptionViewModel vmodel = new MedicinePrescriptionViewModel();
            vmodel.PrescriptionId = id;



            vmodel.MedicinesSelect = new SelectList(medicines, "Id", "Name").ToList();

            return PartialView("_MedicineForm" , vmodel);


        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult CreateMedicine([FromForm]MedicinePrescriptione medicinePrescriptione)
        {
            if (ModelState.IsValid)
            {
                var exsistedMedicine = _unitOfWork.medicinePrescriptionRepository.GetAll().FirstOrDefault(x => x.PrescriptionId == medicinePrescriptione.PrescriptionId && x.MedicineId == medicinePrescriptione.MedicineId);
                if (exsistedMedicine == null)
                {
                    _unitOfWork.medicinePrescriptionRepository.Create(medicinePrescriptione);

                    var medicine = _unitOfWork.medicineRepository.GetById(medicinePrescriptione.MedicineId);

                    MedicinePrescriptionViewModel vmodel = new MedicinePrescriptionViewModel()
                    {
                        Id = medicinePrescriptione.Id,
                        Dose = medicinePrescriptione.Dose,
                        Days = medicinePrescriptione.Days,
                        Hours = medicinePrescriptione.Hours,
                        Medicine = medicine,
                        MedicineId = medicine.Id
                    };

                    return PartialView("_MedicinePrescriptionDetails", vmodel);
                }
                else
                {
                    return BadRequest("Prescription has this Medicine..!!");
                }
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
            
        }

        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMedicine(int id)
        {
            MedicinePrescriptione medicinePrescriptione = _unitOfWork.medicinePrescriptionRepository.GetById(id);
            if(medicinePrescriptione != null)
            {
                _unitOfWork.medicinePrescriptionRepository.Delete(medicinePrescriptione);
                return Ok();
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
        }


        [AjaxOnly]
        [HttpGet]
        public IActionResult CreateAnalysis(int id)
        {
            //Analysis
            var analysis = _unitOfWork.analysisRepository.GetAll().Where(x => !x.IsDeleted)
                .Select(x => new { Id = x.Id, Name = $"{x.Name} : {x.Type}" }).ToList();
            AnalysisPrescriptionViewModel vmodel = new AnalysisPrescriptionViewModel();
            vmodel.PrescriptionId = id;



            vmodel.AnalysisSelect = new SelectList(analysis, "Id", "Name").ToList();

            return PartialView("_AnalysisForm", vmodel);


        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult CreateAnalysis([FromForm] AnalysisPrescription analysisPrescription)
        {
            if (ModelState.IsValid)
            {
                var exsistedAnalysis = _unitOfWork.analysisPrescriptionRepository.GetAll().FirstOrDefault(x => x.PrescriptionId == analysisPrescription.PrescriptionId && x.AnalysisId == analysisPrescription.AnalysisId);
                if (exsistedAnalysis == null)
                {
                    _unitOfWork.analysisPrescriptionRepository.Create(analysisPrescription);

                    var analysis = _unitOfWork.analysisRepository.GetById(analysisPrescription.AnalysisId);

                    AnalysisPrescriptionViewModel vmodel = new AnalysisPrescriptionViewModel()
                    {
                        Id = analysisPrescription.Id,
                        Cause = analysisPrescription.Cause,
                        Comment = analysisPrescription.Comment,
                        PrescriptionId = analysisPrescription.PrescriptionId,
                        AnalysisPrescriptionImage = analysisPrescription.AnalysisPrescriptionImages?.ToList() ?? new List<AnalysisPrescriptionImage>(),
                        AnalysisId = analysis.Id,
                        Analysis = analysis
                    };

                    return PartialView("_AnalysisPrescriptionDetails", vmodel);
                }
                else
                {
                    return BadRequest("Prescription has this Analysis..!!");
                }
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }

        }

        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAnalysis(int id)
        {
            AnalysisPrescription analysisPrescription = _unitOfWork.analysisPrescriptionRepository.GetById(id);
            if (analysisPrescription != null)
            {
                _unitOfWork.analysisPrescriptionRepository.Delete(analysisPrescription);
                return Ok();
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
        }


        //documentations
        [AjaxOnly]
        [HttpGet]
        public IActionResult AnalysisDocumentation(int id)
        {
            //Analysis
            var analysisPrescription = _unitOfWork.analysisPrescriptionRepository.GetById(id);
            if(analysisPrescription != null)
            {

                List<AnalysisPrescriptionImage> documentations = analysisPrescription.AnalysisPrescriptionImages?.ToList() ?? new List<AnalysisPrescriptionImage>();

                AnalysisPrescriptionViewModel vmodel = new AnalysisPrescriptionViewModel()
                {
                    Id = analysisPrescription.Id,
                    AnalysisPrescriptionImage = documentations,
                    Comment = analysisPrescription.Comment,
                    AnalysisId = analysisPrescription.AnalysisId,
                    PrescriptionId = analysisPrescription.PrescriptionId
                };

                return PartialView("_AnalysisDocumentationForm", vmodel);

            }
            else
            {
                return BadRequest("something is wrong..!!");
            }




        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnalysisDocumentation(AnalysisPrescriptionViewModel model)
        {
            //Analysis
            if(ModelState.IsValid)
            {

                AnalysisPrescription analysisPrescription = _unitOfWork.analysisPrescriptionRepository.GetById((int)model.Id!);

                if(analysisPrescription != null)
                {
                    List<AnalysisPrescriptionImage> updatedImage = new List<AnalysisPrescriptionImage>();

                    //check report images 
                    if (model.Documentations.Count > 0)
                    {

                        foreach (var document in model.Documentations)
                        {
                            //check if allowedExtensions
                            var extension = Path.GetExtension(document.FileName);
                            if (AllowedExtensions.Contains(extension))
                            {
                                if (document.Length < MaxFileSize)
                                {
                                    AnalysisPrescriptionImage documentAnalysis = new AnalysisPrescriptionImage();

                                    using (var datastream = new MemoryStream())
                                    {
                                        await document.CopyToAsync(datastream);
                                        documentAnalysis.Image = datastream.ToArray();

                                        updatedImage.Add(documentAnalysis);

                                    }

                                }
                                else
                                {
                                    return BadRequest("all images must be less than 2 mb");
                                }

                            }
                            else
                            {
                                return BadRequest("allowed extensions is jpg , jpeg , png");
                            }


                        }


                        //check if analysis has documentation
                        if (analysisPrescription.AnalysisPrescriptionImages.Any())
                        {
                            _applicationDbContext.AnalysisPrescriptionImages.RemoveRange(analysisPrescription.AnalysisPrescriptionImages);
                        }

                        analysisPrescription.AnalysisPrescriptionImages = updatedImage;

                    }

        
                    analysisPrescription.Comment = model.Comment;
                    _unitOfWork.analysisPrescriptionRepository.Update(analysisPrescription);


                    AnalysisPrescriptionViewModel vmodel = new AnalysisPrescriptionViewModel()
                    {

                        Id = analysisPrescription.Id,
                        Comment = analysisPrescription.Comment,
                        Cause = analysisPrescription.Cause,
                        PrescriptionId = analysisPrescription.PrescriptionId,
                        AnalysisId = analysisPrescription.AnalysisId,
                        Analysis = analysisPrescription.Analysis,
                        AnalysisPrescriptionImage = analysisPrescription.AnalysisPrescriptionImages?.ToList() ?? new List<AnalysisPrescriptionImage>()
                    };

                    return PartialView("_AnalysisPrescriptionDetails", vmodel);




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

        [AjaxOnly]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult DetailsAnalysis(int id)
        {

            //Analysis
            var analysisPrescription = _unitOfWork.analysisPrescriptionRepository.GetById(id);
            if (analysisPrescription != null)
            {

                List<AnalysisPrescriptionImage> documentations = analysisPrescription.AnalysisPrescriptionImages?.ToList() ?? new List<AnalysisPrescriptionImage>();

                AnalysisPrescriptionViewModel vmodel = new AnalysisPrescriptionViewModel()
                {
                    Id = analysisPrescription.Id,
                    AnalysisPrescriptionImage = documentations,
                    Comment = analysisPrescription.Comment,
                    AnalysisId = analysisPrescription.AnalysisId,
                    PrescriptionId = analysisPrescription.PrescriptionId,
                    Cause = analysisPrescription.Cause,
                    Analysis = analysisPrescription.Analysis
                };

                return PartialView("_AnalysisDetails", vmodel);

            }
            else
            {
                return BadRequest("something is wrong..!!");
            }

        }

        [AjaxOnly]
        [HttpGet]
        public IActionResult CreateXray(int id)
        {
            //Xrays
            var xrays = _unitOfWork.xrayRepository.GetAll().Where(x => !x.IsDeleted)
                .Select(x => new { Id = x.Id, Name = $"{x.Name} : {x.Type}" }).ToList();
            XrayPrescriptionViewModel vmodel = new XrayPrescriptionViewModel();
            vmodel.PrescriptionId = id;



            vmodel.XraysSelect = new SelectList(xrays, "Id", "Name").ToList();

            return PartialView("_XrayForm", vmodel);


        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult CreateXray([FromForm] XrayPrescription xrayPrescription)
        {
            if (ModelState.IsValid)
            {
                var exsistedXray = _unitOfWork.xraysPrescriptionRepository.GetAll().FirstOrDefault(x => x.PrescriptionId == xrayPrescription.PrescriptionId && x.XrayId == xrayPrescription.XrayId);
                if (exsistedXray == null)
                {
                    _unitOfWork.xraysPrescriptionRepository.Create(xrayPrescription);

                    var xray = _unitOfWork.xrayRepository.GetById(xrayPrescription.XrayId);

                    XrayPrescriptionViewModel vmodel = new XrayPrescriptionViewModel()
                    {
                        Id = xrayPrescription.Id,
                        Cause = xrayPrescription.Cause,
                        Comment = xrayPrescription.Comment,
                        PrescriptionId = xrayPrescription.PrescriptionId,
                        XrayPrescriptionImages = xrayPrescription.XrayPrescriptionImages?.ToList() ?? new List<XrayPrescriptionImage>(),
                        XrayId = xray.Id,
                        Xray = xray
                    };

                    return PartialView("_XrayPrescriptionDetails", vmodel);
                }
                else
                {
                    return BadRequest("Prescription has this Xray..!!");
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
        public IActionResult DetailsXray(int id)
        {

            //Analysis
            var xrayPrescription = _unitOfWork.xraysPrescriptionRepository.GetById(id);
            if (xrayPrescription != null)
            {

                List<XrayPrescriptionImage> documentations = xrayPrescription.XrayPrescriptionImages?.ToList() ?? new List<XrayPrescriptionImage>();

                XrayPrescriptionViewModel vmodel = new XrayPrescriptionViewModel()
                {
                    Id = xrayPrescription.Id,
                    XrayPrescriptionImages = documentations,
                    Comment = xrayPrescription.Comment,
                    XrayId = xrayPrescription.XrayId,
                    PrescriptionId = xrayPrescription.PrescriptionId,
                    Cause = xrayPrescription.Cause,
                    Xray = xrayPrescription.Xray
                };

                return PartialView("_XrayDetails", vmodel);

            }
            else
            {
                return BadRequest("something is wrong..!!");
            }

        }


        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteXray(int id)
        {
            XrayPrescription xrayPrescription = _unitOfWork.xraysPrescriptionRepository.GetById(id);
            if (xrayPrescription != null)
            {
                _unitOfWork.xraysPrescriptionRepository.Delete(xrayPrescription);
                return Ok();
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
        }

        //documentations
        [AjaxOnly]
        [HttpGet]
        public IActionResult XrayDocumentation(int id)
        {
            //Analysis
            var xrayPrescription = _unitOfWork.xraysPrescriptionRepository.GetById(id);
            if (xrayPrescription != null)
            {

                List<XrayPrescriptionImage> documentations = xrayPrescription.XrayPrescriptionImages?.ToList() ?? new List<XrayPrescriptionImage>();

                XrayPrescriptionViewModel vmodel = new XrayPrescriptionViewModel()
                {
                    Id = xrayPrescription.Id,
                    XrayPrescriptionImages = documentations,
                    Comment = xrayPrescription.Comment,
                    XrayId = xrayPrescription.XrayId,
                    PrescriptionId = xrayPrescription.PrescriptionId
                };

                return PartialView("_XrayDocumentationForm", vmodel);

            }
            else
            {
                return BadRequest("something is wrong..!!");
            }




        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XrayDocumentation(XrayPrescriptionViewModel model)
        {
            //Analysis
            if (ModelState.IsValid)
            {

                XrayPrescription xrayPrescription = _unitOfWork.xraysPrescriptionRepository.GetById((int)model.Id!);

                if (xrayPrescription != null)
                {
                    List<XrayPrescriptionImage> updatedImage = new List<XrayPrescriptionImage>();

                    //check report images 
                    if (model.Documentations.Count > 0)
                    {

                        foreach (var document in model.Documentations)
                        {
                            //check if allowedExtensions
                            var extension = Path.GetExtension(document.FileName);
                            if (AllowedExtensions.Contains(extension))
                            {
                                if (document.Length < MaxFileSize)
                                {
                                    XrayPrescriptionImage documentXrays = new XrayPrescriptionImage();

                                    using (var datastream = new MemoryStream())
                                    {
                                        await document.CopyToAsync(datastream);
                                        documentXrays.Image = datastream.ToArray();

                                        updatedImage.Add(documentXrays);

                                    }

                                }
                                else
                                {
                                    return BadRequest("all images must be less than 2 mb");
                                }

                            }
                            else
                            {
                                return BadRequest("allowed extensions is jpg , jpeg , png");
                            }


                        }


                        //check if analysis has documentation
                        if (xrayPrescription.XrayPrescriptionImages.Any())
                        {
                            _applicationDbContext.XrayPrescriptionImages.RemoveRange(xrayPrescription.XrayPrescriptionImages);
                        }

                        xrayPrescription.XrayPrescriptionImages = updatedImage;

                    }


                    xrayPrescription.Comment = model.Comment;
                    _unitOfWork.xraysPrescriptionRepository.Update(xrayPrescription);


                    XrayPrescriptionViewModel vmodel = new XrayPrescriptionViewModel()
                    {

                        Id = xrayPrescription.Id,
                        Comment = xrayPrescription.Comment,
                        Cause = xrayPrescription.Cause,
                        PrescriptionId = xrayPrescription.PrescriptionId,
                        XrayId = xrayPrescription.XrayId,
                        Xray = xrayPrescription.Xray,
                        XrayPrescriptionImages = xrayPrescription.XrayPrescriptionImages?.ToList() ?? new List<XrayPrescriptionImage>()
                    };

                    return PartialView("_XrayPrescriptionDetails", vmodel);




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


        [AjaxOnly]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ReservationPrescription(int id)
        {
            Appointment appointment = _unitOfWork.appointmentRepository.GetById(id);
            if(appointment != null)
            {
                Prescription? prescription = _unitOfWork.prescriptionRepository.GetAll()
                    .FirstOrDefault(x => x.PatientId == appointment.PatientId && x.Date == appointment.Start)!;

                if(prescription != null)
                {
                    return Json(new { redirectToUrl = Url.Action("Details", "Prescription" , new {id = prescription.Id}) });
                }
                else
                {
                    return BadRequest("Not Have Prescription in This Date");
                }
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }    

        }
    }

}
