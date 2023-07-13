using DataBase.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;
using Personal_Testing_System.Services;
using System.Text.Json;

namespace Personal_Testing_System.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("admin-api")]

    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> logger;
        private MasterService ms;
        public AdminController(ILogger<AdminController> _logger, MasterService _masterService)
        {
            logger = _logger;
            ms = _masterService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!string.IsNullOrEmpty(loginModel.Login) || !string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new { message = "Одно из полей пустое" });
            }

            AdminDto? adminDto = ms.Admin.GetAllAdminDtos()
                                  .Find(x => x.Login.Equals(loginModel.Login));

            if (adminDto == null)
            {
                return BadRequest(new { message = "Администратор не найден" });
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
                        AdminId = adminDto.Id
                    });
                }
            }
        }

        [HttpGet("GetSubdivisions")]
        public async Task<IActionResult> GetSubdivisions()
        {
            return Ok(ms.Subdivision.GetAllSubdivisions());
        }

        [HttpPost("AddSubdivisions")]
        public async Task<IActionResult> AddGetSubdivisions(SubdivisionDto subdivisionDto)
        {
            ms.Subdivision.SaveSubdivisionDto(subdivisionDto);
            return Ok();
        }

        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            logger.LogInformation($"/admin-api/GetEmployees ");
            return Ok(ms.Employee.GetAllEmployeeDtos());
        }

        [HttpGet("GetEmployee")]
        public async Task<IActionResult> GetEmployee(string? id)
        {
            logger.LogInformation($"/admin-api/GetEmployee :id={id}");
            if (!id.IsNullOrEmpty() || ms.Employee.GetEmployeeById(id) != null)
            {
                return Ok(ms.Employee.GetEmployeeDtoById(id));
            }
            return NotFound("Сотрудник не найден");
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employee)
        {
            logger.LogInformation($"/admin-api/AddEmployee :fn={employee.FirstName}, sn={employee.SecondName}, " +
                                  $" ln={employee.LastName}, idSubdivision={employee.IdSubdivision}");
            if (!string.IsNullOrEmpty(employee.FirstName) && !string.IsNullOrEmpty(employee.SecondName) &&
                !string.IsNullOrEmpty(employee.LastName) && !string.IsNullOrEmpty(employee.Login) &&
                !string.IsNullOrEmpty(employee.Password) && employee.IdSubdivision.HasValue)
            {
                ms.Employee.SaveEmployee(employee);
                return Ok("Сотрудник добавлен");
            }
            return BadRequest("Сотрудник не добавлен");
        }

        [HttpPost("AddCompetence")]
        public async Task<IActionResult> AddCompetence(CompetenceDto competenceDto)
        {
            logger.LogInformation($"/admin-api/AddCompetence :id={competenceDto.Id}, Name={competenceDto.Name}");
            if (!competenceDto.Id.HasValue || string.IsNullOrEmpty(competenceDto.Name))
            {
                ms.TestType.SaveCompetenceDto(competenceDto);
                return Ok(new { message = "Компетенция добавлена"});
            }
            return BadRequest(new { message = "Ошиббка при добавлении компетенции" });
        }

        [HttpPost("AddTestType")]
        public async Task<IActionResult> AddTestType(CompetenceDto testTypeDto)
        {
            logger.LogInformation($"/user-api/AddTestType testType: name={testTypeDto.Name}");
            ms.TestType.SaveCompetence(new Competence
            {
                Name = testTypeDto.Name
            });
            //return JsonConvert.SerializeObject(Ok(), Formatting.Indented);
            return Ok("Тип теста добавлен");
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

                TestModel testDto = new TestModel
                {
                    Name = test.Name,
                    Competence = ms.TestType.GetCompetenceDtoById(test.IdCompetence.Value),
                    Questions = new List<QuestionModel>()
                };

                foreach (var quest in questions)
                {
                    QuestionModel createQuestionDto = new QuestionModel
                    {
                        Id = quest.Id,
                        IdQuestionType = quest.IdQuestionType,
                        Text = quest.Text,
                        Answers = new List<object>() { }
                    };

                    if (ms.Answer.GetAnswerDtosByQuestionId(quest.Id).Count != 0)
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

        [HttpPost("AddTest")]
        public async Task<IActionResult> AddTest([FromBody] TestModel createTestDto)
        {
            logger.LogInformation($"/user-api/AddTest test: name={createTestDto.Name}, idCompetence={createTestDto.Competence.Id}," +
                                  $"countOfQuestions={createTestDto.Questions.Count}");

            string idTest = Guid.NewGuid().ToString();
            ms.Test.SaveTest(new Test
            {
                Id = idTest,
                Name = createTestDto.Name,
                IdCompetence = createTestDto.Competence.Id
            });

            foreach (QuestionModel quest in createTestDto.Questions)
            {
                logger.LogInformation($"quest -> text={quest.Text} idType={quest.IdQuestionType} count={quest.Answers.Count}");

                string idQuestion = Guid.NewGuid().ToString();
                ms.Question.SaveQuestion(new Question
                {
                    Id = idQuestion,
                    Text = quest.Text,
                    IdQuestionType = quest.IdQuestionType,
                    IdTest = idTest
                });

                foreach (JsonElement answer in quest.Answers)
                {
                    //JsonConvert.DeserializeObject<AnswerDto>(answer.);
                    //answer.Deserialize<AnswerDto>();
                    //answerDto.Correct = answer.GetProperty("");
                    //AnswerDto answerDto = JsonConvert.DeserializeObject<AnswerDto>(answer.ToString());

                    AnswerDto answerDto = answer.Deserialize<AnswerDto>();
                    SubsequenceDto subsequenceDto = answer.Deserialize<SubsequenceDto>();
                    FirstSecondPartDto firstSecondPartDto = answer.Deserialize<FirstSecondPartDto>();

                    if (answerDto is AnswerDto && answerDto.Correct != null)
                    {
                        logger.LogInformation($"answerDto -> text={answerDto.Text}, correct={answerDto.Correct}");
                        ms.Answer.SaveAnswer(new Answer
                        {
                            Text = answerDto.Text,
                            IdQuestion = idQuestion,
                            Correct = answerDto.Correct
                        });
                    }
                    if (subsequenceDto is SubsequenceDto && subsequenceDto.Number != null && subsequenceDto.Number != 0)
                    {
                        logger.LogInformation($"subsequenceDto -> text={subsequenceDto.Text}, number={subsequenceDto.Number}");
                        ms.Subsequence.SaveSubsequence(new Subsequence
                        {
                            Text = subsequenceDto.Text,
                            Number = subsequenceDto.Number,
                            IdQuestion = idQuestion
                        });
                    }
                    if (firstSecondPartDto is FirstSecondPartDto &&
                        !string.IsNullOrEmpty(firstSecondPartDto.FirstPartText) && !string.IsNullOrEmpty(firstSecondPartDto.SecondPartText))
                    {
                        logger.LogInformation($"firstSecondPartDto -> first={firstSecondPartDto.FirstPartText}, second={firstSecondPartDto.SecondPartText}");
                        string firstPartId = Guid.NewGuid().ToString();
                        ms.FirstPart.SaveFirstPart(new FirstPart
                        {
                            Id = firstPartId,
                            Text = firstSecondPartDto.FirstPartText,
                            IdQuestion = idQuestion
                        });
                        ms.SecondPart.SaveSecondPart(new SecondPart
                        {
                            Text = firstSecondPartDto.SecondPartText,
                            IdFirstPart = firstPartId
                        });
                    }
                }
            }
            return Ok("Добавление теста успешно");
            //return JsonConvert.SerializeObject(Ok(), Formatting.Indented);
            //return BadRequest("Ошибка при добавлении теста");
        }

        [HttpGet("GetPurposes")]
        public async Task<IActionResult> GetPurposes()
        {
            logger.LogInformation($"/user-api/GetPurposess ");
            return Ok(ms.TestPurpose.GetAllTestPurposes());
        }

        [HttpGet("GetPurposesByEmployeeId")]
        public async Task<IActionResult> GetPurposesByEmployeeId(string employeeId)
        {
            logger.LogInformation($"/user-api/GetPurposess ");
            return Ok(ms.TestPurpose.GetAllTestPurposes());
        }

        [HttpPost("AddPurpose")]
        public async Task<IActionResult> AddPurpose(TestPurpose purpose)
        {
            logger.LogInformation($"/user-api/AddPurpose ");
            return Ok(ms.Employee.GetAllEmployeeDtos());
        }

    }
}
