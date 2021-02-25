using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemo.WebDemo.Controllers
{
    public class WeatherForecastController : Controller
    {
        [Authorize]
        [HttpGet("/api/daily-weather-forecast")]
        public IActionResult GetDailyWeatherForecast()
        {
            _ = HttpContext.User.Claims.ToList();
            return Ok("Cold");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("/api/weekly-weather-forecast")]
        public IActionResult GetWeeklyWeatherForecast()
        {
            _ = HttpContext.User.Claims.ToList();
            return Ok("Cold and raining");
        }
    }
}
