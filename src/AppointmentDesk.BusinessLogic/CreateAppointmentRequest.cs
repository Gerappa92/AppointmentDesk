namespace AppointmentDesk.BusinessLogic;

public record CreateAppointmentRequest(
    int PatientId,
    DateTime AppointmentStart,
    TimeSpan AppointmentInterval);