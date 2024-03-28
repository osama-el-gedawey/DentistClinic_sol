using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class AnalysisRepository:GenericRepository<Analysis> , IAnalysisRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AnalysisRepository(ApplicationDbContext applicationDbContext) :base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

    }
}
