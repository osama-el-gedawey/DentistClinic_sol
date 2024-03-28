namespace DentistClinic.Core.Models
{
    public class MedicinePrescriptione
    {
        public int Id { get; set; }
        public double Dose { get; set; }
        public int? Hours { get; set; }
        public int? Days { get; set; }
        public int MedicineId { get; set; }
        public virtual Medicine? Medicine { get; set; }
        public int PrescriptionId { get; set; }
        public virtual Prescription? Prescription { get; set; }

    }
}
