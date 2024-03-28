
using System.ComponentModel.DataAnnotations.Schema;

namespace DentistClinic.Core.Models
{
    public class TPlans_Teeth
    {
        [ForeignKey(nameof(Tplans))]
        public int TplansId {  get; set; }
        public virtual Tplans Tplans { get; set; } = null!;

        [ForeignKey(nameof(Tooth))]
        public int ToothId { get; set; }
        public virtual Tooth Tooth { get; set; } = null!;
    }
}
