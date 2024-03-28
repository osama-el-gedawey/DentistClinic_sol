using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class MedicinePrescriptionRepository : GenericRepository<MedicinePrescriptione> , IMedicinePrescriptionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public MedicinePrescriptionRepository(ApplicationDbContext applicationDbContext) :base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

    }
}
