using Api.Features.Driver.Models;
using Api.Repositories;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace FunctionalTests.Features.Driver;

[Collection("DriverTestsCollection")]
public class GetDriverByIdTests 
{
    private readonly HttpClient _httpClient;
    private readonly IDriverRepository _repository;

    public GetDriverByIdTests(WebApplicationFactory<Program> factory)
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

    [Fact]
    public async Task Can_Get_Driver_By_Id()
    {
        // Arrange
        var savedDriver = await _repository.Insert(TestDriver);

        // Act
        var response = await _httpClient.GetAsync($"/driver/{savedDriver.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var parsedResponse = await response.Content.ReadFromJsonAsync<DriverModel>();

        parsedResponse.Should().NotBeNull();
        parsedResponse.Id.Should().Be(savedDriver.Id);
        parsedResponse.Name.Should().Be(savedDriver.Name);
        parsedResponse.AddressLine1.Should().Be(savedDriver.AddressLine1);
        parsedResponse.AddressLine2.Should().Be(savedDriver.AddressLine2);
        parsedResponse.PhoneNumber.Should().Be(savedDriver.PhoneNumber);
    }

    [Fact]
    public async Task Return_Not_Found_When_No_Driver()
    {
        // Arrange
        var driverid = -1;

        // Act
        var response = await _httpClient.GetAsync($"/driver/{driverid}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
