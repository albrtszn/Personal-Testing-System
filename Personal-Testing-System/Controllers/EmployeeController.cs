using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Personal_Testing_System.Services;
using Newtonsoft.Json;
using Personal_Testing_System.DTOs;
using DataBase.Repository.Models;
using System.Text.Json;

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
        public IActionResult test()
        {
            return Ok("Personal-Testing-System " + DateTime.Now);
        }

        [HttpGet("GetEmployee")]
        public IActionResult GetEmployee(int id)
        {
            logger.LogInformation($"/user-api/GetEmployee :id={id}");
            if (ms.Employee.GetEmployeeById(id) != null)
            {
                //return JsonConvert.SerializeObject(ms.Employee.GetEmployeeById(id), Formatting.Indented);
                //return Results.Json(ms.Employee.GetEmployeeById(id), new(System.Text.Json.JsonSerializerDefaults.General));
                return Ok(ms.Employee.GetEmployeeById(id));
            }
            //return JsonConvert.SerializeObject(NotFound(), Formatting.Indented);
            //return Results.Json(NotFound(), new(System.Text.Json.JsonSerializerDefaults.General));
            return NotFound("Сотрудник не найден");
        }
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            logger.LogInformation($"/user-api-AddEmployee :fn={employee.FirstName}, sn={employee.SecondName}, " +
                                  $" ln={employee.LastName}, idSubdivision={employee.IdSubdivision}");
            if (!string.IsNullOrEmpty(employee.FirstName) && !string.IsNullOrEmpty(employee.SecondName) &&
                !string.IsNullOrEmpty(employee.LastName))
            {
                //ms.Employee.SaveEmployee(employee);
                //return JsonConvert.SerializeObject(Ok("пользователь добавлен"), Formatting.Indented);
                return Ok("Сотрудник добавлен");
            }
            //return JsonConvert.SerializeObject(BadRequest("ошибка при добавлении пользователя"), Formatting.Indented);
            return BadRequest("Сотрудник не добавлен");
        }

        [HttpGet("GetSubdivisions")]
        public IActionResult GetSubdivisions()
        {
            //return JsonConvert.SerializeObject(ms.Subdivision.GetAllSubdivisions(), Formatting.Indented);
            return Ok(ms.Subdivision.GetAllSubdivisions());
        }

        [HttpPost("AddTestType")]
        public IActionResult AddTestType(TestTypeDto testTypeDto)
        {
            logger.LogInformation($"/user-api/AddTestType testType: name={testTypeDto.Name}");
            ms.TestType.SaveCompetence(new Competence
            {
                Name = testTypeDto.Name
            });
            //return JsonConvert.SerializeObject(Ok(), Formatting.Indented);
            return Ok("Тип теста добавлен");
        }

        [HttpGet("GetTestByEmployee")]
        public IActionResult GetTestByEmployee(int id)
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
        public IActionResult GetTest(string id)
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

        [HttpPost("AddTest")]
        public IActionResult AddTest([FromBody] CreateTestDto createTestDto)
        {
            logger.LogInformation($"/user-api/AddTest test: name={createTestDto.Name}, idType={createTestDto.IdCompetence}," +
                                  $"countOfQuestions={createTestDto.Questions.Count}");

            string idTest = Guid.NewGuid().ToString();
            ms.Test.SaveTest(new Test 
            {
                Id = idTest,
                Name = createTestDto.Name,
                IdCompetence = createTestDto.IdCompetence
            });

            foreach (CreateQuestionDto quest in createTestDto.Questions)
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

                    if (answerDto is AnswerDto && answerDto.Correct!=null)
                    {
                        logger.LogInformation($"answerDto -> text={answerDto.Text}, correct={answerDto.Correct}");
                        ms.Answer.SaveAnswer(new Answer
                        {
                            Text = answerDto.Text,
                            IdQuestion = idQuestion,
                            Correct = answerDto.Correct
                        });
                    }
                    if (subsequenceDto is SubsequenceDto && subsequenceDto.Number != null && subsequenceDto.Number!=0)
                    {
                        logger.LogInformation($"subsequenceDto -> text={subsequenceDto.Text}, number={subsequenceDto.Number}");
                        ms.Subsequence.SaveSubsequence(new Subsequence
                        {
                            Text= subsequenceDto.Text,
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
                            IdQuestion= idQuestion
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
    }
}
