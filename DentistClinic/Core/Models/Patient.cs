using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string Occupation { get; set; }
        public string Address { get; set; }
        public double CurentBalance { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public byte[]? ProfilePicture { get; set; }
        public virtual ICollection<PaymentRecord> PaymentRecords { get; set; } = new List<PaymentRecord>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<ChiefComplainPatient> ChiefComplainPatients { get; set; } = new List<ChiefComplainPatient>();
        public virtual ICollection<Tplans> Tplans { get; set; } = new List<Tplans>();
        public virtual ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    }
}
