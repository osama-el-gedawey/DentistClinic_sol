using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
	public class MedicalHistoryImageRepository : GenericRepository<MedicalHistoryImage>, IMedicalHistoryImageRepository
	{
        private readonly ApplicationDbContext applicationDbContext;

        public MedicalHistoryImageRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
            this.applicationDbContext = applicationDbContext;
        }

        public int DeleteAll(ICollection<MedicalHistoryImage> medicalHistoryImages)
        {
            applicationDbContext.MedicalHistoryImages.RemoveRange(medicalHistoryImages);
            return applicationDbContext.SaveChanges();
        }
    }
}
