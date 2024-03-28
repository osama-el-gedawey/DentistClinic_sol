using DentistClinic.Core.Models;

namespace DentistClinic.Services.Interfaces
{
	public interface IMedicalHistoryImageRepository : IGenericRepository<MedicalHistoryImage>
	{
		public int DeleteAll(ICollection<MedicalHistoryImage> medicalHistoryImages);
	}
}
