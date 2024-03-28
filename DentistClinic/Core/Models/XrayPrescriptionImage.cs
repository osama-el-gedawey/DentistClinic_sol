namespace DentistClinic.Core.Models
{
    public class XrayPrescriptionImage
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int XrayPrescriptionId { get; set; }
        public virtual XrayPrescription? XrayPrescription { get; set; }
    }
}
