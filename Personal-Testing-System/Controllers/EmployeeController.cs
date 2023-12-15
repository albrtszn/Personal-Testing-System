using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
using Microsoft.AspNetCore.SignalR;
using Personal_Testing_System.Hubs;
using Personal_Testing_System.MetaData;

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
        private IHubContext<NotificationHub, INotificationClient> notificationHub;

        public EmployeeController(ILogger<EmployeeController> _logger, MasterService _masterService,
                                  IWebHostEnvironment environment, IHubContext<NotificationHub, INotificationClient> _notificationHub)//, EFDbContext db)
        {
            logger = _logger;
            ms = _masterService;
            this.environment = environment;
            notificationHub = _notificationHub;
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
            return Ok(await ms.TokenEmployee.GetAllTokenEmployees());
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
                                  .Find(x => x != null && !x.Login.IsNullOrEmpty() && x.Login.Equals(loginModel.Login));

            EmployeeModel? model = (await ms.Employee.GetEmployeeModelById(employeeDto.Id));

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
                            Employee = model
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
                            Employee = model
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

        [HttpGet("GetMessages")]
        public async Task<IActionResult> GetMessages([FromHeader] string Authorization)
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
                    logger.LogInformation($"/user-api/GetMessages ");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "user-api/GetMessages",
                        UserId = token.IdEmployee,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });
                    var messages = await ms.Message.GetMessageDtosByEmployee(token.IdEmployee);
                    return Ok(messages.OrderByDescending(x => x.Id));
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заолнены" });
        }

        [HttpGet("GetMesssagesPage")]
        public async Task<IActionResult> GetMesssagesPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenEmployee? token = await ms.TokenEmployee.GetTokenEmployeeByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenEmployeeExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetMesssagesPage");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetMesssagesPage",
                            UserId = token.IdEmployee,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });

                        var Allmessages = (await ms.Message.GetMessageDtosByEmployee(token.IdEmployee))
                                                    .OrderByDescending(x => x.Id);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allmessages.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var messages = Allmessages
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(messages);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessage([FromHeader] string Authorization, [FromBody] AddMessageModel message)
        {
            if (!Authorization.IsNullOrEmpty() && message != null && !message.MessageText.IsNullOrEmpty())
            {
                TokenEmployee? token = await ms.TokenEmployee.GetTokenEmployeeByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenEmployeeExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/user-api/AddMessage ");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "user-api/AddMessage",
                        UserId = token.IdEmployee,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now
                    });
                    await ms.Message.SaveMessage(message, token.IdEmployee);
                    return Ok(new { message = "Сообщение добавлено" });
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
                                     .Where(x => x!=null && x.IdEmployee != null && x.IdEmployee.Equals(id.Id)).ToList();

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
                                DatatimePurpose = purpose.DatatimePurpose,
                                Timer = purpose.Timer
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

        [HttpPost("GetPurposesPageByEmployeeId")]
        public async Task<IActionResult> GetPurposesPageByEmployeeId([FromHeader] string? Authorization, [FromBody] StringIdModel? id , [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id) && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenEmployee? token = await ms.TokenEmployee.GetTokenEmployeeByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenEmployeeExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }

                    var Allpurposes = (await ms.TestPurpose.GetAllTestPurposeDtos())
                                     .Where(x => x!=null && x.IdEmployee != null && x.IdEmployee.Equals(id.Id))
                                     .OrderBy(x=>x.DatatimePurpose);

                    var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allpurposes.Count(), pageParams.ItemsPerPage.Value);
                    Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                    var purposes = Allpurposes
                        .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                        .Take(pageParams.ItemsPerPage.Value)
                        .ToList();

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
                                DatatimePurpose = purpose.DatatimePurpose,
                                Timer = purpose.Timer
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
                        questions.OrderBy(x => x.Number);


                        TestModel testDto = new TestModel
                        {
                            Id = test.Id,
                            Name = test.Name,
                            Weight = test.Weight,
                            Generation = test.Generation,
                            Description = test.Description,
                            Instruction = test.Instruction,
                            CompetenceId = test.IdCompetence.Value,
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
                                Weight = quest.Weight,
                                Answers = new List<object>() { }
                            };
                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath))
                                {
                                    byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath);
                                    string base64 = Convert.ToBase64String(array);
                                    createQuestionDto.Base64Image = base64;
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
                                List<FirstPartDto> firstPartDtos = await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id);
                                List<SecondPartDto> secondPartDtos = new List<SecondPartDto>();

                                foreach (var firstPart in firstPartDtos)
                                {
                                    secondPartDtos.Add(await ms.SecondPart.GetSecondPartDtoByFirstPartId(firstPart.IdFirstPart));
                                }

                                Random rand = new Random();
                                firstPartDtos = firstPartDtos.OrderBy(x => rand.Next()).ToList();
                                secondPartDtos = secondPartDtos.OrderBy(x => rand.Next()).ToList();

                                createQuestionDto.Answers.AddRange(firstPartDtos);
                                createQuestionDto.Answers.AddRange(secondPartDtos);

                                /*createQuestionDto.Answers.AddRange(await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id));

                                List<SecondPartDto> secondPartDtos = new List<SecondPartDto>();
                                foreach (var firstPart in await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id))
                                {
                                    secondPartDtos.Add(await ms.SecondPart.GetSecondPartDtoByFirstPartId(firstPart.IdFirstPart));
                                }
                                createQuestionDto.Answers.AddRange(secondPartDtos);*/
                            }

                            testDto.Questions.Add(createQuestionDto);
                        }
                        testDto.Questions = testDto.Questions.OrderBy(x => x.Number).ToList();
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

        [HttpPost("GetEntityTest")]
        public async Task<IActionResult> GetEntityTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
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
                    else
                    {
                        logger.LogInformation($"/admin-api/GetTest :id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "user-api/GetTest",
                            UserId = token.IdEmployee,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Теста={id.Id}"
                        });

                        if (await ms.Test.GetTestById(id.Id) == null)
                            return NotFound(new { message = "Ошибка. Тест не найден" });

                        var test = await ms.Test.GetTestDtoById(id.Id);

                        return Ok(test);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("GetQuestionsInTestPage")]
        public async Task<IActionResult> GetQuestionsInTestPage([FromHeader] string Authorization, [FromBody] StringIdModel? id, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id) && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenEmployee? token = await ms.TokenEmployee.GetTokenEmployeeByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenEmployeeExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/user-api/GetTests ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "user-api/GetTests",
                            UserId = token.IdEmployee,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        if (await ms.Test.GetTestById(id.Id) == null)
                            return NotFound(new { message = "Ошибка. Тест не найден" });

                        var AllquestionsIntest = (await ms.Question.GetAllQuestions())
                                                 .Where(x => x.IdTest.Equals(id.Id))
                                                 .OrderBy(x => x.Number);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, AllquestionsIntest.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var questions = AllquestionsIntest
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        List<QuestionModel> questionModels = new List<QuestionModel>();
                        foreach (var quest in questions)
                        {
                            QuestionModel createQuestionDto = new QuestionModel
                            {
                                Id = quest.Id,
                                IdQuestionType = quest.IdQuestionType,
                                Text = quest.Text,
                                ImagePath = quest.ImagePath,
                                Number = Convert.ToInt32(quest.Number),
                                Weight = quest.Weight,
                                Answers = new List<object>() { }
                            };
                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath))
                                {
                                    byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath);
                                    string base64 = Convert.ToBase64String(array);
                                    createQuestionDto.Base64Image = base64;
                                }
                            }

                            if ((await ms.Answer.GetAnswerDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                List<Answer> list = await ms.Answer.GetAnswersByQuestionId(quest.Id);
                                foreach (Answer answer in list)
                                {
                                    AnswerModel model = new AnswerModel
                                    {
                                        IdAnswer = answer.Id,
                                        Text = answer.Text,
                                        IdQuestion = answer.IdQuestion,
                                        Number = answer.Number,
                                        Weight = answer.Weight,
                                        Correct = answer.Correct
                                    };
                                    if (!answer.ImagePath.IsNullOrEmpty())
                                    {
                                        if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("/images/" + answer.ImagePath).PhysicalPath))
                                        {
                                            byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("/images/" + answer.ImagePath).PhysicalPath);
                                            string base64 = Convert.ToBase64String(array);
                                            model.Base64Image = base64;
                                            model.ImagePath = answer.ImagePath;
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
                                List<FirstPartDto> firstPartDtos = await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id);
                                List<SecondPartDto> secondPartDtos = new List<SecondPartDto>();

                                foreach (var firstPart in firstPartDtos)
                                {
                                    secondPartDtos.Add(await ms.SecondPart.GetSecondPartDtoByFirstPartId(firstPart.IdFirstPart));
                                }

                                createQuestionDto.Answers.AddRange(firstPartDtos);
                                createQuestionDto.Answers.AddRange(secondPartDtos);
                            }
                            questionModels.Add(createQuestionDto);
                        }
                        return Ok(questionModels);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        private async Task<int> GetScoreOfResultAsync(string resultId)
        {
            Result? result = (await ms.Result.GetResultById(resultId));
            if (result == null)
                return 0;
            EmployeeResult employeeResult = (await ms.EmployeeResult.GetEmployeeResultByResultId(result.Id));
            List<EmployeeAnswer>? employeeAnswers = await ms.EmployeeAnswer.GetAllEmployeeAnswersByResultId(result.Id);
            List<EmployeeSubsequence>? employeeSubs = await ms.EmployeeSubsequence.GetAllEmployeeSubsequencesByResultId(result.Id);
            List<EmployeeMatching>? employeeMatchs = await ms.EmployeeMatching.GetAllEmployeeMatchingsByResultId(result.Id);


            Test? test = await ms.Test.GetTestById(result.IdTest);
            if (test == null)
                return 0;

            List<Question> questions = (await ms.Question.GetAllQuestions())
                .Where(x => x.IdTest.Equals(test.Id)).ToList();
            questions = questions.OrderBy(x => x.Number).ToList();

            EmployeeResultAnswersModel testDto = new EmployeeResultAnswersModel
            {
                Id = test.Id,
                Name = test.Name,
                ScoreFrom = employeeResult.ScoreFrom,
                ScoreTo = employeeResult.ScoreTo,
                Generation = test.Generation,
                Instruction = test.Instruction,
                Description = test.Description,
                CompetenceId = test.IdCompetence.Value,
                Questions = new List<QuestionModel>()
            };

            foreach (var quest in questions)
            {
                QuestionModel createQuestionDto = new QuestionModel
                {
                    Id = quest.Id,
                    IdQuestionType = quest.IdQuestionType,
                    Text = quest.Text,
                    ImagePath = quest.ImagePath,
                    Number = Convert.ToInt32(quest.Number),
                    Weight = quest.Weight,
                    Answers = new List<object>() { }
                };
                if (!quest.ImagePath.IsNullOrEmpty())
                {
                    if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath))
                    {
                        byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath);
                        string base64 = Convert.ToBase64String(array);
                        createQuestionDto.Base64Image = base64;
                    }
                }

                if ((await ms.Answer.GetAnswerDtosByQuestionId(quest.Id)).Count != 0)
                {
                    List<Answer> list = await ms.Answer.GetAnswersByQuestionId(quest.Id);
                    foreach (Answer answer in list)
                    {
                        EmployeeAnswerModel model = new EmployeeAnswerModel
                        {
                            IdAnswer = answer.Id,
                            Text = answer.Text,
                            IdQuestion = answer.IdQuestion,
                            Number = answer.Number,
                            Weight = answer.Weight,
                            Correct = answer.Correct
                        };
                        if (!answer.ImagePath.IsNullOrEmpty())
                        {
                            if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("/images/" + answer.ImagePath).PhysicalPath))
                            {
                                byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("/images/" + answer.ImagePath).PhysicalPath);
                                string base64 = Convert.ToBase64String(array);
                                model.Base64Image = base64;
                                model.ImagePath = answer.ImagePath;
                            }
                        }
                        if (employeeAnswers.Find(x => x.IdAnswer.Equals(answer.Id)) != null)
                        {
                            model.IsUserAnswer = true;
                        }
                        else
                        {
                            model.IsUserAnswer = false;
                        }
                        createQuestionDto.Answers.Add(model);
                    }
                }
                if ((await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id)).Count != 0)
                {
                    List<SubsequenceDto> subs = await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id);
                    foreach (SubsequenceDto dto in subs)
                    {
                        EmployeeSubsequence userSubsequence = employeeSubs.Find(x => x.IdSubsequence.Equals(dto.IdSubsequence));
                        if (userSubsequence != null)
                        {
                            dto.Number = userSubsequence.Number;
                            createQuestionDto.Answers.Add(dto);
                        }
                        else
                        {
                            dto.Number = 0;
                            createQuestionDto.Answers.Add(dto);
                        }
                    }
                    //createQuestionDto.Answers.AddRange();
                }
                if ((await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id)).Count != 0)
                {
                    List<FirstPartDto> firstPartDtos = await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id);
                    List<SecondPartDto> secondPartDtos = new List<SecondPartDto>();

                    foreach (var firstPart in firstPartDtos)
                    {
                        secondPartDtos.Add(await ms.SecondPart.GetSecondPartDtoByFirstPartId(firstPart.IdFirstPart));
                    }

                    /*Random rand = new Random();
                    firstPartDtos = firstPartDtos.OrderBy(x => rand.Next()).ToList();
                    secondPartDtos = secondPartDtos.OrderBy(x => rand.Next()).ToList();*/

                    List<EmployeeResultFSPartModel> models = new List<EmployeeResultFSPartModel>();
                    foreach (SecondPartDto sDto in secondPartDtos)
                    {
                        EmployeeMatching match = employeeMatchs.Find(x => x.IdSecondPart.Equals(sDto.IdSecondPart));
                        if (match != null)
                        {
                            models.Add(new EmployeeResultFSPartModel
                            {
                                FirstPartId = firstPartDtos.FirstOrDefault(x => x.IdFirstPart.Equals(match.IdFirstPart)),
                                SecondPartId = secondPartDtos.FirstOrDefault(x => x.IdSecondPart.Equals(match.IdSecondPart)),
                                IsUserAnswer = true
                            });
                        }
                        else
                        {
                            models.Add(new EmployeeResultFSPartModel
                            {
                                FirstPartId = firstPartDtos.FirstOrDefault(x => x.IdFirstPart.Equals(sDto.IdFirstPart)),
                                SecondPartId = sDto,
                                IsUserAnswer = false
                            });
                        }
                    }

                    createQuestionDto.Answers.Add(models);
                    /*createQuestionDto.Answers.AddRange(firstPartDtos);
                    createQuestionDto.Answers.AddRange(secondPartDtos);*/
                }
                testDto.Questions.Add(createQuestionDto);
            }
            testDto.Questions.OrderBy(x => x.Number);

            return RateLogic.RateLogic.GetPointTest(testDto, testDto.Id);
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
                    TestPurpose? purpose = await ms.TestPurpose.GetTestPurposeByEmployeeTestId(testResultModel.TestId, testResultModel.EmployeeId);
                    //todo datetime purpose 
                    /*if ( DateOnly.FromDateTime(purpose.DatatimePurpose.Value) < DateOnly.Parse(testResultModel.StartTime) )
                        return BadRequest(new { message = "Ошибка. Тест уже выполнен или не назначен" });*/

                    if (purpose == null )
                        return BadRequest(new { message = "Ошибка. Тест уже выполнен или не назначен" });

                    Test? testCheck = await ms.Test.GetTestById(testResultModel.TestId);
                    if (testCheck == null)
                        return NotFound(new { message = "Ошибка. Такого теста нет"});

                    string resultId = Guid.NewGuid().ToString();
                    Result result = new Result
                    {
                        Id = resultId,
                        IdTest = testResultModel.TestId,
                        StartDate = DateOnly.Parse(testResultModel.StartDate),
                        StartTime = TimeOnly.Parse(testResultModel.StartTime),
                        EndTime = TimeOnly.Parse(testResultModel.EndTime),
                        Duration = (int)((TimeOnly.Parse(testResultModel.EndTime).ToTimeSpan().TotalMinutes) - (TimeOnly.Parse(testResultModel.StartTime).ToTimeSpan().TotalMinutes)),
                    };
                    await ms.Result.SaveResult(result);

                    Dictionary<int, int> scoreMap = new Dictionary<int, int>();
                    int score = 0;
                    foreach (QuestionResultModel question in testResultModel.Questions)
                    {
                        //todo question score
                        Question? quest= await ms.Question.GetQuestionById(question.QuestionId);
                        QuestionSubcompetence? questSubcompetence = await ms.QuestionSubcompetence.GetQuestionSubcompetenceByQuestionId(quest.Id);
                        if (quest != null)
                        {
                            int subFlag = 0;
                            int fsPartFlag = 0;
                            foreach (JsonElement answer in question.Answers)
                            {
                                AnswerResultModel answerModel = answer.Deserialize<AnswerResultModel>();
                                AnswerWeightResultModel answerWeightModel = answer.Deserialize<AnswerWeightResultModel>();
                                SubsequenceResultModel subsequenceModel = answer.Deserialize<SubsequenceResultModel>();
                                FSPartResultModel fsPartModel = answer.Deserialize<FSPartResultModel>();

                                if (answerModel != null && answerModel.AnswerId.HasValue && answerModel.AnswerId > 0)
                                {
                                    logger.LogInformation($"answerModel -> text={answerModel.AnswerId}");

                                    Answer answerCheck = await ms.Answer.GetAnswerById(answerModel.AnswerId.Value);
                                    //todo answer correct field
                                    if (quest.IdQuestionType == 1)// && answerCheck.Correct != null && answerCheck.Correct.Value)
                                    {
                                        if (answerCheck.Weight != null && answerCheck.Weight.HasValue)
                                            score += answerCheck.Weight.Value;
                                    }
                                    if (quest.IdQuestionType == 2)// && answerCheck.Correct != null && answerCheck.Correct.Value)
                                    {
                                        if (answerCheck.Weight != null && answerCheck.Weight.HasValue)
                                            score += answerCheck.Weight.Value;
                                    }
                                    //Subcompetence Score
                                    if (questSubcompetence != null)
                                    {
                                        if (!scoreMap.ContainsKey(questSubcompetence.Id))
                                        {
                                            scoreMap.Add(questSubcompetence.Id, score);
                                        }
                                        else
                                        {
                                            scoreMap[questSubcompetence.Id] += score;
                                        }
                                    }

                                    await ms.EmployeeAnswer.SaveEmployeeAnswer(new EmployeeAnswer
                                    {
                                        IdResult = resultId,
                                        IdAnswer = answerModel.AnswerId
                                    });

                                    /*if (answerWeightModel != null && answerWeightModel.AnswerId.HasValue && answerWeightModel.AnswerId > 0 && answerWeightModel.NewWeight != null && answerWeightModel.NewWeight.HasValue)
                                    {
                                        logger.LogInformation($"answerWeightModel -> answerId={answerWeightModel.AnswerId}, newWeight={answerWeightModel.NewWeight}");
                                        score += answerWeightModel.NewWeight.Value;
                                        await ms.EmployeeAnswer.SaveEmployeeAnswer(new EmployeeAnswer
                                        {
                                            IdResult = resultId,
                                            IdAnswer = answerModel.AnswerId
                                        });
                                    }*/
                                }

                                if (subsequenceModel != null && subsequenceModel.SubsequenceId.HasValue)
                                {
                                    logger.LogInformation($"subsequenceModel -> id={subsequenceModel.SubsequenceId}, number={subsequenceModel.Number}");
                                    int subsequenceCount = (await ms.Subsequence.GetSubsequencesByQuestionId(question.QuestionId)).Count;
                                    if ((await ms.Subsequence.GetSubsequenceById(subsequenceModel.SubsequenceId.Value)).Number == (subsequenceModel.Number.Value))
                                    {
                                        subFlag++;
                                    }

                                    if(subFlag == subsequenceCount)
                                    {
                                        if (quest.Weight != null)
                                        {
                                            score += quest.Weight.Value;
                                        }
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

                                    int fsPartsCount = (await ms.FirstPart.GetFirstPartsByQuestionId(question.QuestionId)).Count;
                                    if ((await ms.SecondPart.GetSecondPartById(fsPartModel.SecondPartId.Value)).IdFirstPart.Equals(fsPartModel.FirstPartId))
                                    {
                                        fsPartFlag++;
                                    }

                                    if(fsPartFlag == fsPartsCount)
                                    {
                                        if (quest.Weight != null)
                                        {
                                            score += quest.Weight.Value;
                                        }
                                    }

                                    await ms.EmployeeMatching.SaveEmployeeMatching(new EmployeeMatching
                                    {
                                        IdFirstPart = fsPartModel.FirstPartId,
                                        IdSecondPart = fsPartModel.SecondPartId,
                                        IdResult = resultId
                                    });
                                }
                            }
                            subFlag = 0;
                            fsPartFlag = 0;
                        }
                    }
                    await ms.EmployeeResult.SaveEmployeeResult(new EmployeeResult
                    {
                        IdResult = resultId,
                        IdEmployee = testResultModel.EmployeeId,
                        ScoreFrom = score, 
                        ScoreTo = testCheck.Weight.Value
                    });

                    if (!scoreMap.IsNullOrEmpty())
                    {
                        foreach (var item in scoreMap)
                        {
                            Console.WriteLine($"key={item.Key}, value={item.Value}");
                            if (item.Key != 0)
                            {
                                if (await ms.Subcompetence.GetSubcompetenceById(item.Key) != null)
                                {
                                    await ms.EmployeeResultSubcompetence.SaveEmployeeResultSubcompetence(new ElployeeResultSubcompetence()
                                    {
                                        IdSubcompetence = item.Key,
                                        IdResult = resultId,
                                        Result = item.Value
                                    });
                                }
                            }
                        }
                    }
                    EmployeeResult? employeeResult = await ms.EmployeeResult.GetEmployeeResultByResultId(resultId);
                    int resultRate = await GetScoreOfResultAsync(resultId);
                    if (employeeResult != null && resultRate != 0)
                    {
                        employeeResult.ScoreTo = resultRate;
                        await ms.EmployeeResult.SaveEmployeeResult(employeeResult);
                    }

                    //todo deletePurpose
                    //await ms.TestPurpose.DeleteTestPurposeByEmployeeId(testResultModel.TestId, testResultModel.EmployeeId);
                    Employee? employee = await ms.Employee.GetEmployeeById(token.IdEmployee);
                    if (employee != null) {
                        await notificationHub.Clients.All.ReceiveMessage($"{DateTime.Now} Пользователь '{employee.FirstName} {employee.SecondName} {employee.LastName} завершил тест '{testCheck.Name}'.");
                    }
                    logger.LogInformation($"ScoreInformation(/PushTest): OwnScore={score}, Rate={resultRate}");
                    if (resultRate != 0)
                    {
                        return Ok(new { message = $"Тест выполнен. Оценка: {resultRate}" });
                    }
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
