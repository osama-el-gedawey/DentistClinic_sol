using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
	public class TreatmentPlansRepository : GenericRepository<Tplans>, ITreatmentPlansRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TreatmentPlansRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
            this._applicationDbContext = applicationDbContext;
        }

        public int DeleteAll(ICollection<Tooth> teeth)
        {
            _applicationDbContext.Teeth.RemoveRange(teeth);
            return _applicationDbContext.SaveChanges();
        }
    }
}
