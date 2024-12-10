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
            configuration.ConnectionString = @"Data Source=DESKTOP-T312584;Initial Catalog=HospitalManagement;Integrated Security=True;";
            configuration.ProjectName = "HospitalManagement.Web";
            configuration.OutputDirectory = @"D:\Work\DotNet\LiveProject\HospitalManagement\PG";
            configuration.Pages = new List<PageDetail>() {
                new PageDetail()
                {
                    TableName = "t_AppSetting",
                    PageName = "Setting",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_Users",
                    PageName = "Users",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_Patients",
                    PageName = "Patient",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_Staffs",
                    PageName = "Staff",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_PatientAttendance",
                    PageName = "Patient Attendance",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_StaffAttendance",
                    PageName = "Staff Attendance",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_PaidFees",
                    PageName = "Paid Fees",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_Leave",
                    PageName = "Leave",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_Instruments",
                    PageName = "Instruments",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_Diagnostic",
                    PageName = "Diagnostic",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_PatientFollowUp",
                    PageName = "Patient FollowUp",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_Salary",
                    PageName = "Salary",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_Documents",
                    PageName = "Documents",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_LOV",
                    PageName = "List Of Values",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                },
                new PageDetail()
                {
                    TableName = "t_Hospital",
                    PageName = "Hospital",
                    IsGenerateBackEnd = true,
                    IsGenerateFrontEnd = true,
                }

            };
            configuration.IsOverride = true;
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
