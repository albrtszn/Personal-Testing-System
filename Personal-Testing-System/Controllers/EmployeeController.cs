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
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System;
using DataBase.Repository;
using DataBase;

namespace Personal_Testing_System.Controllers
{
    [ApiController]
    [Route("user-api")]
    //[controller]")]
    //[Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly IWebHostEnvironment environment;
        private MasterService ms;
        public EmployeeController(ILogger<EmployeeController> _logger, MasterService _masterService,
                                  IWebHostEnvironment environment)//, EFDbContext db)
        {
            logger = _logger;
            ms = _masterService;
            this.environment = environment;
            //InitDB.InitData(db);
        }

        [HttpGet("Ping")]
        public async Task<IActionResult> Ping()
        {
            return Ok(new { message = $"Ping: {HttpContext.Request.Host + HttpContext.Request.Path} {DateTime.Now}." });
        }

        [HttpGet("TestGetEmployees")]
        public async Task<IActionResult> TestGetEmployees()
        {
            return Ok(await ms.Employee.GetAllEmployeeDtos());
        }

        [HttpDelete("DeleteEmployeeTokens")]
        public async Task<IActionResult> DeleteEmployeeTokens()
        {
            (await ms.TokenEmployee.GetAllTokenEmployees()).ForEach(async x => await ms.TokenEmployee.DeleteTokenEmployeeById(x.Id));
            if (!(await ms.TokenEmployee.GetAllTokenEmployees()).Any())
            {
                return Ok(new { messsage = "Токены удалены" });
            }
            return BadRequest(new { messsage = "Ошибка при удалении токенов" });
        }

        [HttpGet("TestGetEmployeeTokens")]
        public async Task<IActionResult> TestGetEmployeeTokens()
        {
            return Ok(ms.TokenEmployee.GetAllTokenEmployees());
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel? loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new { message = "Одно из полей пустое" });
            }
            logger.LogInformation($"/user-api/Login : login={loginModel.Login}, Password={loginModel.Password} ");
            await ms.Log.SaveLog(new Log
            {
                UrlPath = "user-api/Login",
                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                DataTime = DateTime.Now,
                Params = $"логин={loginModel.Login}, пароль={loginModel.Password}"
            });

            EmployeeDto? employeeDto = (await ms.Employee.GetAllEmployeeDtos())
                                  .Find(x => x.Login.Equals(loginModel.Login));

            if (employeeDto == null)
            {
                return BadRequest(new { message = "Пользователь не найден" });
            }
            else
            {
                if (loginModel.Password.Equals(employeeDto.Password))
                {
                    TokenEmployee? tokenEmployee = await ms.TokenEmployee.GetTokenEmployeeByEmployeeId(employeeDto.Id);
                    if (tokenEmployee != null && !await ms.IsTokenEmployeeExpired(tokenEmployee))
                    {
                        return Ok(new
                        {
                            TokenEmployee = tokenEmployee.Token,
                            Employee = employeeDto
                        });
                    }
                    else
                    {
                        string token = Guid.NewGuid().ToString();
                        await ms.TokenEmployee.SaveTokenEmployee(new TokenEmployee
                        {
                            IdEmployee = employeeDto.Id,
                            Token = token,
                            IssuingTime = DateTime.Now,
                            State = true
                        });
                        return Ok(new
                        {
                            TokenEmployee = token,
                            Employee = employeeDto
                        });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Пароль не совпадает" });
                }
            }
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenEmployee? token = await ms.TokenEmployee.GetTokenEmployeeByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenEmployeeExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/user-api/LogOut : AuthHeader={Authorization}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "user-api/LoOut",
                        UserId = token.IdEmployee,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"Id сотрудника={token.IdEmployee}"
                    });
                    await ms.TokenEmployee.DeleteTokenEmployeeById(token.Id);
                    return Ok(new { message = "Выполнен выход из системы" });
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заолнены" });
        }

        [HttpPost("GetPurposesByEmployeeId")]
        public async Task<IActionResult> GetPurposesByEmployeeId([FromHeader] string? Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenEmployee? token = await ms.TokenEmployee.GetTokenEmployeeByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenEmployeeExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }

                    List<TestPurposeDto> purposes = (await ms.TestPurpose.GetAllTestPurposeDtos())
                                     .Where(x => x.IdEmployee.Equals(id.Id)).ToList();

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
                                IdEmployee = id.Id,
                                Test = await ms.Test.GetTestGetModelById(purpose.IdTest),
                                DatatimePurpose = purpose.DatatimePurpose
                            };

                            models.Add(model);
                        }
                        return Ok(models);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заолнены" });
        }

        [HttpPost("GetTest")]
        public async Task<IActionResult> GetTest([FromHeader] string? Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenEmployee? token = await ms.TokenEmployee.GetTokenEmployeeByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenEmployeeExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/user-api/GetTest testId={id.Id}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "user-api/GetTest",
                        UserId = $"{token.IdEmployee}",
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"TestId={id.Id}"
                    });

                    Test? test = await ms.Test.GetTestById(id.Id);
                    if (test != null)
                    {
                        List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);

                        TestModel testDto = new TestModel
                        {
                            Id = test.Id,
                            Name = test.Name,
                            Weight = test.Weight,
                            Description = test.Description,
                            Instruction = test.Instruction,
                            Competence = await ms.TestType.GetCompetenceDtoById(test.IdCompetence.Value),
                            Questions = new List<QuestionModel>()
                        };

                        foreach (var quest in questions)
                        {
                            QuestionModel createQuestionDto = new QuestionModel
                            {
                                Id = quest.Id,
                                IdQuestionType = quest.IdQuestionType,
                                Text = quest.Text,
                                Number = Convert.ToInt32(quest.Number),
                                Answers = new List<object>() { }
                            };
                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath))
                                {
                                    byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath);
                                    string base64 = Convert.ToBase64String(array);
                                    //createQuestionDto.Base64Image = base64;
                                    createQuestionDto.ImagePath = quest.ImagePath;
                                }
                            }

                            if ((await ms.Answer.GetAnswerDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                //createQuestionDto.Answers.AddRange(ms.Answer.GetAnswerDtosByQuestionId(quest.Id));
                                foreach (AnswerDto answerDto in await ms.Answer.GetAnswerDtosByQuestionId(quest.Id))
                                {
                                    AnswerModel model = new AnswerModel
                                    {
                                        IdAnswer = answerDto.IdAnswer,
                                        Text = answerDto.Text,
                                        Number = answerDto.Number,
                                        IdQuestion = answerDto.IdQuestion,
                                        Correct = answerDto.Correct,
                                        Weight = answerDto.Weight
                                    };
                                    if (!answerDto.ImagePath.IsNullOrEmpty())
                                    {
                                        if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("/images/" + answerDto.ImagePath).PhysicalPath))
                                        {
                                            byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("/images/" + answerDto.ImagePath).PhysicalPath);
                                            string base64 = Convert.ToBase64String(array);
                                            model.ImagePath = answerDto.ImagePath;
                                            model.Base64Image = base64;
                                        }
                                    }
                                    createQuestionDto.Answers.Add(model);
                                }
                            }
                            if ((await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                createQuestionDto.Answers.AddRange(await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id));
                            }
                            if ((await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                createQuestionDto.Answers.AddRange(await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id));

                                List<SecondPartDto> secondPartDtos = new List<SecondPartDto>();
                                foreach (var firstPart in await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id))
                                {
                                    secondPartDtos.Add(await ms.SecondPart.GetSecondPartDtoByFirstPartId(firstPart.IdFirstPart));
                                }
                                createQuestionDto.Answers.AddRange(secondPartDtos);
                            }

                            testDto.Questions.Add(createQuestionDto);
                        }
                        return Ok(testDto);
                    }
                    else
                    {
                        return NotFound(new { message = "Тест не найден" });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
                }
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("PushTest")]
        public async Task<IActionResult> PushTest([FromHeader] string? Authorization, [FromBody] TestResultModel testResultModel)
        {
            if (!Authorization.IsNullOrEmpty() && testResultModel != null &&
                (!testResultModel.TestId.IsNullOrEmpty() && !testResultModel.EmployeeId.IsNullOrEmpty() &&
                !testResultModel.StartDate.IsNullOrEmpty() && !testResultModel.StartTime.IsNullOrEmpty() &&
                !testResultModel.EndTime.IsNullOrEmpty() && testResultModel.Questions.Count != 0))
            {
                TokenEmployee? token = await ms.TokenEmployee.GetTokenEmployeeByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenEmployeeExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/user-api/PushTest testId={testResultModel.TestId}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "user-api/PushTest",
                        UserId = $"{token.IdEmployee}",
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"TestId={testResultModel.TestId}"
                    });
                    if (await ms.TestPurpose.GetTestPurposeByEmployeeTestId(testResultModel.TestId, testResultModel.EmployeeId) == null)
                        return BadRequest(new { message = "Ошибка. Тест уже выполнен или не назначен" });

                    logger.LogInformation($"/user-api/PushTest testId={testResultModel.TestId} emmployeeId={testResultModel.EmployeeId}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "user-api/GetTestResultsByEmployee",
                        UserId = $"{testResultModel.EmployeeId}",
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"TestId={testResultModel.TestId}, StartTime={testResultModel.StartTime}, EndTime={testResultModel.EndTime}"
                    });

                    string resultId = Guid.NewGuid().ToString();
                    await ms.Result.SaveResult(new Result
                    {
                        Id = resultId,
                        IdTest = testResultModel.TestId,
                        StartDate = DateOnly.Parse(testResultModel.StartDate),
                        StartTime = TimeOnly.Parse(testResultModel.StartTime),
                        EndTime = TimeOnly.Parse(testResultModel.EndTime),
                        Duration = (int)((TimeOnly.Parse(testResultModel.EndTime).ToTimeSpan().TotalMinutes) - (TimeOnly.Parse(testResultModel.StartTime).ToTimeSpan().TotalMinutes)),
                    });

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

                                if ((await ms.Answer.GetAnswerById(answerModel.AnswerId.Value)).Correct.Value)
                                {
                                    countOfCorrectAnswer++;
                                }

                                await ms.EmployeeAnswer.SaveEmployeeAnswer(new EmployeeAnswer
                                {
                                    IdResult = resultId,
                                    IdAnswer = answerModel.AnswerId
                                });
                            }
                            if (subsequenceModel != null && subsequenceModel.SubsequenceId.HasValue)
                            {
                                logger.LogInformation($"subsequenceModel -> id={subsequenceModel.SubsequenceId}, number={subsequenceModel.Number}");

                                if ((await ms.Subsequence.GetSubsequenceById(subsequenceModel.SubsequenceId.Value)).Number == (subsequenceModel.Number.Value))
                                {
                                    countOfCorrectAnswer++;
                                }

                                await ms.EmployeeSubsequence.SaveEmployeeSubsequence(new EmployeeSubsequence
                                {
                                    IdSubsequence = subsequenceModel.SubsequenceId.Value,
                                    IdResult = resultId,
                                    Number = subsequenceModel.Number.Value,
                                });
                            }
                            if (!string.IsNullOrEmpty(fsPartModel.FirstPartId) && fsPartModel.SecondPartId.HasValue && fsPartModel != null)
                            {
                                logger.LogInformation($"fsPartModel -> first={fsPartModel.FirstPartId}, second={fsPartModel.SecondPartId}");

                                if ((await ms.SecondPart.GetSecondPartById(fsPartModel.SecondPartId.Value)).IdFirstPart.Equals(fsPartModel.FirstPartId))
                                {
                                    countOfCorrectAnswer++;
                                }

                                await ms.EmployeeMatching.SaveEmployeeMatching(new EmployeeMatching
                                {
                                    IdFirstPart = fsPartModel.FirstPartId,
                                    IdSecondPart = fsPartModel.SecondPartId,
                                    IdResult = resultId
                                });
                            }
                        }
                        if (countOfAnswers == countOfCorrectAnswer)
                        {
                            score++;
                        }
                    }
                    await ms.EmployeeResult.SaveEmployeeResult(new EmployeeResult
                    {
                        IdResult = resultId,
                        IdEmployee = testResultModel.EmployeeId,
                        ScoreFrom = score, //???
                        ScoreTo = testResultModel.Questions.Count
                    });

                    await ms.TestPurpose.DeleteTestPurposeByEmployeeId(testResultModel.TestId, testResultModel.EmployeeId);

                    return Ok(new { message = $"Тест выполнен. Оценка: {score}" });
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            else
            {
                return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
            }
        }

        [HttpPost("GetTestResultsByEmployee")]
        public async Task<IActionResult> GetTestResultsByEmployee([FromHeader] string? Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenEmployee? token = await ms.TokenEmployee.GetTokenEmployeeByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenEmployeeExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/user-api/GetTestResultsByEmployee testId={id.Id}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "user-api/GetTestResultsByEmployee",
                        UserId = $"{token.IdEmployee}",
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"EmployeeId={id.Id}"
                    });

                    List<EmployeeResultModel>? results = await ms.GetAllEmployeeResultModelsByEmployeeId(id.Id);
                    if (results != null && results.Count != 0)
                    {
                        return Ok(results);
                    }
                    else
                    {
                        return BadRequest(new { message = "Результатов нет" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
    }
}
