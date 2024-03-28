namespace DentistClinic.Core.Models
{
    public class XrayPrescription
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public string? Cause { get; set; }
        public int XrayId { get; set; }
        public virtual Xray? Xray { get; set; }
        public int PrescriptionId { get; set; }
        public virtual Prescription? Prescriptions { get; set; }
        public virtual ICollection<XrayPrescriptionImage> XrayPrescriptionImages { get; set; } = new List<XrayPrescriptionImage>();
    }
}
