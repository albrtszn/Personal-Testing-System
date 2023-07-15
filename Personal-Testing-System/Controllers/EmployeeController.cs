using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Personal_Testing_System.Services;
using Newtonsoft.Json;
using Personal_Testing_System.DTOs;
using DataBase.Repository.Models;
using System.Text.Json;
using Personal_Testing_System.Models;
using Microsoft.IdentityModel.Tokens;

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
            return Ok(new { message = "Personal-Testing-System " + DateTime.Now });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            logger.LogInformation($"/user-api/Login : login={loginModel.Login}, Password={loginModel.Password} ");
            if (string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new { message = "Одно из полей пустое" });
            }

            EmployeeDto? employeeDto = ms.Employee.GetAllEmployeeDtos()
                                  .Find(x => x.Login.Equals(loginModel.Login));

            if (employeeDto == null)
            {
                return BadRequest(new { message = "Пользователь не найден" });
            }
            else
            {
                if (loginModel.Password.Equals(employeeDto.Password))
                {
                    return Ok(new
                    {
                        EmployeeId = employeeDto.Id
                    });
                }
                else
                {
                    return BadRequest(new { message = "Пароль не совпадает" });
                }
            }
        }

        [HttpGet("GetPurposesByEmployeeId")]
        public async Task<IActionResult> GetPurposesByEmployeeId(string? employeeId)
        {
            logger.LogInformation($"/user-api/GetPurposess emmployeeId={employeeId}");
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

        [HttpGet("GetTest")]
        public async Task<IActionResult> GetTest(string? id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Test test = ms.Test.GetTestById(id);
                if (test!=null) {
                    List<Question> questions = ms.Question.GetAllQuestions()
                        .Where(x => x.IdTest.Equals(id)).ToList();

                    //.Where(a => a.IdQuestion.Contains(questions.Id)).ToList();
                    //.Where(x => questions.ForEach(a=>a.Id.Equals(x.IdQuestion)));

                    TestModel testDto = new TestModel
                    {
                        Id = test.Id,
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
            }
            else
            {
                return BadRequest(new { message = "Вы не ввели требуемое поле" });
            }
            return NotFound(new { message = "Тест не найден" });
        }

        [HttpPost("PushTest")]
        public async Task<IActionResult> PushTest(TestResultModel testResultModel)
        {
            logger.LogInformation($"/user-api/PushTest testId={testResultModel.TestId} emmployeeId={testResultModel.EmployeeId}");

            if (!testResultModel.TestId.IsNullOrEmpty() && !testResultModel.EmployeeId.IsNullOrEmpty() &&
                !testResultModel.StartDate.IsNullOrEmpty() && !testResultModel.StartTime.IsNullOrEmpty() &&
                !testResultModel.EndTime.IsNullOrEmpty() && testResultModel.Questions.Count != 0)
            {
                string resultId = Guid.NewGuid().ToString();
                /*ms.Result.SaveResult(new Result
                {
                    Id = resultId,
                    IdTest = testResultModel.TestId,
                    StartDate = DateOnly.Parse(testResultModel.StartDate),
                    StartTime = TimeOnly.Parse(testResultModel.StartTime),
                    EndTime = TimeOnly.Parse(testResultModel.EndTime),
                    Duration = (byte)(TimeOnly.Parse(testResultModel.EndTime).Minute - TimeOnly.Parse(testResultModel.StartTime).Minute),
                });*/

                int score = 0;
                foreach (QuestionResultModel question in testResultModel.Questions)
                {
                    int countOfAnswers = 0;
                    int countOfCorrectAnswer = 0;

                    foreach (JsonElement answer in question.Answers)
                    {
                        countOfAnswers++;

                        AnswerResultModel answerModel = answer.Deserialize<AnswerResultModel>();
                        SubsequenceResultModel subsequenceModel = answer.Deserialize<SubsequenceResultModel>();
                        FSPartResultModel fsPartModel = answer.Deserialize<FSPartResultModel>();

                        if (answerModel != null && answerModel.AnswerId.HasValue)
                        {
                            logger.LogInformation($"answerModel -> text={answerModel.AnswerId}");

                            if (ms.Answer.GetAnswerById(answerModel.AnswerId.Value).Correct.Value)
                            {
                                countOfCorrectAnswer++;
                            }

                            /*ms.EmployeeAnswer.SaveEmployeeAnswer(new EmployeeAnswer
                            {
                                IdResult = resultId,
                                IdAnswer = answerModel.AnswerId
                            });*/
                        }
                        if (subsequenceModel != null && subsequenceModel.SubsequenceId.HasValue)
                        {
                            logger.LogInformation($"subsequenceModel -> id={subsequenceModel.SubsequenceId}, number={subsequenceModel.Number}");

                            if (ms.Subsequence.GetSubsequenceById(subsequenceModel.SubsequenceId.Value).Number == (subsequenceModel.Number.Value))
                            {
                                countOfCorrectAnswer++;
                            }

                            /*ms.EmployeeSubsequence.SaveEmployeeSubsequence(new EmployeeSubsequence
                            {
                                IdSubsequence = subsequenceModel.Id.Value,
                                IdResult = resultId,
                                Number = subsequenceModel.Number.Value,
                            });*/
                        }
                        if (!string.IsNullOrEmpty(fsPartModel.FirstPartId) && fsPartModel.SecondPartId.HasValue && fsPartModel != null)
                        {
                            logger.LogInformation($"fsPartModel -> first={fsPartModel.FirstPartId}, second={fsPartModel.SecondPartId}");

                            if (ms.SecondPart.GetSecondPartById(fsPartModel.SecondPartId.Value).IdFirstPart.Equals(fsPartModel.FirstPartId))
                            {
                                countOfCorrectAnswer++;
                            }

                            /*ms.EmployeeMatching.SaveEmployeeMatching(new EmployeeMatching
                            {
                                IdFirstPart = fsPartModel.FirstPartId,
                                IdSecondPart = fsPartModel.SecondPartId,
                                IdResult = resultId
                            });*/
                        }
                    }
                    if (countOfAnswers == countOfCorrectAnswer)
                    {
                        score++;
                    }
                }
                /*ms.EmployeeResult.SaveEmployeeResult(new EmployeeResult
                {
                    IdResult = resultId,
                    IdEmployee = testResultModel.EmployeeId,
                    ScoreFrom = score, //???
                    ScoreTo = score
                });*/

                return Ok(new { message = "Тест выполнен", score = score });
            }
            else
            {
                return BadRequest(new { message = "Ошибка при добавления теста" });
            }
        }
    }
}
