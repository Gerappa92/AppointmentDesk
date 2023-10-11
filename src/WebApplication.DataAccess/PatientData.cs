using System;
using System.Threading.Tasks;
using WebApplication.DataAccess.Entities;
using WebApplication.DataAccess.Interfaces;

namespace WebApplication.DataAccess;

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
