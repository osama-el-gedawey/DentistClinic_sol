namespace DentistClinic.Core.Models
{
    public class ChiefComplain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<ChiefComplainPatient>? ChiefComplainPatients { get; set; }
    }
}
