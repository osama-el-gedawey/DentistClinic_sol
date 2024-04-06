using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class NotificationRepository:GenericRepository<Notification> , INotificationRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public NotificationRepository(ApplicationDbContext applicationDbContext) :base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

    }
}
