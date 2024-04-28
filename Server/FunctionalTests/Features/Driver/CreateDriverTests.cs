using Api.Features.Driver;
using Api.Repositories;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace FunctionalTests.Features.Driver;

[Collection("DriverTestsCollection")]
public class CreateDriverTests 
{
    private readonly HttpClient _httpClient;
    private readonly IDriverRepository _repository;

    public CreateDriverTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
        _repository = factory.Services.GetRequiredService<IDriverRepository>();
    }

    private CreateDriverEndpoint.Request ValidRequest => new Faker<CreateDriverEndpoint.Request>()
        .CustomInstantiator(f =>
                     new CreateDriverEndpoint.Request(
                        f.Name.FullName(),
                        f.Address.StreetAddress(),
                        f.Address.StreetAddress(),
                        f.Phone.PhoneNumber()
                         )
        );

    [Fact]
    public async Task Can_Create_Driver()
    {
        // Arrange
        var request = ValidRequest;

        // Act
        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/driver", content);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var parsedResponse = await response.Content.ReadFromJsonAsync<CreateDriverEndpoint.Response>();

        parsedResponse.Should().NotBeNull();
        parsedResponse.DriverId.Should().NotBe(0);
        parsedResponse.Name.Should().Be(request.Name);
        parsedResponse.AddressLine1.Should().Be(request.AddressLine1);
        parsedResponse.AddressLine2.Should().Be(request.AddressLine2);
        parsedResponse.PhoneNumber.Should().Be(request.PhoneNumber);
    }

    private CreateDriverEndpoint.Request InvalidRequest => new Faker<CreateDriverEndpoint.Request>()
    .CustomInstantiator(f =>
                 new CreateDriverEndpoint.Request(
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                     )
    );

    [Fact]
    public async Task Return_Problem_Details_For_Bad_Request()
    {
        // Arrange
        var request = InvalidRequest;

        // Act
        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/driver", content);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails.Status.Should().Be(400);
        problemDetails.Errors.Should().Contain(ext => ext.Key == nameof(CreateDriverEndpoint.Request.Name));
        problemDetails.Errors.Should().Contain(ext => ext.Key == nameof(CreateDriverEndpoint.Request.AddressLine1));
        problemDetails.Errors.Should().Contain(ext => ext.Key == nameof(CreateDriverEndpoint.Request.PhoneNumber));
    }
}
