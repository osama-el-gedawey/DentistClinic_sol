using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
	public class MedicalHistoryRepository : GenericRepository<MedicalHistory>, IMedicalHistoryRepository
	{
		public MedicalHistoryRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
		}
	}
}
