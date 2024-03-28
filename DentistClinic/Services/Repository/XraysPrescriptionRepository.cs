using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class XraysPrescriptionRepository : GenericRepository<XrayPrescription> , IXraysPrescriptionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public XraysPrescriptionRepository(ApplicationDbContext applicationDbContext) :base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

    }
}
