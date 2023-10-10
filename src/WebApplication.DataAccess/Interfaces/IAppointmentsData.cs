using WebApplication.DataAccess.Entities;

namespace WebApplication.DataAccess.Interfaces
{
    public interface IAppointmentsData
    {
        void CreateAppointment(AppointmentEntity entity);
    }
}