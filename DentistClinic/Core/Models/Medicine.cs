namespace DentistClinic.Core.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
		public bool IsDeleted { get; set; } = false;
        public virtual ICollection<MedicinePrescriptione>? MedicinePrescriptiones { get; set; }

    }
}
