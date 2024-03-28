namespace DentistClinic.Core.Models
{
    public class AnalysisPrescription
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public string? Cause { get; set; }
        public int PrescriptionId { get; set; }
        public virtual Prescription? Prescriptions { get; set; }
        public int AnalysisId { get; set; }
        public virtual Analysis? Analysis { get; set; }
        public virtual ICollection<AnalysisPrescriptionImage> AnalysisPrescriptionImages { get; set; } = new List<AnalysisPrescriptionImage>();
    }
}
