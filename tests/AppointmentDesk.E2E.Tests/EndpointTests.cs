using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using AppointmentDesk.BusinessLogic;
using AppointmentDesk.DataAccess.Entities;
using WireMock.FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace AppointmentDesk.E2E.Tests;

[TestFixture]
internal class EndpointTests : IDisposable
{
    private Fixture _fixture;
    private AppointmentDeskFactory _factory;
    private HttpClient _httpClient;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _fixture = new Fixture();
        _factory = new AppointmentDeskFactory();
        _httpClient = _factory.CreateClient();
    }

    [Test]
    public async Task CreateAppointment_ValidRequest_ReturnsOk()
    {
        // Arrange
        var patient = _fixture.Create<PatientEntity>();
        var patientJson = System.Text.Json.JsonSerializer.Serialize(patient);
        var getPatientPath = $"/patient/{patient.Id}";

        _factory.ServerMock
            .Given(Request.Create().WithPath(getPatientPath).UsingGet())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBody(patientJson));

        // Act
        var createAppointmentPath = $"/appointment";
        var createAppointmentRequestBody = new CreateAppointmentRequest(patient.Id, DateTime.MaxValue, TimeSpan.FromHours(1));
        var httpResponse = await _httpClient.PostAsJsonAsync(createAppointmentPath, createAppointmentRequestBody);

        // Assert
        _factory.ServerMock.Should().HaveReceivedACall().UsingGet()
            .And.AtUrl($"{_factory.ServerMock.Url}/patient/{patient.Id}");

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task CreateAppointment_PatientNotFound_ReturnsBadRequest()
    {
        // Arrange
        var patient = _fixture.Create<PatientEntity>();
        var patientJson = System.Text.Json.JsonSerializer.Serialize(patient);
        var getPatientPath = $"/patient/{patient.Id}";

        _factory.ServerMock
            .Given(Request.Create().WithPath(getPatientPath).UsingGet())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.NotFound));

        // Act
        var createAppointmentPath = $"/appointment";
        var createAppointmentRequestBody = new CreateAppointmentRequest(patient.Id, DateTime.MaxValue, TimeSpan.FromHours(1));
        var httpResponse = await _httpClient.PostAsJsonAsync(createAppointmentPath, createAppointmentRequestBody);

        // Assert
        _factory.ServerMock.Should().HaveReceivedACall().UsingGet()
            .And.AtUrl($"{_factory.ServerMock.Url}/patient/{patient.Id}");

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task CreateAppointment_PatientInternalServerException_ReturnsInternalServerException()
    {
        // Arrange
        var patient = _fixture.Create<PatientEntity>();
        var getPatientPath = $"/patient/{patient.Id}";

        _factory.ServerMock
            .Given(Request.Create().WithPath(getPatientPath).UsingGet())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.InternalServerError));

        // Act
        var createAppointmentPath = $"/appointment";
        var createAppointmentRequestBody = new CreateAppointmentRequest(patient.Id, DateTime.MaxValue, TimeSpan.FromHours(1));
        var httpResponse = await _httpClient.PostAsJsonAsync(createAppointmentPath, createAppointmentRequestBody);

        // Assert
        _factory.ServerMock.Should().HaveReceivedACall().UsingGet()
            .And.AtUrl($"{_factory.ServerMock.Url}/patient/{patient.Id}");

        httpResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    public void Dispose() => _factory.Dispose();
}
