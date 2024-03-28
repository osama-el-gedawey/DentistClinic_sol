using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class PrescriptionRepository:GenericRepository<Prescription> , IPrescriptionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public PrescriptionRepository(ApplicationDbContext applicationDbContext) :base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

    }
}
