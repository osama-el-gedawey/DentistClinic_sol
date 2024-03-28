namespace DentistClinic.Core.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string? Notes { get; set; }
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual ICollection<XrayPrescription>? XrayPrescriptions { get; set; }
        public virtual ICollection<MedicinePrescriptione>? MedicinePrescriptions { get; set; }
        public virtual ICollection<AnalysisPrescription>? AnalysisPrescriptions { get; set; }

    }
}
