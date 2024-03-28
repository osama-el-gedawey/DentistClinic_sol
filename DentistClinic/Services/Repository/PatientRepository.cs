using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class PatientRepository : GenericRepository<Patient> , IPatientRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PatientRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public Patient GetByName(string name)
        {
            return _applicationDbContext.Patients.FirstOrDefault(x => x.FullName == name)!;
        }

        public Patient GetByPhone(string phone)
        {
            return _applicationDbContext.Patients.FirstOrDefault(x => x.PhoneNumber == phone)!;
        }
    }
}
