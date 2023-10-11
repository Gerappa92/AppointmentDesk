using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using WebApplication.DataAccess.Interfaces;

namespace WebApplication.DataAccess;

public static class DataAccessRegistrationExtension
{
    private const string PatientApiUrlPosition = "PatientApiUrl";
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
    {
        var patientUrl = config[PatientApiUrlPosition] ?? throw new InvalidOperationException("Patient Api Url is not configured");

        services.AddRefitClient<IPatientApi>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(patientUrl);
            });

        services.AddTransient<IAppointmentsData, AppointmentsData>();

        return services;
    }
}
