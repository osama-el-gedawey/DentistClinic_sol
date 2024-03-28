using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public MedicineRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }


    }
}
