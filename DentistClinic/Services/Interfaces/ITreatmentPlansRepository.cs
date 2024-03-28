using DentistClinic.Core.Models;

namespace DentistClinic.Services.Interfaces
{
	public interface ITreatmentPlansRepository : IGenericRepository<Tplans>
	{
        public int DeleteAll(ICollection<Tooth> teeth);

    }
}
