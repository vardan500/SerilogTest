using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace SerilogTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        readonly IDiagnosticContext _diagnosticContext;
        readonly ILogger _logger;
        private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public HomeController(IDiagnosticContext diagnosticContext, ILogger logger)
        {
            _diagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
            _logger = logger;
        }

        public IEnumerable<WeatherForecast> Index()
        {

            try
            {
                // The request completion event will carry this property
                _diagnosticContext.Set("CatalogLoadTime", 1423);

                int i = 0;
                int j = 2 / i;

                var rng = new Random();
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
            } catch(Exception ex)
            {
                _logger.Error("Patricia Rhomberg error!");
                throw;
            }
        }
    }
}
