using DentistClinic.Core.Models;
using DentistClinic.Core.ViewModels;
using DentistClinic.CustomeValidation;
using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentistClinic.Controllers
{
    public class MedicalReportsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //1)allowed extension
        public IEnumerable<string> AllowedExtensions { get; set; } = new List<string>() { ".jpg", ".png", ".jpeg" };
        //2)maxsize file
        public int MaxFileSize { get; set; } = 2097152;

        public MedicalReportsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [AjaxOnly]
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public IActionResult Create(int id)
        {
            MedicalReportViewModel viewModal = new MedicalReportViewModel();
            viewModal.PatientId = id;

            return PartialView("_Form", viewModal);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create(MedicalReportViewModel model)
        {

            int result = -1;

            if (ModelState.IsValid)
            {
                if(model.StartDate.HasValue && model.EndDate.HasValue)
                {
                    result = DateTime.Compare(DateTime.Parse(model.EndDate?.ToString()!), DateTime.Parse(model.StartDate?.ToString()!));
                }

                if(!model.StartDate.HasValue || !model.EndDate.HasValue || result >= 0) 
                {
                    MedicalHistory medicalHistory = new MedicalHistory()
                    {
                        Name = model.Name,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        Notes = model.Notes,
                        MedicalHistoryImages = new List<MedicalHistoryImage>(),
                        PatientId = model.PatientId
                    };

                    //check report images 
                    if (model.MedicalHistoryImages.Count > 0)
                    {
                        foreach (var medicalImage in model.MedicalHistoryImages)
                        {
                            //check if allowedExtensions
                            var extension = Path.GetExtension(medicalImage.FileName);
                            if (AllowedExtensions.Contains(extension))
                            {
                                if (medicalImage.Length < MaxFileSize)
                                {
                                    MedicalHistoryImage medicalHistoryImage = new MedicalHistoryImage();

                                    using (var datastream = new MemoryStream())
                                    {
                                        await medicalImage.CopyToAsync(datastream);
                                        medicalHistoryImage.Image = datastream.ToArray();

                                        medicalHistory.MedicalHistoryImages.Add(medicalHistoryImage);

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
                    }

                    _unitOfWork.medicalHistoryRepository.Create(medicalHistory);

                    MedicalReportViewModel vmodel = new MedicalReportViewModel()
                    {
                        Id = medicalHistory.Id,
                        PatientId = medicalHistory.PatientId,
                        Name = medicalHistory.Name,
                        StartDate = medicalHistory.StartDate,
                        EndDate = medicalHistory.EndDate,
                        Notes = medicalHistory.Notes,
                        Documentations = medicalHistory.MedicalHistoryImages.Select(x => x.Image).ToList()
                    };

                    return PartialView("_MdeicalReport", vmodel);
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
        [Authorize(Roles = "Doctor")]
        public IActionResult Update(int id)
        {
            MedicalHistory model = _unitOfWork.medicalHistoryRepository.GetById(id);
            if(model != null)
            {
                MedicalReportViewModel viewModal = new MedicalReportViewModel();
                viewModal.Id = model.Id;
                viewModal.PatientId = model.PatientId;
                viewModal.Name = model.Name;
                viewModal.StartDate = model.StartDate;
                viewModal.EndDate = model.EndDate;
                viewModal.Notes = model.Notes;
                viewModal.Documentations = model.MedicalHistoryImages!.Select(x => x.Image).ToList();
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
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Update(MedicalReportViewModel model)
        {

            int result = -1;

            if (ModelState.IsValid)
            {
                if (model.StartDate.HasValue && model.EndDate.HasValue)
                {
                    result = DateTime.Compare(DateTime.Parse(model.EndDate?.ToString()!), DateTime.Parse(model.StartDate?.ToString()!));
                }

                if (!model.StartDate.HasValue || !model.EndDate.HasValue || result >= 0)
                {
                    MedicalHistory medicalHistory = _unitOfWork.medicalHistoryRepository.GetById((int)model.Id!);
                    medicalHistory.Name = model.Name;
                    medicalHistory.StartDate = model.StartDate;
                    medicalHistory.EndDate = model.EndDate;
                    medicalHistory.Notes = model.Notes;
                    medicalHistory.PatientId = model.PatientId;

                    List<MedicalHistoryImage> updatedImage = new List<MedicalHistoryImage>();

                    //check report images 
                    if (model.MedicalHistoryImages.Count > 0)
                    {
                        foreach (var medicalImage in model.MedicalHistoryImages)
                        {
                            //check if allowedExtensions
                            var extension = Path.GetExtension(medicalImage.FileName);
                            if (AllowedExtensions.Contains(extension))
                            {
                                if (medicalImage.Length < MaxFileSize)
                                {
                                    MedicalHistoryImage medicalHistoryImage = new MedicalHistoryImage();

                                    using (var datastream = new MemoryStream())
                                    {
                                        await medicalImage.CopyToAsync(datastream);
                                        medicalHistoryImage.Image = datastream.ToArray();

                                        updatedImage.Add(medicalHistoryImage);

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

                        //check if medical history has already images
                        if (medicalHistory.MedicalHistoryImages.Count() > 0)
                        {
                            _unitOfWork.medicalHistoryImageRepository.DeleteAll(medicalHistory.MedicalHistoryImages);
                        }
                    }

                    medicalHistory.MedicalHistoryImages = updatedImage;
                    _unitOfWork.medicalHistoryRepository.Update(medicalHistory);

                    MedicalReportViewModel vmodel = new MedicalReportViewModel()
                    {
                        Id = medicalHistory.Id,
                        PatientId = medicalHistory.PatientId,
                        Name = medicalHistory.Name,
                        StartDate = medicalHistory.StartDate,
                        EndDate = medicalHistory.EndDate,
                        Notes = medicalHistory.Notes,
                        Documentations = medicalHistory.MedicalHistoryImages.Select(x => x.Image).ToList()
                    };

                    return PartialView("_MdeicalReport" , vmodel);
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
        [Authorize(Roles = "Doctor , User")]
        public IActionResult Details(int id)
        {
            MedicalHistory model = _unitOfWork.medicalHistoryRepository.GetById(id);
            if(model != null)
            {
                MedicalReportViewModel viewModal = new MedicalReportViewModel();
                viewModal.Id = model.Id;
                viewModal.PatientId = model.PatientId;
                viewModal.Name = model.Name;
                viewModal.StartDate = model.StartDate;
                viewModal.EndDate = model.EndDate;
                viewModal.Notes = model.Notes;
                viewModal.Documentations = model.MedicalHistoryImages!.Select(x => x.Image).ToList();
                return PartialView("_ReportDetails", viewModal);
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }
            
            
        }


        [AjaxOnly]
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public IActionResult Delete(int id)
        {
            MedicalHistory model = _unitOfWork.medicalHistoryRepository.GetById(id);
            if (model != null)
            {
                _unitOfWork.medicalHistoryRepository.Delete(model);
                return Ok();
            }
            else
            {
                return BadRequest("something is wrong..!!");
            }


        }

    }
}
