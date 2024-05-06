using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Api;
using Api.Authentication.Endpoints;
using Api.Domain;
using Api.Repositories;
using Bogus;
using Chirper.Authentication.Endpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace FunctionalTests.Authentication;

public class LoginTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly IUserRepository _repository;
    private readonly ITestOutputHelper _testOutputHelper;

    public LoginTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _httpClient = factory.CreateClient();
        _repository = factory.Services.GetRequiredService<IUserRepository>();
        _testOutputHelper = testOutputHelper;
    }

    private readonly User FakeUser = new Faker<User>()
            .RuleFor(d => d.Name, f => f.Name.FullName())
            .RuleFor(d => d.Password, f => f.Internet.Password())
            .RuleFor(d => d.Email, f => f.Internet.Email())
            .Generate();

    [Fact]
    public async Task Can_Login()
    {
        // Arrange
        var _ = await _repository.InsertAsync(FakeUser);

        var loginRequest = new LoginEndpoint.Request("admin", "admin");

        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login");
        request.Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        _testOutputHelper.WriteLine(responseContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var parsedResponse = await response.Content.ReadFromJsonAsync<LoginEndpoint.Response>();
        parsedResponse.Should().NotBeNull();
        parsedResponse.Token.Should().NotBeNullOrEmpty();
    }
}
