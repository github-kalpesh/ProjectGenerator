using Microsoft.AspNetCore.Mvc;
using ProjectGenerator.Executor;

namespace ProjectGenerator.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public HomeController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Execute")]
        public IEnumerable<WeatherForecast> Get()
        {
            var configuration = new Configuration();
            configuration.ConnectionString = @"Data Source=DESKTOP-T312584;Initial Catalog=BlueLakeCeramic;Integrated Security=True;";
            configuration.ProjectName = "BlueLakeCeramic";
            configuration.OutputDirectory = @"D:\Work\DotNet\LocalProject\PG_Test\PG_Test1";
            configuration.Pages = new List<PageDetail>() {
                new PageDetail()
                {
                    TableName = "t_CustomInvoice",
                    PageName = "Custom Invoice",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_CustomInvoiceDetail",
                    PageName = "CustomInvoiceDetail",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = false,
                },
                new PageDetail()
                {
                    TableName = "t_CustomInvoiceManufactuingCompany",
                    PageName = "CustomInvoiceManufactuingCompany",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = false,
                }
            };
            Execute execute = new Execute();
            execute.Generate(configuration);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
