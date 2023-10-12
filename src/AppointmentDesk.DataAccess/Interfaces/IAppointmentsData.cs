using AppointmentDesk.DataAccess.Entities;

namespace AppointmentDesk.DataAccess.Interfaces;

public interface IAppointmentsData
{
    void CreateAppointment(AppointmentEntity entity);
}