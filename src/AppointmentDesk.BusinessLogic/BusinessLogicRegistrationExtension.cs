using Microsoft.Extensions.DependencyInjection;

namespace AppointmentDesk.BusinessLogic;

public static class BusinessLogicRegistrationExtension
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddTransient<IAppointmentService, AppointmentService>();

        return services;
    }
}