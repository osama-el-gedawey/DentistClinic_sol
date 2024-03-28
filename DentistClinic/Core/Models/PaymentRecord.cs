namespace DentistClinic.Core.Models
{
    public class PaymentRecord
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
        public string? Note { get; set; }
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
    }
}
