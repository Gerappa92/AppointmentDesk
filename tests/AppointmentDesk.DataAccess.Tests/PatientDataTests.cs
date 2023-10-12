using System.Net;
using AutoFixture;
using FluentAssertions;
using Refit;
using AppointmentDesk.DataAccess.Entities;
using AppointmentDesk.DataAccess.Exceptions;
using AppointmentDesk.DataAccess.Interfaces;
using WireMock.FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace AppointmentDesk.DataAccess.Tests;

[TestFixture]
public class PatientDataTests
{
    private Fixture _fixture;
    private WireMockServer _server;

    [OneTimeSetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _server = WireMockServer.Start();
    }

    private PatientData CreatePatientData()
    {
        var patientApi = RestService.For<IPatientApi>(_server.Url!);
        return new PatientData(patientApi);
    }

    [Test]
    public async Task Get_WithStatusCodeOk_ReceivedAndMapPatient()
    {
        // Arrange
        var patient = _fixture.Create<PatientEntity>();
        var patientJson = System.Text.Json.JsonSerializer.Serialize(patient);

        _server
            .Given(
                Request
                    .Create()
                    .WithPath($"/patient/{patient.Id}")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBody(patientJson)
            );

        var patientData = CreatePatientData();


        // Act
        var result = await patientData.Get(patient.Id);

        // Assert
        _server.Should().HaveReceivedACall().UsingGet()
            .And.AtUrl($"{_server.Url}/patient/{patient.Id}");

        result.Id.Should().Be(patient.Id);
        result.FirstName.Should().Be(patient.FirstName);
        result.LastName.Should().Be(patient.LastName);
    }

    [Test]
    public async Task Get_WithStatusCodeInternalServerError_ThrowsExternalServiceException()
    {
        // Arrange
        var patient = _fixture.Create<PatientEntity>();

        _server
            .Given(
                Request
                    .Create()
                    .WithPath($"/patient/{patient.Id}")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(HttpStatusCode.InternalServerError)
            );

        var patientData = CreatePatientData();

        // Act
        var act = () =>  patientData.Get(patient.Id);

        // Assert
        await act.Should().ThrowAsync<ExternalServiceException>();

        _server.Should().HaveReceivedACall().UsingGet()
            .And.AtUrl($"{_server.Url}/patient/{patient.Id}");
    }

    [Test] public async Task Get_WithStatusCodeNotFound_ReturnNull()
    {
        // Arrange
        var patient = _fixture.Create<PatientEntity>();

        _server
            .Given(
                Request
                    .Create()
                    .WithPath($"/patient/{patient.Id}")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(HttpStatusCode.NotFound)
            );

        var patientData = CreatePatientData();

        // Act
        var result = await patientData.Get(patient.Id);

        // Assert
        result.Should().BeNull();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _server.Stop();
    }
}
