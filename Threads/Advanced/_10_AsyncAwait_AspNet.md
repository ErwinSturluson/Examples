# **Асинхронное программирование в ASP.NET**

Асинхронное программирование является важной частью приложений ASP.NET, так как благодаря ему возможно увеличение пропускной способности входящих запросов для сетевого приложения.

До появления **задач**, **TPL** и ```async await``` асинхронное программирование в ASP.NET было достаточно сложным и являлось непростым для поддержки.

После появления **задач**, **TPL** и ```async await``` асинхронное программирование в ASP.NET сильно упростилось за счёт таких мощных абстракций асинхрорнного программирования.

Теперь для увеличения пропускной способности приложений ASP.NET возможно не только горизонтальное масштабирование за счёт увеличения мощностей и ресурсов инфраструктуры, но и вертикальное масштабирование за счёт написания более оптимального асинхронного кода.

Однако, асинхронный код в приложениях ASP.NET необходимо применять только там, где это необходимо, иначе это может наоборот снизить производительность приложения и привести к проблемам.

### **IIS сервер**

Получением запросов в приложениях ASP.NET на платформе ОС Windows и ОС Windows Server занимается IIS сервер. Именно через него запрос пользователя попадает в веб-приложение.

IIS (Internet Information Services) – расширяемый веб-сервер от компании Microsoft

Веб-сервер IIS поддерживает технологию создания веб-приложений ASP.NET. Технология ASP.NET - это одно из основных средств для создания веб-приложений и веб-служб через IIS сервер.

### **Обработка запросов**

Каждый входящий запрос в веб-приложение ASP.NET обрабатывается потоком из пула потоков. Поток из пула потоков не будет освобожден, пока полностью не обработает пришедший запрос.

Если для запроса нет свободного потока в пуле для обработки входящего запроса, то запрос становится в очередь ожидания.

Очередь ожидания имеет свой настраиваемый предел (по умолчанию 1000 запросов), по достижению этого предела, пользователи начнут получать в ответ статус код **503 Service Unavailable** (Сервис недоступен), что явялется ошибочным и неправильным поведением сетевого ресурса.

### **Синхронная обработка запросов в ASP.NET**

> TODO: Анимированное изображение GIF с демонстрацией работы синхронной обработки запросов в ASP.NET.

> TODO: текстовое описание в свободной форме.

## **Асинхронная обработка запросов в ASP.NET**

> TODO: Анимированное изображение GIF с демонстрацией работы асинхронной обработки запросов в ASP.NET.

> TODO: текстовое описание в свободной форме.

### **Применение асинхронности в ASP.NET**

**Асинхронные методы в ASP.NET** рекомендуется использовать при операциях ввода-вывода, при обращении к удалённым серверам или при длительных вычислениях, например:
- Работа с файловой системой;
- Работа с базами данных;
- Работа с сетевыми запросами;
- Работа с удалёнными сетевыми сервисами;
- Сложные длительные вычисления (создание файлов, расчёт внесённых данных).

**Асинхронные методы в ASP.NET** рекомендуется использовать при простых операциях:
- Возврат статических данных;
- Выполнение простых и непродолжительных вычислений;
- Запуск продолжительных операций, не требующих моментального ответа (передача данных в очередь, запуск фонового вычисления).

### **Асинхронные методы действия в ASP.NET**

**Синхронные** методы действия в ASP.NET:
```cs
public ActionResult GetIterations()
{
    IEnumerable<string> iterations = _remoteSerivce.PrintIterations();_
    return View(iterations);
}
```

**Асинхронные** методы действия в ASP.NET:
```cs
public async Task<ActionResult> GetIterations()
{
    IEnumerable<string> iterations = await _remoteSerivce.PrintIterationsAsync();_
    return View(iterations);
}
```

# **Применение асинхронного кода в приложении ASP.NET**
---
Для демонстрации применения асинхронного кода в приложении ASP.NET создадим решение с тремя проектами:
- Проект по шаблону **ASP.NET Core Web App (Model-View-Controller)** с названием **AsyncAwait.AspNet.MvcWebApp**, который будет представлять пользовательское сетевое приложение;
- Проект по шаблону **ASP.NET Core Web API** с названием **AsyncAwait.AspNet.WebApiService**, который будет представлять удалённый сетевой сервис;
- Проект по шаблону **Class Library** с названием **AsyncAwait.AspNet.WebApiService.ViewModels**, который будет содержать общие для остальных проектов типы данных, на которых будут строиться форматы сообщений, передаваемых между ними;
- Проект по шаблону **xUnit Test Project** с названием **AsyncAwait.AspNet.MvcWebApp.Tests**, который будет содержать нагрузочные тесты для демонстрации влияния на производительность асинхронных методов в ASP.NET. 

## **01 Изменения в проекте AsyncAwait.AspNet.WebApiService.ViewModels**

Переносим файл с классом **WeatherForecast** в в проект **WebApiService.ViewModels**:

> AsyncAwait.AspNet.WebApiService.ViewModels.csproj -> WeatherForecast.cs
```cs
namespace AsyncAwait.AspNet.WebApiService.ViewModels;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
```

## **02 Изменения в проекте AsyncAwait.AspNet.WebApiService**

- Добавляем ссылку на проект **AsyncAwait.AspNet.WebApiService.ViewModels**;

- Подключаем в файле **WeatherForecastController.cs** пространство имён **AsyncAwait.AspNet.WebApiService.ViewModels**:

> AsyncAwait.AspNet.WebApiService -> WeatherForecastController.cs
```cs
using AsyncAwait.AspNet.WebApiService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AsyncAwait.AspNet.WebApiService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        Thread.Sleep(2000);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
```

- Меняем настройки запуска в файле **launchSettings.json**:
> AsyncAwait.AspNet.WebApiService -> Properties/launchSettings.json
```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "launchUrl": "weatherforecast",
      "applicationUrl": "http://localhost:40201",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "launchUrl": "weatherforecast",
      "applicationUrl": "https://localhost:50201;http://localhost:40201",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### **03 Изменения в проекте AsyncAwait.AspNet.MvcWebApp**

- Добавляем ссылку на проект **AsyncAwait.AspNet.WebApiService.ViewModels**;

- Меняем лимит пула потоков в самом начале метода ```Main()``` класса ```Program```:
> AsyncAwait.AspNet.MvcWebApp -> Program.cs
```cs
namespace AsyncAwait.AspNet.MvcWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        ThreadPool.SetMaxThreads(10, 10);

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change
            // this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
```

- Меняем настройки запуска в файле **launchSettings.json**:
> AsyncAwait.AspNet.MvcWebApp -> Properties/launchSettings.json
```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "http://localhost:40101",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "https://localhost:50101;http://localhost:40101",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

- Удаляем лишний код из файла **HomeController.cs**:
> AsyncAwait.AspNet.MvcWebApp -> Controllers/HomeController.cs
```cs
using AsyncAwait.AspNet.MvcWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AsyncAwait.AspNet.MvcWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
```

- Добавляем файл **WeatherForecastController.cs**, содержащий класс ```WeatherForecastController``` с 2 методами действия, представляющими асинхронную и синхронную работы методов приложения ASP.NET:
> AsyncAwait.AspNet.MvcWebApp -> Controllers/WeatherForecastController.cs
```cs
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
```

- Удаляем лишнее и меняем файл **Index.cshtml**:
> AsyncAwait.AspNet.MvcWebApp -> Views/Home/Index.cshtml
```html
@{
    ViewData["Title"] = "Home Page";
}

<div class="text-center">
    <h1 class="display-4">Welcome</h1>
    <h3 class="display-6">This is an example of Async Await in ASP.NET applications</h3>
</div>
```

- Меняем содержимое в теге ```<header></header>``` в файле **_Layout.cshtml**:
> AsyncAwait.AspNet.MvcWebApp -> Views/Shared/Index.cshtml
```html
<header>
    <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
        <div class="container-fluid">
            <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">AsyncAwait.AspNet.MvcWebApp</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                <ul class="navbar-nav flex-grow-1">
                    <li class="nav-item">
                        <a class="nav-link text-dark" asp-area="" asp-controller="WeatherForecast" asp-action="GetWeatherForecastExample">Weather Forecast</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" asp-area="" asp-controller="WeatherForecast" asp-action="GetWeatherForecastAsyncExample">Weather Forecast Async</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</header>
```

- Создаём в директории **Views** сабдиректорию **WeatherForecast** и добавляем 2 файла представления асинхронной и синхронной работы методов приложения ASP.NET:

> AsyncAwait.AspNet.MvcWebApp -> Views/WeatherForecast/GetWeatherForecastAsyncExample.cshtml
```html
@using AsyncAwait.AspNet.WebApiService.ViewModels;

@{
    ViewData["Title"] = "Get Weather Forecast Async Example";
}

<div class="text-center">
    <h1 class="display-4">Async Await in ASP.NET applications</h1>
    <h2 class="display-6">Weather Forecast Async Example</h2>
</div>

@{
    if (ViewData["WeatherForecasts"] != null)
    {
        IEnumerable<WeatherForecast> weatherForecasts = (IEnumerable<WeatherForecast>)ViewData["WeatherForecasts"]!;

        <style>
            th, td {
                padding: 1em;
                text-align: center;
            }

            thead {
                border-bottom: 1px;
            }
        </style>

        <table>
            <thead>
                <tr>
                    <th>
                        Date
                    </th>
                    <th>
                        TemperatureC
                    </th>
                    <th>
                        TemperatureF
                    </th>
                    <th>
                        Summary
                    </th>
                </tr>
            </thead>
            @foreach (WeatherForecast item in weatherForecasts)
            {
                <tr>
                    <th>
                        @item.Date
                    </th>
                    <th>
                        @item.TemperatureC
                    </th>
                    <th>
                        @item.TemperatureF
                    </th>
                    <th>
                        @item.Summary
                    </th>
                </tr>
            }
            <tbody>
            </tbody>
        </table>
    }
    else
    {
        <p>Loading data...</p>
    }
}
```

> AsyncAwait.AspNet.MvcWebApp -> Views/WeatherForecast/GetWeatherForecastExample.cshtml
```html
@using AsyncAwait.AspNet.WebApiService.ViewModels;

@{
    ViewData["Title"] = "Get Weather Forecast Example";
}

<div class="text-center">
    <h1 class="display-4">Async Await in ASP.NET applications</h1>
    <h2 class="display-6">Weather Forecast Example</h2>
</div>

@{
    if (ViewData["WeatherForecasts"] != null)
    {
        IEnumerable<WeatherForecast> weatherForecasts = (IEnumerable<WeatherForecast>)ViewData["WeatherForecasts"]!;

        <style>
            th, td {
                padding: 1em;
                text-align: center;
            }

        </style>

        <table>
            <thead>
                <tr>
                    <th>
                        Date
                    </th>
                    <th>
                        TemperatureC
                    </th>
                    <th>
                        TemperatureF
                    </th>
                    <th>
                        Summary
                    </th>
                </tr>
            </thead>
            @foreach (WeatherForecast item in weatherForecasts)
            {
                <tr>
                    <th>
                        @item.Date
                    </th>
                    <th>
                        @item.TemperatureC
                    </th>
                    <th>
                        @item.TemperatureF
                    </th>
                    <th>
                        @item.Summary
                    </th>
                </tr>
            }
            <tbody>
            </tbody>
        </table>
    }
    else
    {
        <p>Loading data...</p>
    }
}
```

## **04 Изменения в проекте AsyncAwait.AspNet.WebApiService.Tests**

- Добавляем ссылку на проект **AsyncAwait.AspNet.MvcWebApp**;
- Добавляем nuget-пакет **Microsoft.AspNetCore.Mvc.Testing**;

- Добавляем файл **WeatherForecastLoadTests.cs**, содержащий класс ```WeatherForecastLoadTests```, производящий нагрузочное тестирование асинхронных и синхронных методов действий приложения ASP.NET **AsyncAwait.AspNet.WebApiService**:
> AsyncAwait.AspNet.WebApiService.Tests -> WeatherForecastLoadTests.cs
```cs
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
```

## **05 Проведение нагрузочного тестирования приложения ASP.NET**

Для запуска тестов в **IDE Visual Studio** необходимо открыть окно **TestExplorer** через пункт меню **Test**. При открытии этого окна, оно найдёт все имеющиеся **Unit тесты** и отобразит их списом.

Для запуска тестов необходимо нажать кнопки ПКМ -> **Run** и дождаться результатов. Для чистоты эксперимента рекомендуется запускать тесты **GetWeatherForecastAsyncExample** и **GetWeatherForecastExample** по отдельности, так как они выполняются в рамках одной физической машины.

После запуска могут быть получены следующие результаты среднего времени обработки запросов в миллисекундах:

|Название теста| Действие "Home" | Действие "WeatherForecast"|
|---|---|---|
|GetWeatherForecastExample|890ms|438ms|
|GetWeatherForecastAsyncExample|115ms|466ms|

Что подтверждает то, что правильное использование асинхронных методов действия в приложениях ASP.NET способно привести к приросту производительности, оптимизации и увеличению общей пропускной способности всего сетевого приложения.
 