using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Api.Authentication.Endpoints;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace FunctionalTests.Authentication;

public class AuthenticationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly ITestOutputHelper _testOutputHelper;

    public AuthenticationTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _httpClient = factory.CreateClient();
        _testOutputHelper = testOutputHelper;
    }

    private readonly SignupEndpoint.Request ValidSignupRequest = new Faker<SignupEndpoint.Request>()
        .CustomInstantiator(f =>
                     new SignupEndpoint.Request(
                        f.Name.FullName(),
                        f.Internet.Password(),
                        f.Internet.Email()
                         ));

    [Fact]
    public async Task Can_Signup()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/signup")
        {
            Content = new StringContent(JsonSerializer.Serialize(ValidSignupRequest), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        _testOutputHelper.WriteLine(responseContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var parsedResponse = await response.Content.ReadFromJsonAsync<SignupEndpoint.Response>();
        parsedResponse.Should().NotBeNull();
        parsedResponse.Token.Should().NotBeNullOrEmpty();
    }
}
