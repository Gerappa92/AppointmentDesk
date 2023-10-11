using Microsoft.Extensions.DependencyInjection;

namespace WebApplication.BusinessLogic;

public static class BusinessLogicRegistrationExtension
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddTransient<IAppointmentService, AppointmentService>();

        return services;
    }
}