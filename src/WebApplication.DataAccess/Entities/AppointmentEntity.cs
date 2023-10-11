using System;

namespace WebApplication.DataAccess.Entities;

public class AppointmentEntity
{
    public DateTime AppointmentStart { get; set; }
    public TimeSpan AppointmentInterval { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; }
}