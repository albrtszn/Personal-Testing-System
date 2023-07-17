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
            return Ok(ms.Subdivision.GetAllSubdivisionDtos());
        }

        [HttpPost("AddSubdivision")]
        public async Task<IActionResult> AddSubdivision(SubdivisionDto subdivisionDto)
        {
            if (!subdivisionDto.Name.IsNullOrEmpty())
            {
                ms.Subdivision.SaveSubdivisionDto(subdivisionDto);
                Ok(new { message = "Отдел добавлен" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            logger.LogInformation($"/admin-api/GetEmployees ");
            return Ok(ms.Employee.GetAllEmployeeModels());
        }

        [HttpGet("GetEmployee")]
        public async Task<IActionResult> GetEmployee(string? id)
        {
            logger.LogInformation($"/admin-api/GetEmployee :id={id}");
            EmployeeDto? dto = ms.Employee.GetEmployeeDtoById(id);
            if (!id.IsNullOrEmpty() && dto != null)
            {
                return Ok(dto);
            }
            return NotFound("Сотрудник не найден");
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto? employee)
        {
            logger.LogInformation($"/admin-api/AddEmployee :fn={employee.FirstName}, sn={employee.SecondName}, " +
                                  $" ln={employee.LastName}, idSubdivision={employee.IdSubdivision}");
            if (employee != null && !string.IsNullOrEmpty(employee.FirstName) && !
                string.IsNullOrEmpty(employee.SecondName) && !string.IsNullOrEmpty(employee.LastName) &&
                !string.IsNullOrEmpty(employee.Login) && !string.IsNullOrEmpty(employee.Password) &&
                employee.IdSubdivision.HasValue)
            {
                ms.Employee.SaveEmployee(employee);
                return Ok("Сотрудник добавлен");
            }
            return BadRequest("Ошибка. Не все поля заполнены");
        }

        [HttpGet("GetCompetences")]
        public async Task<IActionResult> GetCompetences()
        {
            return Ok(ms.TestType.GetAllCompetenceDtos());
        }

        [HttpGet("GetCompetence")]
        public async Task<IActionResult> GetCompetence(int? id)
        {
            if (id.HasValue)
            {
                return Ok(ms.TestType.GetCompetenceDtoById(id.Value));
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("AddCompetence")]
        public async Task<IActionResult> AddCompetence(CompetenceDto? competenceDto)
        {
            logger.LogInformation($"/admin-api/AddCompetence :id={competenceDto.Id}, Name={competenceDto.Name}");
            if (competenceDto.Id.HasValue || !string.IsNullOrEmpty(competenceDto.Name))
            {
                ms.TestType.SaveCompetenceDto(competenceDto);
                return Ok(new { message = "Компетенция добавлена" });
            }
            return BadRequest(new { message = "Ошиббка при добавлении компетенции" });
        }

        [HttpGet("GetTest")]
        public async Task<IActionResult> GetTest(string? id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Test test = ms.Test.GetTestById(id);
                List<Question> questions = ms.Question.GetAllQuestions()
                    .Where(x => x.IdTest.Equals(id)).ToList();

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
        public async Task<IActionResult> AddTest(CreateTestModel? createTestDto)
        {
            logger.LogInformation($"/user-api/AddTest test: name={createTestDto.Name}, idCompetence={createTestDto.CompetenceId}," +
                                  $"countOfQuestions={createTestDto.Questions.Count}");

            if (createTestDto != null && !createTestDto.Name.IsNullOrEmpty() &&
                !createTestDto.Name.IsNullOrEmpty() && createTestDto.CompetenceId.HasValue &&
                createTestDto.Questions.Count != 0) {
                string idTest = Guid.NewGuid().ToString();
                ms.Test.SaveTest(new Test
                {
                    Id = idTest,
                    Name = createTestDto.Name,
                    IdCompetence = createTestDto.CompetenceId
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
                return Ok(new { message = "Добавление теста успешно" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpGet("GetPurposes")]
        public async Task<IActionResult> GetPurposes()
        {
            logger.LogInformation($"/user-api/GetPurposess ");
            return Ok(ms.TestPurpose.GetAllPurposeAdminModels());
        }

        [HttpGet("GetPurposesByEmployeeId")]
        public async Task<IActionResult> GetPurposesByEmployeeId(string employeeId)
        {
            logger.LogInformation($"/admin-api/GetPurposessByEmployeeId id={employeeId}");
            if (string.IsNullOrEmpty(employeeId))
            {
                return BadRequest(new { message = "Вы ввели пустое поле" });
            }
            List<TestPurposeDto> purposes = ms.TestPurpose.GetAllTestPurposeDtos()
                             .Where(x => x.IdEmployee.Equals(employeeId)).ToList();

            if (purposes.Count == 0)
            {
                return BadRequest(new { message = "Нет назначенных тестов" });
            }
            else
            {
                List<PurposeModel> models = new List<PurposeModel>();
                foreach (TestPurposeDto purpose in purposes)
                {
                    PurposeModel model = new PurposeModel
                    {
                        Id = purpose.Id,
                        IdEmployee = employeeId,
                        Test = ms.Test.GetTestGetModelById(purpose.IdTest),
                        DatatimePurpose = purpose.DatatimePurpose
                    };

                    models.Add(model);
                }
                return Ok(models);
            }
            return BadRequest(new { message = "Ошибка при запросе" });
        }

        [HttpPost("AddPurpose")]
        public async Task<IActionResult> AddPurpose(TestPurposeDto? purpose)
        {
            logger.LogInformation($"/admin-api/AddPurpose employeeId={purpose.IdEmployee}, testId={purpose.IdTest}, datatime={purpose.DatatimePurpose}");
            if (purpose != null && !purpose.IdTest.IsNullOrEmpty() &&
                !purpose.IdEmployee.IsNullOrEmpty() && !purpose.DatatimePurpose.IsNullOrEmpty())
            {
                ms.TestPurpose.SaveTestPurposeDto(purpose);
                return Ok(new { message = "Тест назначен" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("AddPurposesBySubdivision")]
        public async Task<IActionResult> AddPurposesBySubdivision(string? testId, int? idSubdivision, DateTime? time)
        {
            logger.LogInformation($"/admin-api/AddPurposesBySubdivision testId={testId}, idSubdivision={idSubdivision}");
            if (!testId.IsNullOrEmpty() && idSubdivision.HasValue && time.HasValue)
            {
                ms.TestPurpose.SavePurposeBySubdivisionId(testId, idSubdivision.Value, time.Value);
                return Ok(new { message = "Тестs назначенs" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("AddPurposesByRange")]
        public async Task<IActionResult> AddPurposesByRange(PurposeRangeModel? purpose)//!!!
        {
            logger.LogInformation($"/admin-api/AddPurposesByRange testId={purpose.IdTest}, datatime={purpose.DatatimePurpose}, idEmployeesCount={purpose.IdEmployees.Count}");
            if (!purpose.IdTest.IsNullOrEmpty() && !purpose.DatatimePurpose.IsNullOrEmpty() &&
                purpose.IdEmployees.Count != 0)
            {
                foreach (string id in purpose.IdEmployees)
                {
                    ms.TestPurpose.SaveTestPurpose(new TestPurpose
                    {
                        IdTest = purpose.IdTest,
                        IdEmployee = id,
                        DatatimePurpose = DateTime.Parse(purpose.DatatimePurpose)
                    });
                }
                return Ok(new { message = "Тестs назначенs" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpGet("GetResults")]
        public async Task<IActionResult> GetResults()
        {
            return Ok(ms.EmployeeResult.GetAllEmployeeResultModels());
        }

        [HttpGet("GetResults")]
        public async Task<IActionResult> GetResults(int? idSubdivision, string? FirstName,
                                                    string? SecondName, string LastName,
                                                    int? score, string? sortType)
        {
            List<EmployeeResultModel> list = ms.EmployeeResult.GetAllEmployeeResultModels();
            if (idSubdivision.HasValue)
            {
                list = list.Where(x => x.Employee.Subdivision.Id == idSubdivision).ToList();
            }
            if (!FirstName.IsNullOrEmpty())
            {
                list = list.Where(x => x.Employee.FirstName.Contains(FirstName)).ToList();
            }
            if (!SecondName.IsNullOrEmpty())
            {
                list = list.Where(x => x.Employee.SecondName.Contains(SecondName)).ToList();
            }
            if (!LastName.IsNullOrEmpty())
            {
                list = list.Where(x => x.Employee.LastName.Contains(LastName)).ToList();
            }
            if (!sortType.IsNullOrEmpty())
            {
                if (sortType.Equals("fname↑"))//↑
                {
                    list = list.OrderBy(x => x.Employee.FirstName).ToList();
                }
                if (sortType.Equals("fname↓"))//↓
                {
                    list = list.OrderByDescending(x => x.Employee.FirstName).ToList();
                }

                if (sortType.Equals("sname↑"))//↑
                {
                    list = list.OrderBy(x => x.Employee.SecondName).ToList();
                }
                if (sortType.Equals("sname↓"))//↓
                {
                    list = list.OrderByDescending(x => x.Employee.SecondName).ToList();
                }

                if (sortType.Equals("lname↑"))//↑
                {
                    list = list.OrderBy(x => x.Employee.LastName).ToList();
                }
                if (sortType.Equals("lname↓"))//↓
                {
                    list = list.OrderByDescending(x => x.Employee.LastName).ToList();
                }

                if (sortType.Equals("score↑"))//↑
                {
                    list = list.OrderBy(x => x.Result.ScoreFrom).ToList();
                }
                if (sortType.Equals("score↓"))//↓
                {
                    list = list.OrderByDescending(x => x.Result.ScoreFrom).ToList();
                }
            }
            return Ok(list);
        }

        [HttpPost("DeleteResults")]
        public async Task<IActionResult> DeleteResults()
        {
            List<EmployeeResultDto> employeeResults = ms.EmployeeResult.GetAllEmployeeResultDtos();
            foreach (EmployeeResultDto result in employeeResults)
            {
                ms.EmployeeResult.DeleteEmployeeResultById(result.Id.Value);
                ms.Result.DeleteResultById(result.IdResult);
                ms.EmployeeAnswer.DeleteEmployeeAnswersByResult(result.IdResult);

                ms.Result.DeleteResultById(result.IdResult);//EmployeeMatching
                ms.Result.DeleteResultById(result.IdResult);//EmployeeSubsequence

            }
            return Ok(new { message = "Результаты удалены" });
        }

    }
}
