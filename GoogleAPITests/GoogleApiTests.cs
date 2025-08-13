using NUnit.Framework;
using RestSharp;
using FluentAssertions;
using System.Net;

namespace GoogleAPITests;

[TestFixture]
public class GoogleApiTests
{
    private RestClient _client = null!;
    private const string BaseUrl = "https://www.google.com";

    [SetUp]
    public void Setup()
    {
        var options = new RestClientOptions(BaseUrl)
        {
            ThrowOnAnyError = false,
            MaxTimeout = 30000
        };
        _client = new RestClient(options);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
    }

    [Test]
    [Category("API")]
    [Category("Smoke")]
    public async Task GoogleHomepage_ShouldReturn200_WhenRequested()
    {
        // Arrange
        var request = new RestRequest("/", Method.Get);
        request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNullOrEmpty();
        response.Content.Should().Contain("Google");
    }

    [Test]
    [Category("API")]
    [Category("Regression")]
    public async Task GoogleHomepage_ShouldContainExpectedElements_WhenRequested()
    {
        // Arrange
        var request = new RestRequest("/", Method.Get);
        request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().Contain("<title>Google</title>");
        response.Content.Should().Contain("name=\"q\""); // Search input
        response.Content.Should().Contain("Google Search");
    }

    [Test]
    [Category("API")]
    [Category("Performance")]
    public async Task GoogleHomepage_ShouldRespondWithinTimeout_WhenRequested()
    {
        // Arrange
        var request = new RestRequest("/", Method.Get);
        request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var response = await _client.ExecuteAsync(request);
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000, "Response should be received within 5 seconds");
    }

    [Test]
    [Category("API")]
    [Category("Security")]
    public async Task GoogleHomepage_ShouldHaveSecurityHeaders_WhenRequested()
    {
        // Arrange
        var request = new RestRequest("/", Method.Get);
        request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Check for common security headers
        var headers = response.Headers?.Where(h => h.Name != null)
            .GroupBy(h => h.Name!.ToLower())
            .ToDictionary(g => g.Key, g => g.First().Value?.ToString() ?? string.Empty);
        
        // Google typically includes these security-related headers
        headers.Should().NotBeNull();
        
        // Check if any security headers are present (Google may not always include all)
        var hasSecurityHeaders = headers!.ContainsKey("x-frame-options") || 
                               headers.ContainsKey("x-xss-protection") ||
                               headers.ContainsKey("strict-transport-security");
        
        hasSecurityHeaders.Should().BeTrue("At least one security header should be present");
    }

    [TestCase("/search?q=test")]
    [TestCase("/images")]
    [TestCase("/maps")]
    [Category("API")]
    [Category("Regression")]
    public async Task GoogleEndpoints_ShouldBeAccessible_WhenRequested(string endpoint)
    {
        // Arrange
        var request = new RestRequest(endpoint, Method.Get);
        request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Found, HttpStatusCode.MovedPermanently);
        response.Content.Should().NotBeNullOrEmpty();
    }
}