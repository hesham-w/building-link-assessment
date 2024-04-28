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
public class UpdateDriverTests
{
    private readonly HttpClient _httpClient;
    private readonly IDriverRepository _repository;

    public UpdateDriverTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
        _repository = factory.Services.GetRequiredService<IDriverRepository>();
    }

    private Api.Domain.Driver TestDriver => new Faker<Api.Domain.Driver>()
            .RuleFor(d => d.Name, f => f.Name.FullName())
            .RuleFor(d => d.AddressLine1, f => f.Address.StreetAddress())
            .RuleFor(d => d.AddressLine2, f => f.Address.SecondaryAddress())
            .RuleFor(d => d.PhoneNumber, f => f.Phone.PhoneNumber())
            .Generate();

    private UpdateDriverEndpoint.Request UpdateRequest(int DriverId) => new Faker<UpdateDriverEndpoint.Request>()
        .CustomInstantiator(f =>
                     new UpdateDriverEndpoint.Request(
                        DriverId,
                        f.Name.FullName(),
                        f.Address.StreetAddress(),
                        f.Address.StreetAddress(),
                        f.Phone.PhoneNumber()
                         ));

    [Fact]
    public async Task Can_Update_Driver()
    {
        // Arrange
        var savedDriver = await _repository.Insert(TestDriver);
        var updateRequest = UpdateRequest(savedDriver.Id);

        // Act
        var jsonRequest = JsonSerializer.Serialize(updateRequest);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("/driver", content);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var parsedResponse = await response.Content.ReadFromJsonAsync<UpdateDriverEndpoint.Response>();

        parsedResponse.Should().NotBeNull();
        parsedResponse.Id.Should().Be(savedDriver.Id);
        parsedResponse.Name.Should().Be(updateRequest.Name);
        parsedResponse.AddressLine1.Should().Be(updateRequest.AddressLine1);
        parsedResponse.AddressLine2.Should().Be(updateRequest.AddressLine2);
        parsedResponse.PhoneNumber.Should().Be(updateRequest.PhoneNumber);
    }

    private UpdateDriverEndpoint.Request InvalidUpdateRequest(int DriverId) => new Faker<UpdateDriverEndpoint.Request>()
        .CustomInstantiator(f =>
                     new UpdateDriverEndpoint.Request(
                        DriverId,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty
                         ));

    [Fact]
    public async Task Return_Not_Found_When_No_Driver()
    {
        // Arrange
        var savedDriver = await _repository.Insert(TestDriver);
        var updateRequest = InvalidUpdateRequest(savedDriver.Id);

        // Act
        var jsonRequest = JsonSerializer.Serialize(updateRequest);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("/driver", content);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails.Status.Should().Be(400);
        problemDetails.Errors.Should().Contain(ext => ext.Key == nameof(CreateDriverEndpoint.Request.Name));
        problemDetails.Errors.Should().Contain(ext => ext.Key == nameof(CreateDriverEndpoint.Request.AddressLine1));
        problemDetails.Errors.Should().Contain(ext => ext.Key == nameof(CreateDriverEndpoint.Request.PhoneNumber));
    }
}
