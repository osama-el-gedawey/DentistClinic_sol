using DentistClinic.Core.Models;

namespace DentistClinic.Services.Interfaces
{
	public interface IPaymentsRepository:IGenericRepository<PaymentRecord>
	{
		public List<PaymentRecord> GetByPatientId(int id);
	}
}
