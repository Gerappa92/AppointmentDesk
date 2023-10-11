using WebApplication.DataAccess.Entities;
using WebApplication.DataAccess.Interfaces;

namespace WebApplication.BusinessLogic;

internal class AppointmentService : IAppointmentService
{
    private readonly IAppointmentsData _appointmentsData;
    private readonly IPatientApi _patientData;

    public AppointmentService(IAppointmentsData appointmentsData, IPatientApi patientData)
    {
        _appointmentsData = appointmentsData ?? throw new ArgumentNullException(nameof(appointmentsData));
        _patientData = patientData ?? throw new ArgumentNullException(nameof(patientData));
    }

    public async Task Create(CreateAppointmentRequest request)
    {
        Validation(request);

        var patient = await _patientData.Get(request.PatientId);
        
        if (patient == null)
        {
            throw new InvalidOperationException("Patient not Found");
        }

        var patientName = $"{patient.FirstName} {patient.LastName}";

        var appointment = new AppointmentEntity
        {
            PatientId = patient.Id,
            AppointmentInterval = request.AppointmentInterval,
            AppointmentStart = request.AppointmentStart,
            PatientName = patientName,
        };

        _appointmentsData.CreateAppointment(appointment);
    }

    private static void Validation(CreateAppointmentRequest request)
    {
        if(request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if(request.AppointmentStart < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Appointment can't start in the past");
        }

        // etc.
    }
}
