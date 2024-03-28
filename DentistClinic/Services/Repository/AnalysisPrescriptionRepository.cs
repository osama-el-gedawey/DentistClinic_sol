using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class AnalysisPrescriptionRepository : GenericRepository<AnalysisPrescription> , IAnalysisPrescriptionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AnalysisPrescriptionRepository(ApplicationDbContext applicationDbContext) :base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

    }
}
