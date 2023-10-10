using WebApplication.DataAccess.Entities;

namespace WebApplication.DataAccess.Interfaces
{
    public interface IPatientData
    {
        PatientEntity Get(int id);
    }
}