using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Personal_Testing_System.Services;
using Newtonsoft.Json;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Controllers
{
    [ApiController]
    [Route("user-api")]//[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> logger;
        private MasterService ms;
        public EmployeeController(ILogger<EmployeeController> _logger, MasterService _masterService)
        {
            logger = _logger;
            ms = _masterService;
        }

        //[HttpGet(Name = "test")]
        [HttpGet("test")]
        public string test()
        {
            return "test" + DateTime.Now;
        }

        [HttpGet("GetEmployee")]
        public string GetEmployee(int id)
        {
            logger.LogInformation($"/user-api/GetEmployee :id={id}");
            if (ms.Employee.GetEmployeeById(id) != null)
            {
                return JsonConvert.SerializeObject(ms.Employee.GetEmployeeById(id), Formatting.Indented);
                //return Results.Json(ms.Employee.GetEmployeeById(id), new(System.Text.Json.JsonSerializerDefaults.General));
            }
            return JsonConvert.SerializeObject(NotFound(), Formatting.Indented);
            //return Results.Json(NotFound(), new(System.Text.Json.JsonSerializerDefaults.General));
        }
        [HttpPost("AddEmployee")]
        public async Task<string> AddEmployee([FromBody] Employee employee)
        {
            logger.LogInformation($"/user-api-AddEmployee :fn={employee.FirstName}, sn={employee.SecondName}, " +
                                  $" ln={employee.LastName}, idSubdivision={employee.IdSubdivision}");
            if (!string.IsNullOrEmpty(employee.FirstName) && !string.IsNullOrEmpty(employee.SecondName) &&
                !string.IsNullOrEmpty(employee.LastName) )
            {
                //ms.Employee.SaveEmployee(employee);
                return JsonConvert.SerializeObject(Ok("пользователь добавлен"), Formatting.Indented);
            }
            return JsonConvert.SerializeObject(BadRequest("ошибка при добавлении пользователя"), Formatting.Indented);
        }
    }
}
