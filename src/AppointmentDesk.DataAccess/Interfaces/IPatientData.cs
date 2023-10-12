using System.Threading.Tasks;
using AppointmentDesk.DataAccess.Entities;

namespace AppointmentDesk.DataAccess.Interfaces;

public interface IPatientData
{
    Task<PatientEntity> Get(int id);
}
