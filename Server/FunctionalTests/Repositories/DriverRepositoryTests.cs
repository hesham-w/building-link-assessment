using Api.Domain;
using Api.Repositories;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace FunctionalTests.Repositories;

public class DriverRepositoryTests
{
    private readonly IDriverRepository _sut;

    public DriverRepositoryTests()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Test.json")
        .Build();

        _sut = new DriverRepository(configuration);
    }

    private Driver TestDriver => new Faker<Driver>()
            .RuleFor(d => d.Name, f => f.Name.FullName())
            .RuleFor(d => d.AddressLine1, f => f.Address.StreetAddress())
            .RuleFor(d => d.AddressLine2, f => f.Address.SecondaryAddress())
            .RuleFor(d => d.PhoneNumber, f => f.Phone.PhoneNumber())
            .Generate();

    [Fact]
    public async Task Can_Insert_Driver()
    {
        // Arrange
        var testDriver = TestDriver;

        // Act
        var savedDriver = await _sut.Insert(testDriver);

        // Assert
        savedDriver.Id.Should().NotBe(0);
    }

    [Fact]
    public async Task Can_Get_DriverById()
    {
        // Arrange
        var testDriver = TestDriver;
        var _ = await _sut.Insert(testDriver);

        // Act
        var driver = await _sut.GetById(testDriver.Id);

        // Assert
        driver.Id.Should().Be(testDriver.Id);
        driver.AddressLine1.Should().Be(testDriver.AddressLine1);
        driver.AddressLine2.Should().Be(testDriver.AddressLine2);
        driver.PhoneNumber.Should().Be(testDriver.PhoneNumber);
    }
}