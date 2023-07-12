using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Personal_Testing_System.Services;
using Newtonsoft.Json;
using Personal_Testing_System.DTOs;
using DataBase.Repository.Models;
using System.Text.Json;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Controllers
{
    [ApiController]
    [Route("user-api")]
    //[controller]")]
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
        public IActionResult test()
        {
            return Ok("Personal-Testing-System " + DateTime.Now);
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!string.IsNullOrEmpty(loginModel.Login) || !string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new  { message = "Одно из полей пустое" });
            }

            EmployeeDto? employeeDto = ms.Employee.GetAllEmployeeDtos()
                                  .Find(x => x.Login.Equals(loginModel.Login));

            if (employeeDto == null)
            {
                return BadRequest(new { message = "Пользователь не найден" });
            }
            else
            {
                if (!loginModel.Password.Equals(loginModel.Password))
                {
                    return BadRequest(new { message = "Пароль не совпадает" });
                }
                else
                {
                    return Ok(new
                    {
                        EmployeeId = employeeDto.Id
                    });
                }
            }
        }

        [HttpGet("GetTestByEmployee")]
        public async Task<IActionResult> GetTestByEmployee(int id)
        {
            if (ms.Employee.GetEmployeeById(id) != null)
            {
                //ms.Employee.SaveEmployee(employee);
                string testId = ms.TestPurpose.GetAllTestPurposes().Find(x => x.IdEmployee == id).IdTest;
                if (testId != null)
                {
                    //return JsonConvert.SerializeObject(ms.Test.GetTestById(testId), Formatting.Indented);
                    return Ok(ms.Test.GetTestById(testId));
                }
            }
            //return JsonConvert.SerializeObject(BadRequest("ошибка при поиске пользователя пользователя"), Formatting.Indented);
            return NotFound("Тест не найден");
        }

        [HttpGet("GetTest")]
        public async Task<IActionResult> GetTest(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Test test = ms.Test.GetTestById(id);
                List<Question> questions = ms.Question.GetAllQuestions()
                    .Where(x => x.IdTest.Equals(id)).ToList();

                //.Where(a => a.IdQuestion.Contains(questions.Id)).ToList();
                //.Where(x => questions.ForEach(a=>a.Id.Equals(x.IdQuestion)));

                CreateTestDto testDto = new CreateTestDto
                {
                    Name = test.Name,
                    IdCompetence = test.IdCompetence,
                    Questions = new List<CreateQuestionDto>()
                };

                foreach (var quest in questions)
                {
                    CreateQuestionDto createQuestionDto = new CreateQuestionDto
                    {
                        Id = quest.Id,
                        IdQuestionType = quest.IdQuestionType,
                        Text = quest.Text,
                        Answers = new List<object>(){}
                    };

                    if (ms.Answer.GetAnswerDtosByQuestionId(quest.Id).Count!=0)
                    {
                        createQuestionDto.Answers.AddRange(ms.Answer.GetAnswerDtosByQuestionId(quest.Id));
                    }
                    if (ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id).Count != 0)
                    {
                        createQuestionDto.Answers.AddRange(ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id));
                    }
                    if (ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id).Count != 0)
                    {
                        createQuestionDto.Answers.AddRange(ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id));

                        List<SecondPartDto> secondPartDtos = new List<SecondPartDto>();
                        foreach (var firstPart in ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id))
                        {
                            secondPartDtos.Add(ms.SecondPart.GetSecondPartDtoByFirstPartId(firstPart.Id));
                        }
                        createQuestionDto.Answers.AddRange(secondPartDtos);
                    }

                    testDto.Questions.Add(createQuestionDto);  
                }
                return Ok(testDto);
            }
            return NotFound("Тест не найден");
        }

        [HttpGet("GetPurposesByEmployeeId")]
        public async Task<IActionResult> GetPurposesByEmployeeId(string employeeId)
        {
            logger.LogInformation($"/user-api/GetPurposess ");
            return Ok(ms.TestPurpose.GetAllTestPurposes());
        }

    }
}
