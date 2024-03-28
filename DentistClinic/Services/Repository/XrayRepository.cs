using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class XrayRepository : GenericRepository<Xray>, IXrayRepository
    {
        public XrayRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {

        }
    }
}
