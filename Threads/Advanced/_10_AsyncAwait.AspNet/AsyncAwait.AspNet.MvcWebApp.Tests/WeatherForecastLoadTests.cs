using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using Xunit.Abstractions;

namespace AsyncAwait.AspNet.MvcWebApp.Tests;

public class WeatherForecastLoadTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly ITestOutputHelper _output;

    public WeatherForecastLoadTests(WebApplicationFactory<Program> webApplicationFactory, ITestOutputHelper output)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _output = output;
    }

    [Fact]
    public void GetWeatherForecastExample()
    {
        Task<TimeSpan> measurementHome = MeasureHttpGetRequest("Home/Index", 1000);
        Task<TimeSpan> measurementWeatherForecast = MeasureHttpGetRequest("WeatherForecast/GetWeatherForecastExample", 1000);

        string reportHome = $"AVG[Home]:[{measurementHome.Result.Milliseconds}](ms)";
        string reportWeatherForecast = $"AVG[WeatherForecast]:[{measurementWeatherForecast.Result.Milliseconds}](ms)";

        _output.WriteLine(reportHome);
        _output.WriteLine(reportWeatherForecast);
    }

    [Fact]
    public async Task GetWeatherForecastAsyncExample()
    {
        Task<TimeSpan> measurementHome = MeasureHttpGetRequest("Home/Index", 1000);
        Task<TimeSpan> measurementWeatherForecast = MeasureHttpGetRequest("WeatherForecast/GetWeatherForecastExample", 1000);

        await Task.WhenAll(measurementHome, measurementWeatherForecast);

        string reportHome = $"AVG[Home]:[{measurementHome.Result.Milliseconds}](ms)";
        string reportWeatherForecast = $"AVG[WeatherForecastAsync]:[{measurementWeatherForecast.Result.Milliseconds}](ms)";

        _output.WriteLine(reportHome);
        _output.WriteLine(reportWeatherForecast);
    }

    private async Task<TimeSpan> MeasureHttpGetRequest(string testUri, int requestNumber)
    {
        IEnumerable<Task<TimeSpan>> tasks = Enumerable.Range(1, requestNumber).Select(i =>
        {
            return MeasureHttpGetRequest(testUri);
        });

        TimeSpan[] measurements = await Task.WhenAll(tasks);

        double averageMeasurement = measurements.Average((ts) => ts.Milliseconds);

        return TimeSpan.FromMilliseconds(averageMeasurement);
    }

    private async Task<TimeSpan> MeasureHttpGetRequest(string uri)
    {
        Stopwatch sw = Stopwatch.StartNew();

        await _httpClient.GetAsync(uri);

        sw.Stop();

        return sw.Elapsed;
    }
}
