using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DentistClinic.Services.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AppointmentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public int Cancel(Appointment appointment)
        {
            appointment.Patient = null;
            appointment.PatientId = null;
            return Update(appointment);
        }

        public IEnumerable<Appointment> PreviousAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);

            foreach (var appointment in _applicationDbContext.Appointments)
            {
                DateTime dt1 = DateTime.Parse(appointment.Start.ToString()).Date;
                DateTime dt2 = DateTime.Parse(today.ToString()).Date;
                int result = DateTime.Compare(dt1, dt2);

                if (result < 0 || (result == 0 && appointment.StartTime.CompareTo(time) < 0))
                {
                    appointments.Add(appointment);
                }
            }

            return appointments;

        }

        public int ReserveTo(Appointment appointment, Patient patient)
        {
            appointment.Patient = patient;
            return Update(appointment);
        }

        public IEnumerable<Appointment> UpComming()
        {
            List<Appointment> appointments = new List<Appointment>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);

            foreach (var appointment in _applicationDbContext.Appointments)
            {
                DateTime dt1 = DateTime.Parse(appointment.Start.ToString()).Date;
                DateTime dt2 = DateTime.Parse(today.ToString()).Date;
                int result = DateTime.Compare(dt1, dt2);

                if (result > 0 || (result == 0 && appointment.StartTime.CompareTo(time) >= 0))
                {
                    appointments.Add(appointment);
                }
            }

            return appointments;
        }
    }
}
