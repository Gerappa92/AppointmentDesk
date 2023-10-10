namespace WebApplication.BusinessLogic;

public record CreateAppointmentRequest(
    int PatientId,
    DateTime AppointmentStart,
    TimeSpan AppointmentInterval);