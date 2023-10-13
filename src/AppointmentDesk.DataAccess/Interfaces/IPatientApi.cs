using System.Threading.Tasks;
using AppointmentDesk.DataAccess.Entities;
using Refit;

namespace AppointmentDesk.DataAccess.Interfaces;

public interface IPatientApi
{
    [Get("/patient/{id}")]
    Task<ApiResponse<PatientEntity>> Get(int id);
}