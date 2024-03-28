namespace DentistClinic.Core.Models
{
    public class MedicalHistoryImage
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int MedicalHistoryId { get; set; }
        public virtual MedicalHistory? MedicalHistory { get; set; }

    }
}
