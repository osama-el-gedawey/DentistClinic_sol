namespace DentistClinic.Core.Models
{
    public class ChiefComplainPatient
    {
        public int Id { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Notes { get; set; }
        public int ChiefComplainId { get; set; }
        public virtual ChiefComplain? ChiefComplain { get; set; }
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }

    }
}
