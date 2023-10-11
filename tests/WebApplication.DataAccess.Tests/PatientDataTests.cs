using System.Net;
using AutoFixture;
using FluentAssertions;
using Refit;
using WebApplication.DataAccess.Entities;
using WebApplication.DataAccess.Exceptions;
using WebApplication.DataAccess.Interfaces;
using WireMock.FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace WebApplication.DataAccess.Tests;

[TestFixture]
public class PatientDataTests
{
    private Fixture _fixture;
    private WireMockServer _server;

    [SetUp]
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
        const int id = 1;
        var patientData = CreatePatientData();
        var patient = _fixture.Create<PatientEntity>();
        var patientJson = System.Text.Json.JsonSerializer.Serialize(patient);

        _server
            .Given(
                Request
                    .Create()
                    .WithPath($"/patient/{id}")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBody(patientJson)
            );


        // Act
        var result = await patientData.Get(
            id);

        // Assert
        _server.Should().HaveReceivedACall().UsingGet()
            .And.AtUrl($"{_server.Url}/patient/{id}");

        result.Id.Should().Be(patient.Id);
        result.FirstName.Should().Be(patient.FirstName);
        result.LastName.Should().Be(patient.LastName);
    }

    [Test]
    public async Task Get_WithStatusCodeInternalServerError_ThrowsExternalServiceException()
    {
        // Arrange
        const int id = 1;
        var patientData = CreatePatientData();
        var patient = _fixture.Create<PatientEntity>();
        var patientJson = System.Text.Json.JsonSerializer.Serialize(patient);

        _server
            .Given(
                Request
                    .Create()
                    .WithPath($"/patient/{id}")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(HttpStatusCode.InternalServerError)
                    .WithBody(patientJson)
            );


        // Act
        var act = () =>  patientData.Get(
            id);

        // Assert
        await act.Should().ThrowAsync<ExternalServiceException>();

        _server.Should().HaveReceivedACall().UsingGet()
            .And.AtUrl($"{_server.Url}/patient/{id}");
    }

    [Test] public async Task Get_WithStatusCodeNotFound_ReturnNull()
    {
        // Arrange
        const int id = 1;
        var patientData = CreatePatientData();
        var patient = _fixture.Create<PatientEntity>();
        var patientJson = System.Text.Json.JsonSerializer.Serialize(patient);

        _server
            .Given(
                Request
                    .Create()
                    .WithPath($"/patient/{id}")
                    .UsingGet()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(HttpStatusCode.NotFound)
                    .WithBody(patientJson)
            );


        // Act
        var result = await patientData.Get(id);

        // Assert
        result.Should().BeNull();
    }

    [TearDown]
    public void TearDown()
    {
        _server.Stop();
    }
}
