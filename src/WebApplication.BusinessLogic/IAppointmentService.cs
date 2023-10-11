namespace WebApplication.BusinessLogic;

public interface IAppointmentService
{
    Task Create(CreateAppointmentRequest request);
}