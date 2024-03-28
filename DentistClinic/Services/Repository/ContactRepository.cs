using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
	public class ContactRepository : GenericRepository<ContactMsg>, IContactRepository
	{
		public ContactRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
		}
	}
}
