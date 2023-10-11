using System.Threading.Tasks;
using Refit;
using WebApplication.DataAccess.Entities;

namespace WebApplication.DataAccess.Interfaces;

public interface IPatientApi
{
    [Get("/patient/{id}")]
    Task<PatientEntity> Get(int id);
}