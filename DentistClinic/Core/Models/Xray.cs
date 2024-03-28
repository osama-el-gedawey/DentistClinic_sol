using DentistClinic.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.Models
{
    public class Xray
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Name must be at least 3 char")]
        public string Name { get; set; } = null!;
		[Required]
		[MinLength(3, ErrorMessage = "Type must be at least 3 char")]
		public string Type { get; set; } = null!;
		public bool IsDeleted { get; set; } = false;
        public virtual ICollection<XrayPrescription>? XrayPrescriptions { get; set; }

    }
}
