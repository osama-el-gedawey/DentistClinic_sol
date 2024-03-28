using System.ComponentModel.DataAnnotations.Schema;

namespace DentistClinic.Core.Models
{
    public class Tooth
    {
        public int Id {  get; set; }
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(TPlan))]
        public int TPlanId {  get; set; }
        public virtual Tplans TPlan { get; set; } = null!;

    }
}
