using DentistClinic.Core.Models;

namespace DentistClinic.Services.Interfaces
{
    public interface IPatientRepository:IGenericRepository<Patient>
    {
        public Patient GetByName(string name);
        public Patient GetByPhone(string phone);
    }
}
