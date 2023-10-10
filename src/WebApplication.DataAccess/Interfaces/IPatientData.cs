using Refit;
using WebApplication.DataAccess.Entities;

namespace WebApplication.DataAccess.Interfaces
{
    public interface IPatientData
    {
        [Get("/patient/{id}")]
        PatientEntity Get(int id);
    }
}