using System;
using System.Threading.Tasks;
using AppointmentDesk.DataAccess.Entities;
using AppointmentDesk.DataAccess.Interfaces;

namespace AppointmentDesk.DataAccess;

public class PatientData : IPatientData
{
    private readonly IPatientApi _api;
    public PatientData(IPatientApi api)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
    }
    public Task<PatientEntity> Get(int id)
    {
        throw new NotImplementedException();
    }
}
