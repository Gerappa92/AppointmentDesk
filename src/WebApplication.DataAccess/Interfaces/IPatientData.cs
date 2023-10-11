﻿using System.Threading.Tasks;
using WebApplication.DataAccess.Entities;

namespace WebApplication.DataAccess.Interfaces;

internal interface IPatientData
{
    Task<PatientEntity> Get(int id);
}
