using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
	public class PaymentsRepository : GenericRepository<PaymentRecord>, IPaymentsRepository
	{
        private readonly ApplicationDbContext _applicationDbContext;

        public PaymentsRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
            this._applicationDbContext = applicationDbContext;
        }

        public List<PaymentRecord> GetByPatientId(int id)
        {
            return _applicationDbContext.PaymentRecords.Where(x => x.PatientId == id).ToList();
        }
    }
}
