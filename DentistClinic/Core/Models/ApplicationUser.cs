using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentistClinic.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey(nameof(Patient))]
        public int? PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
