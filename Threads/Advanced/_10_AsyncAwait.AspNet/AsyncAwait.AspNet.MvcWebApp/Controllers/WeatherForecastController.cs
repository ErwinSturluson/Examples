using AsyncAwait.AspNet.MvcWebApp.Models;
using AsyncAwait.AspNet.WebApiService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AsyncAwait.AspNet.MvcWebApp.Controllers
{
    public class WeatherForecastController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        public IActionResult GetWeatherForecastExample()
        {
            ViewData["WeatherForecasts"] = DownloadWeatherForecastAsync().Result;

            return View();
        }

        public async Task<IActionResult> GetWeatherForecastAsyncExample()
        {
            ViewData["WeatherForecasts"] = await DownloadWeatherForecastAsync();

            return View();
        }

        private async Task<IEnumerable<WeatherForecast>?> DownloadWeatherForecastAsync()
        {
            using HttpClient client = new();

            IEnumerable<WeatherForecast>? weatherForecast = await client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("https://localhost:50201/WeatherForecast");

            return weatherForecast;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
