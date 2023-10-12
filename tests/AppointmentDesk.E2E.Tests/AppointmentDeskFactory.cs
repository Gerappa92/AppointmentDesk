using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using AppointmentDesk.DataAccess.Interfaces;
using WireMock.Server;

namespace AppointmentDesk.E2E.Tests;

internal class AppointmentDeskFactory : WebApplicationFactory<Program>
{
    public WireMockServer ServerMock { get; init; }

    public AppointmentDeskFactory()
    {
        ServerMock = WireMockServer.Start();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureTestServices(MockPatientApi);
    }

    private void MockPatientApi(IServiceCollection services)
    {
        services.AddRefitClient<IPatientApi>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(ServerMock.Url!);
            });
    }

    protected override void Dispose(bool disposing)
    {
        ServerMock.Dispose();
        base.Dispose(disposing);
    }
}
