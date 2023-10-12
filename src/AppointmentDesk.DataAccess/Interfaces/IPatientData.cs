using System.Threading.Tasks;
using AppointmentDesk.DataAccess.Entities;

namespace AppointmentDesk.DataAccess.Interfaces;

internal interface IPatientData
{
    Task<PatientEntity> Get(int id);
}
