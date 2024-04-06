using System.ComponentModel.DataAnnotations.Schema;

namespace DentistClinic.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsSeened { get; set; } = false;
        public DateTime Date { get; set; }
        [ForeignKey(nameof(Patient))]
        public int PatientId {  get; set; }
        public virtual Patient? Patient { get; set; }

    }
}
