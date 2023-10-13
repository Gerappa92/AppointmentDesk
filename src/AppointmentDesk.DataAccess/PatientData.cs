using System;
using System.Threading.Tasks;
using AppointmentDesk.DataAccess.Entities;
using AppointmentDesk.DataAccess.Exceptions;
using AppointmentDesk.DataAccess.Interfaces;
using Refit;

namespace AppointmentDesk.DataAccess;

public class PatientData : IPatientData
{
    private readonly IPatientApi _api;
    public PatientData(IPatientApi api)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
    }
    public async Task<PatientEntity> Get(int id)
    {
        var response = await _api.Get(id);

        if(response.IsSuccessStatusCode) 
        {
            return response.Content;
        }

        if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new ExternalServiceException("An error occurred while making an API to Patient Api", response.Error);
    }
}
