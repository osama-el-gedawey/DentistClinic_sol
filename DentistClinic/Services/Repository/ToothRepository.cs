using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class ToothRepository : GenericRepository<Tooth>, IToothRepository
    {
        public ToothRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {

        }
    }
}
