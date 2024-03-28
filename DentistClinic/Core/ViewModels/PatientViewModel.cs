using DentistClinic.Core.Constants;
using DentistClinic.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
    public class PatientViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 3)]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must be letters only")]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(20, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must be letters only")]
        public string LastName { get; set; } = null!;
        public string? FullName { get; set; }

        [Required]
        [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = Errors.phoneValidation)]
        [Display(Name = "Mobile Number")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = null!;
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; } = new DateTime(2010, 1, 1);
        [Required]
        [Display(Name = "Occuopation")]
        public string Occupation { get; set; } = null!;
        [Required]
        [StringLength(200, ErrorMessage = Errors.usernameLengthMSG, MinimumLength = 5)]
        [Display(Name = "Address")]
        public string Address { get; set; } = null!;
        public double CurentBalance { get; set; } = 0;
        public double GainPayment { get; set; } = 0;
        public Boolean IsDeleted { get; set; } = false;
        public byte[]? ProfilePicture { get; set; }
        public virtual List<PaymentViewModel> PaymentRecords { get; set; } = new List<PaymentViewModel>();
        public virtual List<AppointmentViewModel> Appointments { get; set; } = new List<AppointmentViewModel>();       
        public virtual ICollection<ChiefComplainPatient> ChiefComplainPatients { get; set; } = new List<ChiefComplainPatient>();
        public virtual List<TreatmentPlansViewModel> Tplans { get; set; } = new List<TreatmentPlansViewModel>();
        public virtual List<MedicalReportViewModel> MedicalHistories { get; set; } = new List<MedicalReportViewModel>();
        public virtual List<PrescriptionViewModel> Prescriptions { get; set; } = new List<PrescriptionViewModel>();
        public MedicalReportViewModel? MedicalReportViewModel { get; set; }
    }
}
