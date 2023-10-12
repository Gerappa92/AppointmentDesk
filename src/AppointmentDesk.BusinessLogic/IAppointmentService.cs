namespace AppointmentDesk.BusinessLogic;

public interface IAppointmentService
{
    Task Create(CreateAppointmentRequest request);
}