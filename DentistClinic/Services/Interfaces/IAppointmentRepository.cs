using DentistClinic.Core.Models;

namespace DentistClinic.Services.Interfaces
{
    public interface IAppointmentRepository:IGenericRepository<Appointment>
    {
        public IEnumerable<Appointment> PreviousAppointments();
        public IEnumerable<Appointment> UpComming();
        public int ReserveTo(Appointment appointment , Patient patient);
        public int Cancel(Appointment appointment);
    }
}
