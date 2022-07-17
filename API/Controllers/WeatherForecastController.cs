using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class WeatherForecastController : BaseApiController
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
    [Route("WhatsWeather.{format}"), FormatFilter]
    [Route("WhatsWeather")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var result =  await Task.Run( () =>   Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }));

        return  result;
    }

    [HttpPost]
    [Route("Add")]
    [Route("Add.{format}"), FormatFilter]
    public async Task AddWeather(WeatherForecast wthForcast)
    {

    }

}
