using DataBase;
using DataBase.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Hubs;
using Personal_Testing_System.MetaData;
using Personal_Testing_System.Models;
using Personal_Testing_System.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.IO;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Wordroller;
using Wordroller.Content.Images;
using Wordroller.Content.Text;
using Xceed.Words.NET;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace Personal_Testing_System.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("admin-api")]

    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> logger;
        private readonly IWebHostEnvironment environment;
        private MasterService ms;
        private IHubContext<NotificationHub, INotificationClient> notificationHub;
        public AdminController(ILogger<AdminController> _logger, MasterService _masterService,
                               IWebHostEnvironment _environmentironment, IHubContext<NotificationHub, INotificationClient> _notificationHub)//, EFDbContext db)
        {
            logger = _logger;
            ms = _masterService;
            environment = _environmentironment;
            notificationHub = _notificationHub;
            this.notificationHub = notificationHub;
            ///InitDB.InitData(db);
            //CreateDirectory !!!
            /*if (!Directory.Exists(environment.WebRootPath + "/images"))
            {
                Directory.CreateDirectory(environment.WebRootPath + "/images");
            }*/
        }

        [HttpGet("Ping")]
        public async Task<IActionResult> Ping()
        {
            return Ok(new { message = $"Ping: {HttpContext.Request.Host + HttpContext.Request.Path} {DateTime.Now}." });
        }
        [HttpGet("RequestestNotification")]
        public async Task<IActionResult> RequestestNotification()
        {
            await notificationHub.Clients.Group(Roles.ADMIN.ToString()).ReceiveMessage($"Test message to all connections.{DateTime.Now}");
            return NoContent();
        }
        [HttpGet("RequesMessageNotification")]
        public async Task<IActionResult> RequesMessageNotification()
        {
            await notificationHub.Clients.Group(Roles.ADMIN.ToString()).MessageNotification($"Test message to all connections.{DateTime.Now}");
            return NoContent();
        }
        [HttpGet("RequestestTestCompleteNotification")]
        public async Task<IActionResult> RequesTestCompleteNotification()
        {
            await notificationHub.Clients.Group(Roles.ADMIN.ToString()).TestCompleteNotification($"Test message to all connections.{DateTime.Now}");
            return NoContent();
        }
        [HttpGet("RequestEmployeeNotification")]
        public async Task<IActionResult> RequestEmployeeNotification()
        {
            await notificationHub.Clients.Group(Roles.ADMIN.ToString()).EmployeeNotification($"Test message to all connections.{DateTime.Now}");
            return NoContent();
        }       
        [HttpGet("RequesConfigurationNotification")]
        public async Task<IActionResult> RequestestConfigurationNotification()
        {
            await notificationHub.Clients.Group(Roles.ADMIN.ToString()).ConfigurationNotification($"Test message to all connections.{DateTime.Now}");
            return NoContent();
        }
        /*
         *  TEST METHODS
         */
        /*[HttpGet("CalculateTestsWeight")]
        public async Task<IActionResult> CalculateTestsWeight()
        {
            var tests = await ms.Test.GetAllTests();
            if (tests.Any())
            {
                foreach(var test in tests)
                {
                    int testWeight = 0;
                    var questions = await ms.Question.GetQuestionsByTest(test.Id);
                    if (questions.Any())
                    {
                        foreach (var question in questions)
                        {
                            if (question.Weight.HasValue)
                            {
                                testWeight += question.Weight.Value;
                            }
                        }
                        test.Weight = testWeight;
                        logger.LogInformation($"testId={test.Id}, newScore={test.Weight}");
                        await ms.Test.SaveTest(test);
                    }
                }
            }
            return NoContent();
        }
        [HttpPost("InitAdminDB")]
        public async Task<IActionResult> InitAdminDB()
        {
            if (!(await ms.Admin.GetAllAdmins()).Any())
            {
                await ms.Admin.SaveAdmin(new Admin
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Иван",
                    SecondName = "Иванов",
                    LastName = "Иванович",
                    Login = "admin",
                    Password = "password",
                    //DateOfBirth = DateOnly.Parse("01.01.2000"),
                    //IdSubdivision = (await ms.Subdivision.GetAllSubdivisions()).Find(x => x.Name.Equals("Отдел кадров")).Id
                });
            }

            /*if (!(await ms.TestType.GetAllCompetences()).Any())
            {
                await ms.TestType.SaveCompetence(new Competence
                {
                    Name = "Оценка имеющихся компетенций"
                });
            }

            if (!(await ms.QuestionType.GetAllQuestionTypes()).Any())
            {
                await ms.QuestionType.SaveQuestionType(new QuestionType
                {
                    Name = "Выбор одного варианта ответа"
                });

                await ms.QuestionType.SaveQuestionType(new QuestionType
                {
                    Name = "Выбор нескольких вариантов ответа"
                });

                await ms.QuestionType.SaveQuestionType(new QuestionType
                {
                    Name = "Установка соответствия"
                });

                await ms.QuestionType.SaveQuestionType(new QuestionType
                {
                    Name = "Расстановка в нужном порядке"
                });
            }
            return Ok();
        }*/

        /*[HttpGet("TestGetWord")]
        public async Task<IActionResult> TestGetWord()
        {


            WordDocument doc = new WordDocument(CultureInfo.GetCultureInfo("ru-ru"));
            doc.Styles.DocumentDefaults.RunProperties.Font.Ascii = "Times New Roman";
            doc.Styles.DocumentDefaults.RunProperties.Font.HighAnsi = "Times New Roman";

            var section = doc.Body.Sections.First();
            var paragraph = section.AppendParagraph();
            paragraph.AppendText("sample text");
            paragraph.AppendText("\n");

            FileStream file = new FileStream("D:\\trash_collection\\2.jpg",FileMode.Open);
            var image = doc.AddImage(file,KnownImageContentTypes.Jpg);
            var picture = section.WrapImageIntoInlinePicture(image, "wordroller","",200,100);
            paragraph.AppendPicture(picture);

            byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms);
                res = ms.ToArray();
            }

            return File(res, "application/octet-stream", "sample.docx"); 
        }*/

        [HttpGet("TestGetAdmins")]
        public async Task<IActionResult> TestGetAdmins()
        {
            return Ok(await ms.Admin.GetAllAdminDtos());
        }
        /*[HttpGet("TestGetAdminTokens")]
        public async Task<IActionResult> TestGetAdminTokens()
        {
            return Ok(await ms.TokenAdmin.GetAllTokenAdmins());
        }

        [HttpDelete("DeleteEmployeeTokens")]
        public async Task<IActionResult> DeleteEmployeeTokens()
        {
            (await ms.TokenAdmin.GetAllTokenAdmins()).ForEach(async x => await ms.TokenAdmin.DeleteTokenAdminById(x.Id));
            if (!(await ms.TokenAdmin.GetAllTokenAdmins()).Any())
            {
                return Ok(new { messsage = "Токены удалены" });
            }
            return BadRequest(new { messsage = "Ошибка при удалении токенов" });
        }
        [HttpDelete("DeletePurposes")]
        public async Task<IActionResult> DeletePurposes()
        {
            (await ms.TestPurpose.GetAllTestPurposes())
                     .ForEach(async x => await ms.TestPurpose.DeleteTestPurposeById(x.Id));
            if (!(await ms.TestPurpose.GetAllTestPurposes()).Any())
            {
                return Ok(new { messsage = "Назначения удалены" });
            }
            return BadRequest(new { messsage = "Ошибка при удалении токенов" });
        }        
        [HttpDelete("DeleteResults")]
        public async Task<IActionResult> DeleteResults()
        {
            foreach (var answer in await ms.EmployeeAnswer.GetAllEmployeeAnswers()) { 
                     await ms.EmployeeAnswer.DeleteEmployeeAnswerById(answer.Id);
            }
            foreach (var match in await ms.EmployeeMatching.GetAllEmployeeMatchings()) { 
                    await ms.EmployeeMatching.DeleteEmployeeMatchingById(match.Id);
            }
            foreach (var sub in await ms.EmployeeSubsequence.GetAllEmployeeSubsequences()) { 
                    await ms.EmployeeSubsequence.DeleteEmployeeSubsequenceById(sub.Id);
            }
            foreach (var employeeResult in await ms.EmployeeResult.GetAllEmployeeResults())
            {
                await ms.EmployeeResult.DeleteEmployeeResultById(employeeResult.Id);
            }
            foreach (var result in await ms.Result.GetAllResults()) { 
                    await ms.Result.DeleteResultById(result.Id);
            }


            /*await ms.EmployeeAnswer.DeleteEmployeeAnswersByResult(result.IdResult);
            await ms.EmployeeMatching.DeleteEmployeeMatchingByResult(result.IdResult);
            await ms.EmployeeSubsequence.DeleteEmployeeSubsequenceByResult(result.IdResult);
            await ms.Result.DeleteResultById(result.IdResult);
            await ms.EmployeeResult.DeleteEmployeeResultById(result.Id.Value);
            if (!(await ms.Result.GetAllResults()).Any())
            {
                return Ok(new { messsage = "Результаты удалены" });
            }
            return BadRequest(new { messsage = "Ошибка при удалении результатов" });
        }
        [HttpDelete("DeleteEmployees")]
        public async Task<IActionResult> DeleteEmployees()
        {
            (await ms.Employee.GetAllEmployees())
                     .ForEach(async x => await ms.Employee.DeleteEmployeeById(x.Id));
            if (!(await ms.Employee.GetAllEmployees()).Any())
            {
                return Ok(new { messsage = "Сотрудники удалены" });
            }
            return BadRequest(new { messsage = "Ошибка при удалении токенов" });
        }*/
        /*
         * 
         */

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel? loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new { message = "Одно из полей пустое" });
            }
            logger.LogInformation($"/admin-api/Login : login={loginModel.Login}, Password={loginModel.Password} ");
            await ms.Log.SaveLog(new Log
            {
                UrlPath = "admin-api/Login",
                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                DataTime = DateTime.Now,
                Params = $"логин={loginModel.Login}, пароль={loginModel.Password}"
            });

            AdminDto? adminDto = (await ms.Admin.GetAllAdminDtos())
                                  .Find(x => x.Login.Equals(loginModel.Login));

            if (adminDto == null)
            {
                return BadRequest(new { message = "Администратор не найден" });
            }
            else
            {
                if (loginModel.Password.Equals(adminDto.Password))
                {
                    TokenAdmin? tokenAdmin = await ms.TokenAdmin.GetTokenAdminByAdminId(adminDto.Id);
                    if (tokenAdmin != null && !await ms.IsTokenAdminExpired(tokenAdmin))
                    {
                        return Ok(new
                        {
                            TokenAdmin = tokenAdmin.Token,
                            Admin = adminDto
                        });
                    }
                    else
                    {
                        string token = Guid.NewGuid().ToString();
                        await ms.TokenAdmin.SaveTokenAdmin(new TokenAdmin
                        {
                            IdAdmin = adminDto.Id,
                            Token = token,
                            IssuingTime = DateTime.Now,
                            State = true
                        });
                        return Ok(new
                        {
                            TokenAdmin = token,
                            Admin = adminDto
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
        public async Task<IActionResult> LogOut([FromHeader] string? Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/LogOut : AuthHeader={Authorization}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/LogOut",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"Id администратора={token.IdAdmin}"
                    });
                    await ms.TokenAdmin.DeleteTokenAdminById(token.Id);
                    return Ok(new { message = "Выполнен выход из системы" });
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
        *  GlobalConfigure
        */
        [SwaggerOperation(Tags = new[] { "Admin/GlobalConfigure" })]
        [HttpGet("GetGlobalConfigures")]
        public async Task<IActionResult> GetGlobalConfigures([FromHeader] string Authorization)
        {
            logger.LogInformation($"/admin-api/GetGlobalConfigures");
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetGlobalConfigures : AuthHeader={Authorization}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetGlobalConfigures",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });
                    return Ok(await ms.GlobalConfigure.GetAllGlobalConfigureDtos());
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/GlobalConfigure" })]
        [HttpPost("AddGlobalConfigure")]
        public async Task<IActionResult> AddGlobalConfigure([FromHeader] string Authorization, [FromBody] AddGlobalConfigureModel? model)
        {
            if (!Authorization.IsNullOrEmpty() && model != null 
                && model.TestingTimeLimit.HasValue && model.SkippingQuestion.HasValue
                && model.EarlyCompletionTesting.HasValue )
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                        logger.LogInformation($"/admin-api/AddGlobalConfigure ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddGlobalConfigure",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        await ms.GlobalConfigure.SaveGlobalConfigure(model);
                        return Ok(new { message = "Конфигурация добавлена" });
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/GlobalConfigure" })]
        [HttpPost("UpdateGlobalConfigure")]
        public async Task<IActionResult> UpdateGlobalConfigure([FromHeader] string Authorization, [FromBody] GlobalConfigureDto? dto)
        {
            if (!Authorization.IsNullOrEmpty() && dto != null &&  dto.Id.HasValue && dto.Id != 0
                && dto.TestingTimeLimit.HasValue && dto.SkippingQuestion.HasValue
                && dto.EarlyCompletionTesting.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/UpdateGlobalConfigure ");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/UpdateGlobalConfigure",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now
                    });
                    await ms.GlobalConfigure.SaveGlobalConfigure(dto);
                    //await notificationHub.Clients.All.ReceiveMessage($"UpdateGlobalConfig: Временной лимт:'{dto.TestingTimeLimit}, Пропуск вопросов: {dto.SkippingQuestion}, Завершение: {dto.EarlyCompletionTesting}.");
                    await notificationHub.Clients.All.ConfigurationNotification($"UpdateGlobalConfig: Временной лимт:'{dto.TestingTimeLimit}, Пропуск вопросов: {dto.SkippingQuestion}, Завершение: {dto.EarlyCompletionTesting}.");
                    return Ok(new { message = "Конфигурация обновлена" });
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/GlobalConfigure" })]
        [HttpPost("DeleteGlobalConfigure")]
        public async Task<IActionResult> DeleteGlobalConfigure([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if ((await ms.Profile.GetProfileById(id.Id.Value)) != null)
                        {
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/DeleteGlobalConfigure",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now
                            });
                            if (await ms.GlobalConfigure.GetGlobalConfigureById(id.Id.Value) != null)
                            {
                                await ms.GlobalConfigure.DeleteGlobalConfigureById(id.Id.Value);
                            }
                            return Ok(new { message = "Конфигурация удалена" });
                        }
                        else
                        {
                            return NotFound(new { message = "Ошибка. Такой конфигурации не существует" });
                        }
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
        *  Profile
        */
        //[ApiExplorerSettings(GroupName = "Profiles")]
        [SwaggerOperation(Tags = new[] { "Admin/Profile" })]
        [HttpGet("GetProfiles")]
        public async Task<IActionResult> GetProfiles([FromHeader] string Authorization)
        {
            logger.LogInformation($"/admin-api/GetProfiles");
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetProfiles : AuthHeader={Authorization}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetProfiles",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });
                    return Ok(await ms.Profile.GetAllProfileDtos());
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Profile" })]
        [HttpGet("GetProfilesPage")]
        public async Task<IActionResult> GetProfilesPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            logger.LogInformation($"/admin-api/GetProfiles");
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetProfiles : AuthHeader={Authorization}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetProfiles",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });

                    var Allprofiles = (await ms.Profile.GetAllProfileDtos())
                                                    .OrderBy(x => x.Id);

                    var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allprofiles.Count(), pageParams.ItemsPerPage.Value);
                    Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                    var profiles = Allprofiles
                        .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                        .Take(pageParams.ItemsPerPage.Value)
                        .ToList();

                    return Ok(profiles);
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Profile" })]
        [HttpPost("AddProfile")]
        public async Task<IActionResult> AddProfile([FromHeader] string Authorization, [FromBody] AddProfileModel? profile)
        {
            if (!Authorization.IsNullOrEmpty() && profile != null && !string.IsNullOrEmpty(profile.Name))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    if ((await ms.Profile.GetAllProfileDtos()).Find(x => x.Name.Equals(profile.Name)) != null)
                    {
                        return BadRequest(new { message = "Ошибка. Такой профиль уже есть" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddProfile :name={profile.Name}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddProfile",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Имя group={profile.Name}"
                        });
                        await ms.Profile.SaveProfile(profile);
                        return Ok(new { message = "Профиль добавлена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Profile" })]
        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromHeader] string Authorization, [FromBody] ProfileDto? profile)
        {
            if (!Authorization.IsNullOrEmpty() && profile != null && profile.Id != 0 &&
                !string.IsNullOrEmpty(profile.Name))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    if (await ms.Profile.GetProfileById(profile.Id.Value) == null)
                    {
                        return BadRequest(new { message = "Ошибка. Такой группы нет" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateProfile :id={profile.Id}, name={profile.Name}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateProfile",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id профиля={profile.Id}, названия профиля={profile.Name}"
                        });
                        await ms.Profile.SaveProfile(profile);
                        return Ok(new { message = "Профиль обновлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Profile" })]
        [HttpPost("DeleteProfile")]
        public async Task<IActionResult> DeleteProfile([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if ((await ms.Profile.GetProfileById(id.Id.Value)) != null)
                        {
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/DeleteProfile",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id профиля={id.Id}"
                            });
                            await ms.Profile.DeleteProfileById(id.Id.Value);
                            return Ok(new { message = "Профиль удалена" });
                        }
                        else
                        {
                            return NotFound(new { message = "Ошибка. Такого профиля не существует" });
                        }
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  GroupPosition
         */
        [SwaggerOperation(Tags = new[] { "Admin/GetGroupPositions" })]
        [HttpGet("GetGroupPositions")]
        public async Task<IActionResult> GetGroupPositions([FromHeader] string Authorization)
        {
            logger.LogInformation($"/admin-api/GetGroupPositions");
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetGroupPositions : AuthHeader={Authorization}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetGroupPositions",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });
                    return Ok(await ms.GroupPosition.GetAllGroupPositionDtos());
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/GetGroupPositions" })]
        [HttpGet("GetGroupPositionsPage")]
        public async Task<IActionResult> GetGroupPositionsPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            logger.LogInformation($"/admin-api/GetGroupPositions");
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetGroupPositions : AuthHeader={Authorization}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetGroupPositions",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });

                    var AllgroupPositions = (await ms.GroupPosition.GetAllGroupPositionDtos())
                                                    .OrderBy(x => x.Id);

                    var pageHeader = new PageHeader(pageParams.PageNumber.Value, AllgroupPositions.Count(), pageParams.ItemsPerPage.Value);
                    Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                    var groupPositions = AllgroupPositions
                        .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                        .Take(pageParams.ItemsPerPage.Value)
                        .ToList();

                    return Ok(groupPositions);
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/GetGroupPositions" })]
        [HttpPost("AddGroupPosition")]
        public async Task<IActionResult> AddGroupPosition([FromHeader] string Authorization, [FromBody] AddGroupPositionModel? groupPos)
        {
            if (!Authorization.IsNullOrEmpty() && groupPos != null && !string.IsNullOrEmpty(groupPos.Name) && groupPos.IdProfile.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    if ((await ms.GroupPosition.GetAllGroupPositionDtos()).Find(x => x.Name.Equals(groupPos.Name)) != null)
                    {
                        return BadRequest(new { message = "Ошибка. Такая группа уже есть" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddGroupPositions :name={groupPos.Name}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddGroupPositions",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Имя group={groupPos.Name}"
                        });
                        await ms.GroupPosition.SaveGroupPosition(groupPos);
                        return Ok(new { message = "Группа добавлена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/GetGroupPositions" })]
        [HttpPost("UpdateGroupPosition")]
        public async Task<IActionResult> UpdateGroupPositions([FromHeader] string Authorization, [FromBody] GroupPositionDto? groupPos)
        {
            if (!Authorization.IsNullOrEmpty() && groupPos != null && groupPos.Id != 0 &&
                !string.IsNullOrEmpty(groupPos.Name) && groupPos.IdProfile != 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    if (await ms.GroupPosition.GetGroupPositionById(groupPos.Id.Value) == null)
                    {
                        return BadRequest(new { message = "Ошибка. Такой группы нет" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateGroupPositions :id={groupPos.Id}, name={groupPos.Name}, Id={groupPos.IdProfile}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateGroupPositions",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id группы={groupPos.Id}, id профиля={groupPos.IdProfile}"
                        });
                        await ms.GroupPosition.SaveGroupPosition(groupPos);
                        return Ok(new { message = "Группа обновлена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/GetGroupPositions" })]
        [HttpPost("DeleteGroupPosition")]
        public async Task<IActionResult> DeleteGroupPosition([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if (await ms.GroupPosition.GetGroupPositionById(id.Id.Value) != null)
                        {
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/DeleteGroupPosition",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id отдела={id.Id}"
                            });
                            await ms.GroupPosition.DeleteGroupPositionById(id.Id.Value);
                            return Ok(new { message = "Группа удалена" });
                        }
                        else
                        {
                            return NotFound(new { message = "Ошибка. Такой группы не существует" });
                        }
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        /*
         *  Subdivision
         */
        [SwaggerOperation(Tags = new[] { "Admin/Subdivision" })]
        [HttpGet("GetSubdivisions")]
        public async Task<IActionResult> GetSubdivisions([FromHeader] string Authorization)
        {
            logger.LogInformation($"/admin-api/GetSubdivisions");
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetSubdivisions : AuthHeader={Authorization}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetSubdivisions",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });
                    return Ok(await ms.Subdivision.GetAllSubdivisionDtos());
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subdivision" })]
        [HttpGet("GetSubdivisionsPage")]
        public async Task<IActionResult> GetSubdivisionsPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            logger.LogInformation($"/admin-api/GetSubdivisions");
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetSubdivisions : AuthHeader={Authorization}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetSubdivisions",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });

                    var Allsubs = (await ms.Subdivision.GetAllSubdivisionDtos())
                                                    .OrderBy(x => x.Id);

                    var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allsubs.Count(), pageParams.ItemsPerPage.Value);
                    Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                    var subs = Allsubs
                        .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                        .Take(pageParams.ItemsPerPage.Value)
                        .ToList();

                    return Ok(subs);
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subdivision" })]
        [HttpPost("AddSubdivision")]
        public async Task<IActionResult> AddSubdivision([FromHeader] string Authorization, [FromBody] SubdivisionModel? sub)
        {
            if (!Authorization.IsNullOrEmpty() && sub != null && !string.IsNullOrEmpty(sub.Name) && sub.IdGroupPositions != 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    /*if ((await ms.Subdivision.GetAllSubdivisions()).Find(x => x.Name.Equals(sub.Name)) != null)
                    {
                        return BadRequest(new { message = "Ошибка. Такой отдел уже есть" });
                    }
                    else
                    {*/
                    if ((await ms.GroupPosition.GetGroupPositionById(sub.IdGroupPositions.Value)) == null)
                    {
                        return NotFound(new { message = "Ошибка. Такой группы нет" });
                    }

                    logger.LogInformation($"/admin-api/AddSubdivision :name={sub.Name}");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/AddSubdivision",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"Имя Отдела={sub.Name}, id группы={sub.IdGroupPositions}"
                    });
                    await ms.Subdivision.SaveSubdivision(sub);
                    return Ok(new { message = "Отдел добавлен" });
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subdivision" })]
        [HttpPost("UpdateSubdivision")]
        public async Task<IActionResult> UpdateSubdivision([FromHeader] string Authorization, [FromBody] AddSubdivisionModel? sub)
        {
            if (!Authorization.IsNullOrEmpty() && sub != null && !string.IsNullOrEmpty(sub.Name) && sub.IdGroupPositions != 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    if (await ms.Subdivision.GetSubdivisionById(sub.Id.Value) == null)
                    {
                        return BadRequest(new { message = "Ошибка. Такого отдела нет" });
                    }
                    if (await ms.GroupPosition.GetGroupPositionDtoById(sub.IdGroupPositions.Value) == null)
                    {
                        return NotFound(new { message = "Ошибка. Такой группы нет" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateSubdivision :id={sub.Id}, name={sub.Name}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateSubdivision",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id отдела={sub.Id}, Имя Отдела={sub.Name}, Id группы={sub.IdGroupPositions}"
                        });
                        await ms.Subdivision.SaveSubdivision(sub);
                        return Ok(new { message = "Отдел обновлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subdivision" })]
        [HttpPost("DeleteSubdivision")]
        public async Task<IActionResult> DeleteSubdivision([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if (await ms.Subdivision.GetSubdivisionById(id.Id.Value) != null)
                        {
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/DeleteSubdivision",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id отдела={id.Id}"
                            });
                            await ms.Subdivision.DeleteSubdivisionById(id.Id.Value);
                            return Ok(new { message = "Отдел удален" });
                        }
                        else
                        {
                            return NotFound(new { message = "Ошибка. Такого отдела не существует" });
                        }
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  Employee
         */

        [SwaggerOperation(Tags = new[] { "Admin/Employee" })]
        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmployees([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetEmployees ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetEmployees",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        List<Employee>? employees = await ms.Employee.GetAllEmployees();
                        List<GetEmployeeModel> models = new List<GetEmployeeModel>();
                        foreach (var employee in employees)
                        {
                            int countOfPurposes = (await ms.TestPurpose.GetAllTestPurposes())
                                .Where(x => x != null && x.IdEmployee != null && !string.IsNullOrEmpty(x.IdEmployee) && x.IdEmployee.Equals(employee.Id))
                                .Count();

                            int countOfResults = (await ms.EmployeeResult.GetAllEmployeeResults())
                                .Where(x => x != null && x.IdEmployee != null && !string.IsNullOrEmpty(x.IdEmployee) && x.IdEmployee.Equals(employee.Id))
                                .Count();

                            int IdGroupPosition = (await ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value)).IdGroupPositions.Value;
                            int countOfTestsToPurpose = 0;
                            if (IdGroupPosition != 0)
                            {
                                countOfTestsToPurpose = (await ms.CompetenciesForGroup.GetAllCompetenciesForGroups())
                                    .Where(x => x != null && x.IdGroupPositions != null && x.IdGroupPositions.Equals(IdGroupPosition))
                                    .Count();
                            }
                            models.Add(new GetEmployeeModel
                            {
                                Id = employee.Id,
                                FirstName = employee.FirstName,
                                SecondName = employee.SecondName,
                                LastName = employee.LastName,
                                Login = employee.Login,
                                Password = employee.Password,
                                Phone = employee.Phone,
                                DateOfBirth = employee.DateOfBirth.ToString(),
                                IdSubdivision = employee.IdSubdivision,
                                RegistrationDate = employee.RegistrationDate.ToString(),
                                CountOfPurposes = countOfPurposes,
                                CountOfResults = countOfResults,
                                CountOfTestsToPurpose = countOfTestsToPurpose,
                            });
                        }
                        return Ok(models);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Employee" })]
        [HttpGet("GetEmployeesPage")]
        public async Task<IActionResult> GetEmployeesPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetEmployees ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetEmployees",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        var Allemployees = (await ms.Employee.GetAllEmployees());

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allemployees.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var employees = Allemployees
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        List<GetEmployeeModel> models = new List<GetEmployeeModel>();
                        foreach (var employee in employees)
                        {
                            int countOfPurposes = (await ms.TestPurpose.GetAllTestPurposes())
                                .Where(x => x != null && x.IdEmployee != null && !string.IsNullOrEmpty(x.IdEmployee) && x.IdEmployee.Equals(employee.Id))
                                .Count();

                            int countOfResults = (await ms.EmployeeResult.GetAllEmployeeResults())
                                .Where(x => x != null && x.IdEmployee != null && !string.IsNullOrEmpty(x.IdEmployee) && x.IdEmployee.Equals(employee.Id))
                                .Count();

                            int IdGroupPosition = (await ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value)).IdGroupPositions.Value;
                            int countOfTestsToPurpose = 0;
                            if (IdGroupPosition != 0)
                            {
                                countOfTestsToPurpose = (await ms.CompetenciesForGroup.GetAllCompetenciesForGroups())
                                   .Where(x => x != null && x.IdGroupPositions != null && x.IdGroupPositions.Equals(IdGroupPosition))
                                   .Count();
                            }
                            models.Add(new GetEmployeeModel
                            {
                                Id = employee.Id,
                                FirstName = employee.FirstName,
                                SecondName = employee.SecondName,
                                LastName = employee.LastName,
                                Login = employee.Login,
                                Password = employee.Password,
                                Phone = employee.Phone,
                                DateOfBirth = employee.DateOfBirth.ToString(),
                                IdSubdivision = employee.IdSubdivision,
                                RegistrationDate = employee.RegistrationDate.ToString(),
                                CountOfPurposes = countOfPurposes,
                                CountOfResults = countOfResults,
                                CountOfTestsToPurpose = countOfTestsToPurpose,
                            });
                        }

                        return Ok(models);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Employee" })]
        [HttpPost("GetEmployee")]
        public async Task<IActionResult> GetEmployee([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetEmployee :id={id.Id}");

                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetEmployee",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Сотрудника={id.Id}"
                        });
                        Employee? employee = await ms.Employee.GetEmployeeById(id.Id); ;
                        if (employee != null)
                        {
                            int countOfPurposes = (await ms.TestPurpose.GetAllTestPurposes())
                                .Where(x => x != null && x.IdEmployee != null && x.IdEmployee.Equals(employee.Id))
                                .Count();

                            int countOfResults = (await ms.EmployeeResult.GetAllEmployeeResults())
                                .Where(x => x != null && x.IdEmployee != null && x.IdEmployee.Equals(employee.Id))
                                .Count();

                            int IdGroupPosition = (await ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value)).IdGroupPositions.Value;
                            int countOfTestsToPurpose = (await ms.CompetenciesForGroup.GetAllCompetenciesForGroups())
                                .Where(x => x != null && x.IdGroupPositions != null && x.IdGroupPositions.Equals(IdGroupPosition))
                                .Count();

                            GetEmployeeModel model = new GetEmployeeModel
                            {
                                Id = employee.Id,
                                FirstName = employee.FirstName,
                                SecondName = employee.SecondName,
                                LastName = employee.LastName,
                                Login = employee.Login,
                                Password = employee.Password,
                                Phone = employee.Phone,
                                DateOfBirth = employee.DateOfBirth.ToString(),
                                IdSubdivision = employee.IdSubdivision,
                                RegistrationDate = employee.RegistrationDate.ToString(),
                                CountOfPurposes = countOfPurposes,
                                CountOfResults = countOfResults,
                                CountOfTestsToPurpose = countOfTestsToPurpose,
                            };

                            return Ok(model);
                        }
                        return NotFound(new { message = "Сотрудник не найден" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Employee" })]
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromHeader] string Authorization, [FromBody] AddEmployeeModel? employee)
        {
            if (!Authorization.IsNullOrEmpty() &&
                employee != null && !string.IsNullOrEmpty(employee.FirstName) &&
                !string.IsNullOrEmpty(employee.SecondName) && !string.IsNullOrEmpty(employee.LastName) &&
                !string.IsNullOrEmpty(employee.Login) && !string.IsNullOrEmpty(employee.Password) &&
                employee.IdSubdivision.HasValue && employee.IdSubdivision != 0 &&
                !employee.Phone.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddEmployee :fn={employee.FirstName}, sn={employee.SecondName}, " +
                                               $" ln={employee.LastName}, idSubdivision={employee.IdSubdivision}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddEmployee",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Имя сотрудника ={employee.FirstName}, фамилия={employee.SecondName}, отчество={employee.LastName}"
                        });

                        if (await ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value) == null)
                        {
                            return BadRequest(new { message = "Ошибка. Такого отдела нет" });
                        }

                        if(await ms.Employee.GetEmployeeByLogin(employee.Login) != null)
                            return BadRequest(new { message = "Ошибка. Пользователь с таким логином уже зарегестрирован" });

                        await ms.Employee.SaveEmployee(employee);
                        //await notificationHub.Clients.All.ReceiveMessage($"{DateTime.Now} Зарегестрирован новый пользователь");
                        await notificationHub.Clients.Group(Roles.ADMIN.ToString()).EmployeeNotification($"{DateTime.Now} Зарегестрирован новый пользователь '{employee.SecondName} {employee.FirstName}'");
                        return Ok(new { message = "Сотрудник добавлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Employee" })]
        [HttpPost("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromHeader] string Authorization, [FromBody] UpdateEmployeeModel? employee)
        {
            if (!Authorization.IsNullOrEmpty() && employee != null && !string.IsNullOrEmpty(employee.Id) &&
                !string.IsNullOrEmpty(employee.FirstName) && !string.IsNullOrEmpty(employee.SecondName) &&
                !string.IsNullOrEmpty(employee.LastName) && !string.IsNullOrEmpty(employee.Login) &&
                !string.IsNullOrEmpty(employee.Password) && employee.IdSubdivision.HasValue &&
                !employee.Phone.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if (await ms.Employee.GetEmployeeById(employee.Id) != null)
                        {
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/UpdateEmployee",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id сотрудника={employee.Id}, Имя сотрудника ={employee.FirstName}, фамилия={employee.SecondName}, отчество={employee.LastName}"
                            });
                            if (await ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value) == null)
                                return BadRequest(new { message = "Ошибка. Такой должности нет" });
                            Employee? checkEmployee = await ms.Employee.GetEmployeeByLoginOnUpdate(employee.Login, employee.Id);
                            if (checkEmployee != null && !checkEmployee.Id.Equals(employee.Id))
                                return BadRequest(new { message = "Ошибка. Пользователь с таким логином уже зарегестрирован" });

                            await ms.Employee.SaveEmployee(employee);
                            //await notificationHub.Clients.All.ReceiveMessage($"{DateTime.Now} Пользователь '{employee.SecondName} {employee.FirstName}' обновлен");
                            await notificationHub.Clients.Group(Roles.ADMIN.ToString()).EmployeeNotification($"{DateTime.Now} Пользователь '{employee.SecondName} {employee.FirstName}' обновлен");
                            return Ok(new { message = "Сотрудник обновлен" });
                        }
                        else
                        {
                            return BadRequest(new { message = "Ошибка. Такого пользователя не существует" });
                        }
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Employee" })]
        [HttpPost("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteEmployee :id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteEmployee",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Сотрудника={id.Id}"
                        });
                        Employee? employee = await ms.Employee.GetEmployeeById(id.Id);
                        if (employee != null)
                        {
                            await ms.Employee.DeleteEmployeeById(id.Id);
                            //await notificationHub.Clients.All.ReceiveMessage($"{DateTime.Now} Пользователь '{employee.SecondName} {employee.FirstName}' удален");
                            await notificationHub.Clients.Group(Roles.ADMIN.ToString()).EmployeeNotification($"{DateTime.Now} Пользователь '{employee.SecondName} {employee.FirstName}' удален");
                            return Ok(new { message = "Сотрудник удален" });
                        }
                        return NotFound(new { message = "Ошибка. Пользователь не найден" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  Message
         */
        [SwaggerOperation(Tags = new[] { "Admin/Message" })]
        [HttpGet("GetMessages")]
        public async Task<IActionResult> GetMessages([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetSMessages");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetSMessages",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        var messages = await ms.Message.GetAllMessageDtos();
                        return Ok(messages.OrderByDescending(x => x.Id));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Message" })]
        [HttpGet("GetMessagesPage")]
        public async Task<IActionResult> GetMessagesPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetMessagesPage");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetMessagesPage",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });

                        var Allmessages= (await ms.Message.GetAllMessageDtos())
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

        [SwaggerOperation(Tags = new[] { "Admin/Message" })]
        [HttpPost("GetMessage")]
        public async Task<IActionResult> GetMessage([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetMessage :Id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetMessage",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id сообщения ={id.Id}"
                        });
                        MessageDto? dto = await ms.Message.GetMessageDtoById(id.Id.Value);
                        if (dto != null)
                        {
                            return Ok(dto);
                        }
                        return NotFound(new { message = "Ошибка. Сообщение не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [SwaggerOperation(Tags = new[] { "Admin/Message" })]
        [HttpPost("SendMessageToEmployee")]
        public async Task<IActionResult> SendMessageToEmployee([FromHeader] string Authorization, [FromBody] AddMessageModel? message)
        {
            if (!Authorization.IsNullOrEmpty() && message != null && !message.IdEmployee.IsNullOrEmpty() && !message.MessageText.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/SendMessageToEmployee");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/SendMessageToEmployee",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id сотрудника={message.IdEmployee}"
                        });

                        TokenEmployee? employeeToken = await ms.TokenEmployee.GetTokenEmployeeByEmployeeId(message.IdEmployee);
                        if(employeeToken != null && !employeeToken.ConnectionId.IsNullOrEmpty())
                        {
                            //await ms.Subcompetence.SaveSubcompetence(subcompetence);
                            //await notificationHub.Clients.All.ConfigurationNotification($"Test message to all connections.{DateTime.Now}");
                            //await notificationHub.Clients.Client(message.IdEmployee).MessageNotificationToEmployee(message.IdEmployee, DateTime.Now.ToString() + " " +  message.MessageText);
                            //await notificationHub.Clients.Group(Roles.EMPLOYEE.ToString()).MessageNotificationToEmployee(message.IdEmployee, DateTime.Now.ToString() + " " + message.MessageText);
                            await notificationHub.Clients.Client(employeeToken.ConnectionId).MessageNotificationToEmployee(message.IdEmployee, DateTime.Now.ToString() + " " + message.MessageText);
                            return Ok(new { message = "Сообщение отправлено" });
                        }
                        return BadRequest(new { message = "Ошибка. Пользователь не подключен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Message" })]
        [HttpPost("ChangeMessageStatus")]
        public async Task<IActionResult> ChangeMessageStatus([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue )
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/ChangeMessageStatus :Id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/ChangeMessageStatus",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id сообщения={id.Id}"
                        });
                        if (await ms.Message.GetMessageById(id.Id.Value) != null)
                        {
                            await ms.Message.ChangeMessageStatus(id.Id.Value);
                            return Ok(new { message = "Статус сообщения обновлен" });
                        }
                        return NotFound(new { message = "Ошибка. Сообщение не найдено" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Message" })]
        [HttpPost("UpdateMessage")]
        public async Task<IActionResult> UpdateMessage([FromHeader] string Authorization, [FromBody] MessageDto? messageDto)
        {
            if (!Authorization.IsNullOrEmpty() && messageDto != null &&
                messageDto.Id.HasValue && !messageDto.IdEmployee.IsNullOrEmpty() && messageDto.MessageText.IsNullOrEmpty() &&
                messageDto.StatusRead.HasValue && !messageDto.DateAndTime.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateMessage :Id={messageDto.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateMessage",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id сообщения={messageDto.Id}"
                        });
                        if (await ms.Message.GetMessageById(messageDto.Id.Value) != null)
                        {
                            await ms.Message.SaveMessage(messageDto);
                            return Ok(new { message = "Подкомпетенция обновлена" });
                        }
                        return NotFound(new { message = "Ошибка. Подкомпетенция не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Message" })]
        [HttpPost("DeleteMessage")]
        public async Task<IActionResult> DeleteMessage([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteMessage :messageId={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteMessage",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id сообщения={id.Id}"
                        });
                        if (id.Id.HasValue && id.Id != 0 && await ms.Message.GetMessageById(id.Id.Value) != null)
                        {
                            await ms.Message.DeleteMessageById(id.Id.Value);
                            return Ok(new { message = "Сообщение удалено" });
                        }
                        return NotFound(new { message = "Ошибка. Сообщение не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  Admin
         */
        [SwaggerOperation(Tags = new[] { "Admin/Admin" })]
        [HttpGet("GetAdmins")]
        public async Task<IActionResult> GetAdmins([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetAdmins ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetAdmins",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        return Ok(await ms.Admin.GetAllAdminDtos());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Admin" })]
        [HttpGet("GetAdminsPage")]
        public async Task<IActionResult> GetAdminsPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetAdmins ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetAdmins",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        var Alladmins = (await ms.Admin.GetAllAdminDtos());

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Alladmins.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var admins = Alladmins
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(admins);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Admin" })]
        [HttpPost("GetAdmin")]
        public async Task<IActionResult> GetAdmin([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetAdmin ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetAdmin",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Администратора={id.Id}"
                        });
                        if (await ms.Admin.GetAdminById(id.Id) != null)
                            return Ok(await ms.Admin.GetAdminDtoById(id.Id));
                        return NotFound(new { message = "Ошибка. Такого администратора нет" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Admin" })]
        [HttpPost("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin([FromHeader] string Authorization, [FromBody] AdminDto? admin)
        {
            if (!Authorization.IsNullOrEmpty() && admin != null && !string.IsNullOrEmpty(admin.Id) &&
                !string.IsNullOrEmpty(admin.FirstName) && !string.IsNullOrEmpty(admin.SecondName) && !string.IsNullOrEmpty(admin.LastName) &&
                !string.IsNullOrEmpty(admin.Login) && !string.IsNullOrEmpty(admin.Password))// && admin.IdSubdivision.HasValue && admin.IdSubdivision > 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateAdmin ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateAdmin",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Администратора={admin.Id}"
                        });
                        if (await ms.Admin.GetAdminById(admin.Id) != null)
                        {
                            Employee? checkEmployee = await ms.Employee.GetEmployeeByLoginOnUpdate(admin.Login, admin.Id);
                            if (checkEmployee != null && !checkEmployee.Id.Equals(admin.Id))
                                return BadRequest(new { message = "Ошибка. Пользователь с таким логином уже зарегестрирован" });

                            await ms.Admin.SaveAdmin(admin);
                            return Ok(new { message = "Администратор добавлен" });
                        }
                        else
                        {
                            return NotFound(new { message = "Ошибка. Такого пользователя не существует" });
                        }
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [SwaggerOperation(Tags = new[] { "Admin/Admin" })]
        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin([FromHeader] string Authorization, [FromBody] AddAdminModel? admin)
        {
            if (!Authorization.IsNullOrEmpty() && admin != null &&
                !string.IsNullOrEmpty(admin.FirstName) && !string.IsNullOrEmpty(admin.SecondName) && !string.IsNullOrEmpty(admin.LastName) &&
                !string.IsNullOrEmpty(admin.Login) && !string.IsNullOrEmpty(admin.Password))//&& admin.IdSubdivision.HasValue && admin.IdSubdivision > 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddAdmin ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddAdmin",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Логин Администратора={admin.Login}"
                        });

                        if (await ms.Employee.GetEmployeeByLogin(admin.Login) != null)
                            return BadRequest(new { message = "Ошибка. Пользователь с таким логином уже зарегестрирован" });


                        await ms.Admin.SaveAdmin(admin);
                        return Ok(new { message = "Администратор добавлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [SwaggerOperation(Tags = new[] { "Admin/Admin" })]
        [HttpPost("DeleteAdmin")]
        public async Task<IActionResult> DeleteAdmin([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteAdmin ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteAdmin",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Администратора={id.Id}"
                        });

                        if (id.Id.Equals(token.IdAdmin))
                            return BadRequest(new { message = "Ошибка. Вы не можете удалить свою учетную запись" });

                        Admin? adminToDelete = await ms.Admin.GetAdminById(id.Id);
                        if (adminToDelete == null)
                            return NotFound(new { message = "Администратор не найден" });

                        await ms.Admin.DeleteAdminById(adminToDelete.Id);
                        return Ok(new { message = "Администратор удален" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  Competence
         */
        [SwaggerOperation(Tags = new[] { "Admin/Competence" })]
        [HttpGet("GetCompetences")]
        public async Task<IActionResult> GetCompetences([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetences");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetences",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        return Ok(await ms.TestType.GetAllCompetenceDtos());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Competence" })]
        [HttpGet("GetCompetencesPage")]
        public async Task<IActionResult> GetCompetencesPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetences");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetences",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });

                        var Alcompetences = (await ms.TestType.GetAllCompetenceDtos())
                                                    .OrderBy(x => x.Id);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Alcompetences.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var competences = Alcompetences
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(competences);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Competence" })]
        [HttpPost("GetCompetence")]
        public async Task<IActionResult> GetCompetence([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetence :Id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции теста={id.Id}"
                        });
                        if (await ms.TestType.GetCompetenceById(id.Id.Value) != null)
                        {
                            CompetenceDto? competenceDto = await ms.TestType.GetCompetenceDtoById(id.Id.Value);
                            return Ok(competenceDto);
                        }
                        return NotFound(new { message = "Ошибка. Компетенция не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [SwaggerOperation(Tags = new[] { "Admin/Competence" })]
        [HttpPost("AddCompetence")]
        public async Task<IActionResult> AddCompetence([FromHeader] string Authorization, [FromBody] AddCompetenceModel? competence)
        {
            if (!Authorization.IsNullOrEmpty() && !competence.Name.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddCompetence :Name={competence.Name}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddCompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Название компетенции теста={competence.Name}"
                        });
                        await ms.TestType.SaveCompetence(competence);
                        return Ok(new { message = "Компетенция добавлена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Competence" })]
        [HttpPost("UpdateCompetence")]
        public async Task<IActionResult> UpdateCompetence([FromHeader] string Authorization, [FromBody] CompetenceDto? competence)
        {
            if (!Authorization.IsNullOrEmpty() && competence != null &&
                competence.Id.HasValue && !competence.Name.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateCompetence :Id={competence.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateCompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции теста={competence.Id}, Название={competence.Name}"
                        });
                        if (await ms.TestType.GetCompetenceById(competence.Id.Value) != null)
                        {
                            await ms.TestType.SaveCompetence(competence);
                            return Ok(new { message = "Компетенция обновлена" });
                        }
                        return NotFound(new { message = "Ошибка. Компетенция не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Competence" })]
        [HttpPost("DeleteCompetence")]
        public async Task<IActionResult> DeleteCompetence([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteCompetence :competenceId={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteCompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции теста={id.Id}"
                        });
                        if (id.Id.HasValue && id.Id != 0 && await ms.TestType.GetCompetenceById(id.Id.Value) != null)
                        {
                            await ms.TestType.DeleteCompetenceById(id.Id.Value);
                            return Ok(new { message = "Компетенция удалена" });
                        }
                        return NotFound(new { message = "Ошибка. Компетенция не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  Competence
         */
        [SwaggerOperation(Tags = new[] { "Admin/CompetenceCoeff" })]
        [HttpGet("GetCompetenceCoeffs")]
        public async Task<IActionResult> GetCompetenceCoeffs([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetenceCoeffs");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetenceCoeffs",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        return Ok(await ms.CompetenceCoeff.GetAllCompetenceCoeffDtos());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenceCoeff" })]
        [HttpGet("GetCompetenceCoeffsPage")]
        public async Task<IActionResult> GetCompetenceCoeffsPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetenceCoeffsPage");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetenceCoeffsPage",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });

                        var AlcompetenceCoeffs = (await ms.CompetenceCoeff.GetAllCompetenceCoeffDtos())
                                                    .OrderBy(x => x.Id);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, AlcompetenceCoeffs.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var competenceCoeffs = AlcompetenceCoeffs
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(competenceCoeffs);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenceCoeff" })]
        [HttpPost("GetCompetenceCoeffsByCompetenceAndGroupId")]
        public async Task<IActionResult> GetCompetenceCoeffsByCompetenceAndGroupId([FromHeader] string Authorization, [FromBody] QuerryCompetenceCoeffModel? model)
        {
            if (!Authorization.IsNullOrEmpty() && model != null && model.IdCompetence.HasValue && model.IdGroup.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetenceCoeffs");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetenceCoeffs",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        if (await ms.TestType.GetCompetenceById(model.IdCompetence.Value) == null)
                            return BadRequest(new { message = "Ошибка. Такой компетенции нет" });

                        if (await ms.GroupPosition.GetGroupPositionById(model.IdGroup.Value) == null)
                            return BadRequest(new { message = "Ошибка. Такой группы нет" });

                        return Ok(await ms.CompetenceCoeff.GetCompetenceCoeffDtoByCompetenceAndGroupId(model.IdCompetence.Value, model.IdGroup.Value));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenceCoeff" })]
        [HttpPost("GetCompetenceCoeffsByGroupId")]
        public async Task<IActionResult> GetCompetenceCoeffsByGroupId([FromHeader] string Authorization, [FromBody] IntIdModel? model)
        {
            if (!Authorization.IsNullOrEmpty() && model != null && model.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetenceCoeffs");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetenceCoeffs",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        if (await ms.GroupPosition.GetGroupPositionById(model.Id.Value) == null)
                            return BadRequest(new { message = "Ошибка. Такой группы нет" });

                        return Ok(await ms.CompetenceCoeff.GetCompetenceCoeffDtosByGroupId(model.Id.Value));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenceCoeff" })]
        [HttpPost("GetCompetenceCoeff")]
        public async Task<IActionResult> GetCompetenceCoeff([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetenceCoeff :Id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetenceCoeff",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции коэффицента={id.Id}"
                        });
                        if (await ms.CompetenceCoeff.GetCompetenceCoeffById(id.Id.Value) != null)
                        {
                            CompetenceCoeffDto? competenceCoeffDto = await ms.CompetenceCoeff.GetCompetenceCoeffDtoById(id.Id.Value);
                            return Ok(competenceCoeffDto);
                        }
                        return NotFound(new { message = "Ошибка. Коэффицент компетенции не найден" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenceCoeff" })]
        [HttpPost("AddCompetenceCoeff")]
        public async Task<IActionResult> AddCompetenceCoeff([FromHeader] string Authorization, [FromBody] AddCompetenceCoeffModel? model)
        {
            if (!Authorization.IsNullOrEmpty() && model.IdCompetence.HasValue && model.IdGroup.HasValue 
                && model.Coefficient.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddCompetenceCoeff");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddCompetenceCoeff",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции теста={model.IdCompetence}"
                        });

                        if(await ms.TestType.GetCompetenceById(model.IdCompetence.Value) == null)
                            return BadRequest(new { message = "Ошибка. Такой компетенции нет" });

                        if (await ms.GroupPosition.GetGroupPositionById(model.IdGroup.Value) == null)
                            return BadRequest(new { message = "Ошибка. Такой группы нет" });

                        await ms.CompetenceCoeff.SaveCompetenceCoeff(model);
                        return Ok(new { message = "Кожффицент компетенции добавлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenceCoeff" })]
        [HttpPost("UpdateCompetenceCoeff")]
        public async Task<IActionResult> UpdateCompetence([FromHeader] string Authorization, [FromBody] CompetenceCoeffDto? competence)
        {
            if (!Authorization.IsNullOrEmpty() && competence != null &&
                competence.Id.HasValue && competence.IdCompetence.HasValue && 
                competence.IdGroup.HasValue && competence.Coefficient.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateCompetenceCoeff");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateCompetenceCoeff",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции ={competence.Id}"
                        });

                        if(await ms.GroupPosition.GetGroupPositionById(competence.IdGroup.Value) != null)
                            return NotFound(new { message = "Ошибка. Группа не найдена" });
                        if (await ms.TestType.GetCompetenceById(competence.Id.Value) != null)
                            return NotFound(new { message = "Ошибка. Компетенция не найдена" });

                        await ms.CompetenceCoeff.SaveCompetenceCoeff(competence);
                        return Ok(new { message = "Коэффицент компетенция обновлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenceCoeff" })]
        [HttpPost("DeleteCompetenceCoeff")]
        public async Task<IActionResult> DeleteCompetenceCoeff([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteCompetenceCoeff");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteCompetenceCoeff",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id коэффицента={id.Id}"
                        });
                        if (id.Id.HasValue && id.Id != 0 && await ms.CompetenceCoeff.GetCompetenceCoeffById(id.Id.Value) != null)
                        {
                            await ms.TestType.DeleteCompetenceById(id.Id.Value);
                            return Ok(new { message = "Коэффицент удален" });
                        }
                        return NotFound(new { message = "Ошибка. Коэфицент не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  Subcompetence
         */
        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpGet("GetSubcompetences")]
        public async Task<IActionResult> GetSubcompetences([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetSubcompetences");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetSubcompetences",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        return Ok(await ms.Subcompetence.GetAllSubcompetenceDtos());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpGet("GetSubcompetencesPage")]
        public async Task<IActionResult> GetSubcompetencesPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetSubcompetences");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetSubcompetences",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });

                        var Alsubcompetences = (await ms.Subcompetence.GetAllSubcompetenceDtos())
                                                    .OrderBy(x => x.Id);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Alsubcompetences.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var subcompetences = Alsubcompetences
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(subcompetences);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpPost("GetSubcompetence")]
        public async Task<IActionResult> GetSubcompetence([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetSubcompetence :Id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id подкомпетенции ={id.Id}"
                        });
                        SubcompetenceDto? dto = await ms.Subcompetence.GetSubcompetenceDtoById(id.Id.Value);
                        if (dto != null)
                        {
                            return Ok(dto);
                        }
                        return NotFound(new { message = "Ошибка. Подкомпетенция не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpPost("AddSubcompetence")]
        public async Task<IActionResult> AddSubcompetence([FromHeader] string Authorization, [FromBody] AddSubcompetenceModel? subcompetence)
        {
            if (!Authorization.IsNullOrEmpty() && subcompetence != null && !subcompetence.Name.IsNullOrEmpty() && !subcompetence.Name.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddSubompetence :Name={subcompetence.Name}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Название подкомпетенции теста={subcompetence.Name}"
                        });
                        await ms.Subcompetence.SaveSubcompetence(subcompetence);
                        return Ok(new { message = "Подкомпетенция добавлена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpPost("UpdateSubcompetence")]
        public async Task<IActionResult> UpdateSubcompetence([FromHeader] string Authorization, [FromBody] SubcompetenceDto? subcompetence)
        {
            if (!Authorization.IsNullOrEmpty() && subcompetence != null &&
                subcompetence.Id.HasValue && !subcompetence.Name.IsNullOrEmpty() && subcompetence.Description.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateSubcompetence :Id={subcompetence.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id подкомпетенции теста={subcompetence.Id}, Название={subcompetence.Name}"
                        });
                        if (await ms.TestType.GetCompetenceById(subcompetence.Id.Value) != null)
                        {
                            await ms.Subcompetence.SaveSubcompetence(subcompetence);
                            return Ok(new { message = "Подкомпетенция обновлена" });
                        }
                        return NotFound(new { message = "Ошибка. Подкомпетенция не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpPost("DeleteSubcompetence")]
        public async Task<IActionResult> DeleteSubcompetence([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteSubcompetence :competenceId={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id подкомпетенции теста={id.Id}"
                        });
                        if (id.Id.HasValue && id.Id != 0 && await ms.Subcompetence.GetSubcompetenceById(id.Id.Value) != null)
                        {
                            await ms.Subcompetence.DeleteSubcompetenceById(id.Id.Value);
                            return Ok(new { message = "Подкомпетенция удалена" });
                        }
                        return NotFound(new { message = "Ошибка. Подкомпетенция не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        /*
         *  SubcompetenceScore
         */
        [SwaggerOperation(Tags = new[] { "Admin/SubcompetenceScore" })]
        [HttpGet("GetSubcompetenceScores")]
        public async Task<IActionResult> GetSubcompetenceScores([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetSubcompetenceScores");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetSubcompetenceScores",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        return Ok(await ms.SubcompetenceScore.GetAllSubcompetenceScoreDtos());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/SubcompetenceScore" })]
        [HttpPost("GetSubcompetenceScoresBySubcompetence")]
        public async Task<IActionResult> GetSubcompetenceScoresBySubcompetence([FromHeader] string Authorization, [FromBody] IntIdModel id)
        {
            if (!Authorization.IsNullOrEmpty() && id !=null && id.Id > 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetSubcompetenceScores");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetSubcompetenceScores",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        return Ok(await ms.SubcompetenceScore.GetSubcompetenceScoreDtosBySubCompetence(id.Id.Value));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/SubcompetenceScore" })]
        [HttpGet("GetSubcompetenceScoresPage")]
        public async Task<IActionResult> GetSubcompetenceScoresPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetSubcompetenceScoresPage");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetSubcompetenceScores",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });

                        var Alsubcompetences = (await ms.SubcompetenceScore.GetAllSubcompetenceScoreDtos())
                                                    .OrderBy(x => x.Id);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Alsubcompetences.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var subcompetences = Alsubcompetences
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(subcompetences);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/SubcompetenceScore" })]
        [HttpPost("GetSubcompetenceScore")]
        public async Task<IActionResult> GetSubcompetenceScore([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetSubcompetenceScore :Id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetSubcompetenceScore",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id ={id.Id}"
                        });
                        SubcompetenceScoreDto? dto = await ms.SubcompetenceScore.GetSubcompetenceScoreDtoById(id.Id.Value);
                        if (dto != null)
                        {
                            return Ok(dto);
                        }
                        return NotFound(new { message = "Ошибка. Оценка подкомпетенции не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [SwaggerOperation(Tags = new[] { "Admin/SubcompetenceScore" })]
        [HttpPost("AddSubcompetenceScore")]
        public async Task<IActionResult> AddSubcompetenceScore([FromHeader] string Authorization, [FromBody] AddSubcompetenceScoreModel? model)
        {
            if (!Authorization.IsNullOrEmpty() && model != null && 
                model.MinValue.HasValue && model.MaxValue.HasValue &&
                !model.Description.IsNullOrEmpty() && model.IdSubcompetence.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddSubcompetenceScore");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddSubcompetenceScore",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"id подкомпентенции={model.IdSubcompetence}"
                        });
                        await ms.SubcompetenceScore.SaveSubcompetenceScore(model);
                        return Ok(new { message = "Оценка подкомпетенции добавлена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/SubcompetenceScore" })]
        [HttpPost("UpdateSubcompetenceScore")]
        public async Task<IActionResult> UpdateSubcompetenceScore([FromHeader] string Authorization, [FromBody] SubcompetenceScoreDto? model)
        {
            if (!Authorization.IsNullOrEmpty() && model != null &&
                model.Id.HasValue && model.Id > 0 && 
                model.MinValue.HasValue && model.MaxValue.HasValue &&
                !model.Description.IsNullOrEmpty() && model.IdSubcompetence.HasValue && model.IdSubcompetence > 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateSubcompetenceScore");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateSubcompetenceScore",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id подкомпетенции теста={model.IdSubcompetence}"
                        });
                        if (await ms.Subcompetence.GetSubcompetenceById(model.IdSubcompetence.Value) != null)
                        {
                            await ms.SubcompetenceScore.SaveSubcompetenceScore(model);
                            return Ok(new { message = "Оценка подкомпетенции обновлена" });
                        }
                        return NotFound(new { message = "Ошибка. Оценка подкомпетенции не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/SubcompetenceScore" })]
        [HttpPost("DeleteSubcompetenceScore")]
        public async Task<IActionResult> DeleteSubcompetenceScore([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue && id.Id > 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteSubcompetence :competenceId={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id подкомпетенции теста={id.Id}"
                        });
                        if (await ms.SubcompetenceScore.GetSubcompetenceScoreById(id.Id.Value) != null)
                        {
                            await ms.SubcompetenceScore.DeleteSubcompetenceScoreById(id.Id.Value);
                            return Ok(new { message = "Подкомпетенция удалена" });
                        }
                        return NotFound(new { message = "Ошибка. Подкомпетенция не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        /*
         *  QuestionSubcompetence
         */
        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpGet("GetQuestionSubcompetences")]
        public async Task<IActionResult> GetQuestionSubcompetences([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetQuestionSubcompetences");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetQuestionSubcompetences",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        return Ok(await ms.QuestionSubcompetence.GetAllQuestionSubcompetenceDtos());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpGet("GetQuestionSubcompetencesByQuestion")]
        public async Task<IActionResult> GetQuestionSubcompetencesByQuestion([FromHeader] string Authorization, [FromBody] StringIdModel id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetQuestionSubcompetences");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetQuestionSubcompetences",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        return Ok(await ms.QuestionSubcompetence.GetAllQuestionSubcompetenceDtosByQuestion(id.Id));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpGet("GetQuestionSubcompetencePage")]
        public async Task<IActionResult> GetQuestionSubcompetencePage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetQuestionSubcompetencePage");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetQuestionSubcompetencePage",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });

                        var allQuestionSubcompetences = (await ms.QuestionSubcompetence.GetAllQuestionSubcompetenceDtos())
                                                    .OrderBy(x => x.Id);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, allQuestionSubcompetences.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var questionSubcompetences = allQuestionSubcompetences
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(questionSubcompetences);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpPost("GetQuestionSubcompetence")]
        public async Task<IActionResult> GetQuestionSubcompetence([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetQuestionSubcompetence :Id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetQuestionSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id подкомпетенции вопроса ={id.Id}"
                        });
                        QuestionSubcompetenceDto? dto = await ms.QuestionSubcompetence.GetQuestionSubcompetenceDtoById(id.Id.Value);
                        if (dto != null)
                        {
                            return Ok(dto);
                        }
                        return NotFound(new { message = "Ошибка. Подкомпетенция вопроса не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpPost("AddQuestionSubcompetence")]
        public async Task<IActionResult> AddQuestionSubcompetence([FromHeader] string Authorization, [FromBody] AddQuestionSubcompetenceModel? questionSubcompetence)
        {
            if (!Authorization.IsNullOrEmpty() && questionSubcompetence != null && !questionSubcompetence.IdQuestion.IsNullOrEmpty() && questionSubcompetence.IdSubcompetence.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddQuestionSubcompetence :idQuestion={questionSubcompetence.IdQuestion}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddQuestionSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"id вопроса={questionSubcompetence.IdQuestion}"
                        });
                        await ms.QuestionSubcompetence.SaveQuestionSubcompetence(questionSubcompetence);
                        return Ok(new { message = "Подкомпетенция вопроса добавлена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpPost("UpdateQuestionSubcompetence")]
        public async Task<IActionResult> UpdateQuestionSubcompetence([FromHeader] string Authorization, [FromBody] QuestionSubcompetenceDto? questionSubcompetence)
        {
            if (!Authorization.IsNullOrEmpty() && questionSubcompetence != null &&
                questionSubcompetence.Id.HasValue && !questionSubcompetence.IdQuestion.IsNullOrEmpty() && questionSubcompetence.IdSubcompetence.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateQuestionSubcompetence :Id={questionSubcompetence.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateQuestionSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id подкомпетенции вопроса={questionSubcompetence.Id}"
                        });
                        if (await ms.TestType.GetCompetenceById(questionSubcompetence.Id.Value) != null)
                        {
                            await ms.QuestionSubcompetence.SaveQuestionSubcompetence(questionSubcompetence);
                            return Ok(new { message = "Подкомпетенция вопроса обновлена" });
                        }
                        return NotFound(new { message = "Ошибка. Подкомпетенция вопроса не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Subcompetence" })]
        [HttpPost("DeleteQuestionSubcompetence")]
        public async Task<IActionResult> DeleteQuestionSubcompetence([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteQuestionSubcompetence :questionSubcompetenceId={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteQuestionSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id подкомпетенции вопроса={id.Id}"
                        });
                        if (id.Id.HasValue && id.Id != 0 && await ms.QuestionSubcompetence.GetQuestionSubcompetenceById(id.Id.Value) != null)
                        {
                            await ms.QuestionSubcompetence.DeleteQuestionSubcompetenceById(id.Id.Value);
                            return Ok(new { message = "Подкомпетенция удалена" });
                        }
                        return NotFound(new { message = "Ошибка. Подкомпетенция вопроса не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  CompetenciesForGroup
         */
        [SwaggerOperation(Tags = new[] { "Admin/CompetenciesForGroup" })]
        [HttpGet("GetCompetenciesForGroups")]
        public async Task<IActionResult> GetCompetenciesForGroups([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetenciesForGroups");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetenciesForGroups",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        return Ok(await ms.CompetenciesForGroup.GetAllCompetenciesForGroupDtos());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenciesForGroup" })]
        [HttpGet("GetCompetenciesForGroupsPage")]
        public async Task<IActionResult> GetCompetenciesForGroupsPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetenciesForGroups");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetenciesForGroups",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });

                        var AllcompsForGroups = (await ms.CompetenciesForGroup.GetAllCompetenciesForGroupDtos())
                                                    .OrderBy(x => x.Id);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, AllcompsForGroups.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var compsForGroups = AllcompsForGroups
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(compsForGroups);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenciesForGroup" })]
        [HttpPost("GetCompetenciesForGroup")]
        public async Task<IActionResult> GetCompetenciesForGroup([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetenciesForGroup :Id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetenciesForGroup",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции теста={id.Id}"
                        });
                        if (await ms.CompetenciesForGroup.GetCompetenciesForGroupById(id.Id.Value) != null)
                        {
                            CompetenciesForGroupDto? dto = (await ms.CompetenciesForGroup.GetCompetenciesForGroupDtoById(id.Id.Value));
                            return Ok(dto);
                        }
                        return NotFound(new { message = "Ошибка. Компетенция для группы не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenciesForGroup" })]
        [HttpPost("AddCompetenciesForGroup")]
        public async Task<IActionResult> AddCompetenciesForGroup([FromHeader] string Authorization, [FromBody] AddCompetenciesForGroupModel? model)
        {
            if (!Authorization.IsNullOrEmpty() && !string.IsNullOrEmpty(model.IdTest) && model.IdGroupPositions != 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if ((await ms.CompetenciesForGroup.GetAllCompetenciesForGroups()).Find(x => x.IdTest.Equals(model.IdTest) && x.IdGroupPositions.Equals(model.IdGroupPositions.Value)) != null)
                        {
                            return NotFound(new { message = "Ошибка. Тест уже назначен этой группе" });
                        }
                        logger.LogInformation($"/admin-api/AddCompetenciesForGroup :Name={model.IdTest}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddCompetenciesForGroup",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"id теста={model.IdTest}"
                        });
                        await ms.CompetenciesForGroup.SaveCompetenciesForGroup(model);
                        return Ok(new { message = "Компетенция добавлена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenciesForGroup" })]
        [HttpPost("UpdateCompetenciesForGroup")]
        public async Task<IActionResult> UpdateCompetenciesForGroup([FromHeader] string Authorization, [FromBody] CompetenciesForGroupDto? model)
        {
            if (!Authorization.IsNullOrEmpty() && model != null &&
                model.Id.HasValue && !model.IdTest.IsNullOrEmpty() && model.IdGroupPositions != 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if ((await ms.CompetenciesForGroup.GetAllCompetenciesForGroups()).Find(x => x.IdTest.Equals(model.IdTest) && x.IdGroupPositions.Equals(model.IdGroupPositions.Value)) != null)
                        {
                            return BadRequest(new { message = "Ошибка. Тест уже назначен этой группе" });
                        }
                        logger.LogInformation($"/admin-api/UpdateCompetenciesForGroup :Id={model.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateCompetenciesForGroup",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id теста={model.IdTest}, id группы={model.IdGroupPositions}"
                        });
                        if (await ms.CompetenciesForGroup.GetCompetenciesForGroupById(model.Id.Value) != null)
                        {
                            await ms.CompetenciesForGroup.SaveCompetenciesForGroup(model);
                            return Ok(new { message = "Компетенция для группы обновлена" });
                        }
                        return NotFound(new { message = "Ошибка. Компетенция для группы не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/CompetenciesForGroup" })]
        [HttpPost("DeleteCompetenciesForGroup")]
        public async Task<IActionResult> DeleteCompetenciesForGroup([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteCompetenciesForGroup :competenceId={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteCompetenciesForGroup",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенциидля группы теста={id.Id}"
                        });
                        if (id.Id != 0 && await ms.CompetenciesForGroup.GetCompetenciesForGroupById(id.Id.Value) != null)
                        {
                            await ms.CompetenciesForGroup.DeleteCompetenciesForGroupById(id.Id.Value);
                            return Ok(new { message = "Компетенция для группы удалена" });
                        }
                        return NotFound(new { message = "Ошибка. Компетенция для группы не найдена" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
        *  TestScore
        */
        [SwaggerOperation(Tags = new[] { "Admin/TestScore" })]
        [HttpGet("GetTestScores")]
        public async Task<IActionResult> GetTestScores([FromHeader] string Authorization)
        {
            logger.LogInformation($"/admin-api/GetTestScores");
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetTestScores");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetTestScores",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });
                    return Ok(await ms.TestScore.GetAllTestScoreDtos());
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/TestScore" })]
        [HttpPost("GetTestScoresByTest")]
        public async Task<IActionResult> GetTestScoresByTest([FromHeader] string Authorization, [FromBody]StringIdModel id)
        {
            logger.LogInformation($"/admin-api/GetTestScores");
            if (!Authorization.IsNullOrEmpty() && id!=null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetTestScores");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetTestScores",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });
                    return Ok(await ms.TestScore.GetTestScoreDtosByTest(id.Id));
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/TestScore" })]
        [HttpPost("AddTestScore")]
        public async Task<IActionResult> AddTestScore([FromHeader] string Authorization, [FromBody] AddTestScoreModel? model)
        {
            if (!Authorization.IsNullOrEmpty() && model != null
                && model.MinValue.HasValue && model.MaxValue.HasValue
                && !model.Description.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/AddTestScore ");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/AddTestScore",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now
                    });
                    await ms.TestScore.SaveTestScore(model);
                    return Ok(new { message = "Оценка результата теста добавлена" });
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/TestScore" })]
        [HttpPost("UpdateTestScore")]
        public async Task<IActionResult> UpdateTestScore([FromHeader] string Authorization, [FromBody] TestScoreDto? dto)
        {
            if (!Authorization.IsNullOrEmpty() && dto != null && dto.Id.HasValue && dto.Id != 0
                && dto.MinValue.HasValue && dto.MaxValue.HasValue
                && !dto.Description.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/UpdateGlobalConfigure ");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/UpdateGlobalConfigure",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now
                    });
                    await ms.TestScore.SaveTestScore(dto);
                    return Ok(new { message = "Конфигурация обновлена" });
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/TestScore" })]
        [HttpPost("DeleteTestScore")]
        public async Task<IActionResult> DeleteTestScore([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if ((await ms.TestScore.GetTestScoreById(id.Id.Value)) != null)
                        {
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/DeleteTestScore",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now
                            });
                            if (await ms.GlobalConfigure.GetGlobalConfigureById(id.Id.Value) != null)
                            {
                                await ms.GlobalConfigure.DeleteGlobalConfigureById(id.Id.Value);
                            }
                            return Ok(new { message = "Конфигурация удалена" });
                        }
                        else
                        {
                            return NotFound(new { message = "Ошибка. Такой оценки результата нет" });
                        }
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  Test
         */
        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpGet("GetTests")]
        public async Task<IActionResult> GetTests([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetTests ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetTests",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        return Ok(await ms.Test.GetAllTestGetModels());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpGet("GetTestsPage")]
        public async Task<IActionResult> GetTestsPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetTests ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetTests",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        var Alltests = (await ms.Test.GetAllTestGetModels());

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Alltests.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var tests = Alltests
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(tests);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("GetTestsByEmployeeId")]
        public async Task<IActionResult> GetTestsByEmployeeId([FromHeader] string Authorization, [FromBody] StringIdModel id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        List<TestGetModel> models = new List<TestGetModel>();
                        Employee employee = await ms.Employee.GetEmployeeById(id.Id);
                        if (employee == null)
                            return NotFound(new { message = "Ошибка. Такого сотрудника нет" });

                        Subdivision subdivision = await ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value);
                        if (subdivision != null)
                        {
                            List<CompetenciesForGroup>? compsForGroup = (await ms.CompetenciesForGroup.GetAllCompetenciesForGroups()).Where(x => x.IdGroupPositions.Equals(subdivision.IdGroupPositions.Value)).ToList(); ;
                            if (compsForGroup.Count == 0)
                            {
                                return NotFound(new { message = $"Ошибка. В группе этого пользователя нет тестов" });
                            }
                            else
                            {
                                models = new List<TestGetModel>();
                                foreach (CompetenciesForGroup comp in compsForGroup)
                                {
                                    TestGetModel testModel = await ms.Test.GetTestModelById(comp.IdTest);
                                    models.Add(testModel);
                                }
                            }
                        }

                        logger.LogInformation($"/admin-api/GetTestsByEmployeeId ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetTestsByEmployeeId",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"id сотрудника={id.Id}"
                        });
                        return Ok(models);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("GetTest")]
        public async Task<IActionResult> GetTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetTest :id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Теста={id.Id}"
                        });
                        if (!string.IsNullOrEmpty(id.Id))
                        {
                            Test? test = await ms.Test.GetTestById(id.Id);
                            if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });
                            List<Question> questions = (await ms.Question.GetAllQuestions())
                                .Where(x => x.IdTest.Equals(id.Id)).ToList();
                            questions = questions.OrderBy(x => x.Number).ToList();

                            TestModel testDto = new TestModel
                            {
                                Id = test.Id,
                                Name = test.Name,
                                Weight = test.Weight,
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

                                    /*Random rand = new Random();
                                    firstPartDtos = firstPartDtos.OrderBy(x => rand.Next()).ToList();
                                    secondPartDtos = secondPartDtos.OrderBy(x => rand.Next()).ToList();*/

                                    createQuestionDto.Answers.AddRange(firstPartDtos);
                                    createQuestionDto.Answers.AddRange(secondPartDtos);
                                }
                                testDto.Questions.Add(createQuestionDto);
                            }
                            testDto.Questions.OrderBy(x => x.Number);
                            return Ok(testDto);
                        }
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("GetEntityTest")]
        public async Task<IActionResult> GetEntityTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetEntityTest :id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetEntityTest",
                            UserId = token.IdAdmin,
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

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("UpdateEntityTest")]
        public async Task<IActionResult> UpdateEntityTest([FromHeader] string Authorization, [FromBody] TestDto? test)
        {
            if (!Authorization.IsNullOrEmpty() && test != null && !string.IsNullOrEmpty(test.Id) && !string.IsNullOrEmpty(test.Name) &&
                test.Weight != null && test.Weight.HasValue && test.IdCompetence != null && test.IdCompetence != 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateEntityTest :id={test.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateEntityTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Теста={test.Id}"
                        });

                        if (await ms.Test.GetTestById(test.Id) == null)
                            return NotFound(new { message = "Ошибка. Тест не найден" });

                        if (await ms.TestType.GetCompetenceById(test.IdCompetence.Value) == null)
                            return BadRequest(new { message = "Ошибка. Компетенция не найдена" });

                        await ms.Test.SaveTest(test);

                        return Ok(test);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("GetPdfTest")]
        public async Task<IActionResult> GetPdfTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPdfTest :id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPdfTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id теста ={id.Id}"
                        });

                        Test? test = await ms.Test.GetTestById(id.Id);
                        if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });
                        string html = "<h1 style=\"text-align: left;\">Название Теста: " + test.Name + "</h1> " +
                                      "<h2 style=\"text-align: left;\">Компетенция: " + (await ms.TestType.GetCompetenceById(test.IdCompetence.Value)).Name + "</h2><hr>";
                        html += $"<p>Кол-во баллов: {test.Weight}</p>";
                        html += $"<p>Описание: {test.Description}</p>";
                        html += $"<p>Инструкция: {test.Instruction}</p>";
                        List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);
                        questions = questions.OrderBy(x => x.Number).ToList();

                        //.Where(x => x.IdTest.Equals(id)).ToList();

                        //!var Renderer = new IronPdf.ChromePdfRenderer();
                        var doc = new PdfDocument();

                        foreach (var quest in questions)
                        {
                            html += $"<p>{Convert.ToInt32(quest.Number)}. " + quest.Text + "\r\n<p>Тип вопроса:" + (await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name + "</p>";

                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                /*byte[] imageBytes = System.IO.File.ReadAllBytes(Path.Combine(environment.WebRootPath)+ "\\images\\" + quest.ImagePath);
                                var ImgDataURI = @"data:image/png;base64," + Convert.ToBase64String(imageBytes);
                                var ImgHtml = $"<img src='{ImgDataURI}'>";*/

                                //html += "<style>\r\n  img.logo { \r\n   width:auto;\r\n   height:200px;\r\n   content: url(\"data:image/png;charset=utf-8;base64, " + Convert.ToBase64String(imageBytes) +"\");\r\n    }\r\n</style>";
                                //html += "<p>Вопрос:" + quest.Text + "<img class=\"logo\" ></img></p>\r\n" +
                                //      "\r\n<p>Тип вопроса:" + ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value).Name + "</p>";

                                /*string data = "data:image/png;charset=utf-8;base64," + Convert.ToBase64String(imageBytes)+ "\"";
                                string imageUrl = "https://"+Request.Host.Value+"/images/" + quest.ImagePath;
                                html += "<p>Вопрос:" + quest.Text + "</p>\r\n<p>Тип вопроса:" + ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value).Name + "</p>";*/
                                if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                {
                                    /*string filePath = environment.WebRootPath + "/images/" + quest.ImagePath;
                                    byte[] imgArray = System.IO.File.ReadAllBytes(filePath);
                                    string base64 = Convert.ToBase64String(imgArray);
                                    string imageUrl = "data:image/jpg;base64, " + base64 + "";
                                    //html += "<img style='width:auto; height:100px;' src=\"" + imageUrl + "\" />";*/
                                    //html += "<style>\r\n  img.logo { \r\n   width:auto;\r\n   height:200px;\r\n   content: url(\"data:image/png;charset=utf-8;base64, " + base64 +"\");\r\n    }\r\n</style>";
                                    byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                    string base64 = Convert.ToBase64String(array);
                                    html += $"<p><img style=\"width:300px; height:auto;\" src='data:image/jpg;base64,{base64}'/></p>";
                                }
                            }

                            if ((await ms.Answer.GetAnswerDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                foreach (AnswerDto answer in await ms.Answer.GetAnswerDtosByQuestionId(quest.Id))
                                {
                                    if (!answer.ImagePath.IsNullOrEmpty())
                                    {
                                        if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                        {
                                            byte[] imageBytes = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                            //html += "<style>\r\n    img.logo { \r\n        width:auto;\r\n        height:50px;\r\n        content: url('data:image/jpeg;base64," + Convert.ToBase64String(imageBytes) + "')\r\n    }\r\n</style>";
                                            //string path = "https://" + Request.Host+"/"+answer.ImagePath;
                                            /*html += " img.logo { width:110px;height:110px;content: url('data:image/jpeg;base64," + Convert.ToBase64String(imageBytes) + "')} ";
                                            html += "<p>&#10065 " + "<img class=\"logo\"></img>" +
                                                    answer.Text + "</p>";*/
                                            byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath);
                                            string base64 = Convert.ToBase64String(array);
                                            html += $"<p><img style=\"width:200px; height:auto;\" src='data:image/png;base64,{base64}'/></p>";
                                        }
                                    }
                                    html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\"></span>{answer.Number}. {answer.Text}</span>";
                                }
                            }
                            if ((await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                foreach (SubsequenceDto sub in await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id))
                                {
                                    html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\"></span>{sub.Text}</span>";
                                }
                            }
                            if ((await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                List<FirstSecondPartDto> list = await ms.GetFirstSecondPartDtoByQuestion(quest.Id);
                                List<string> fpText = new List<string>();
                                List<string> spText = new List<string>();
                                foreach (FirstSecondPartDto dto in list)
                                {
                                    fpText.Add(dto.FirstPartText);
                                    spText.Add(dto.SecondPartText);
                                }
                                Random rnd = new Random();
                                fpText = fpText.OrderBy(x => rnd.Next()).ToList();
                                spText = spText.OrderBy(x => rnd.Next()).ToList();

                                for (int i = 0; i < fpText.Count; i++)
                                {
                                    html += "<p style=\"white-space: pre;\">" + fpText[i] + "               " + spText[i] + "</p>";
                                }
                            }
                        }
                        html += "<hr>    <p>Дата прохождения теста:_________________________________________</p>\r\n    <p>ФИО:_________________________________________</p>    <p>Подпись:___________________</p>";
                        //html += "</div>";
                        //var doc = new PdfDocument();
                        PdfGenerator.AddPdfPages(doc, html, PageSize.A4);

                        byte[] res = null;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            doc.Save(ms);
                            res = ms.ToArray();
                        }
                        //!using var pdfdoc = Renderer.RenderHtmlAsPdf(html);

                        return File(res, "application/pdf", test.Name + ".pdf");
                        //!return File(pdfdoc.BinaryData, "application/pdf", "TESTPDF.Pdf");
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("GetPdfCorerectTest")]
        public async Task<IActionResult> GetPdfCorerectTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPdfCorerectTest :id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPdfCorerectTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id теста ={id.Id}"
                        });

                        Test? test = await ms.Test.GetTestById(id.Id);
                        if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });
                        string html = "";
                        html += "<div><h1 style=\"text-align: left;\">Название Теста: " + test.Name + "</h1>\r\n " +
                                      "<h2 style=\"text-align: left;\">Компетенция: " + (await ms.TestType.GetCompetenceById(test.IdCompetence.Value)).Name + "</h2>\r\n<hr>";
                        html += $"<p>Кол-во баллов: {test.Weight}</p>";
                        html += $"<p>Описание: {test.Description}</p>";
                        html += $"<p>Инструкция: {test.Instruction}</p>";
                        List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);
                        questions = questions.OrderBy(x => x.Number).ToList();

                        var doc = new PdfDocument();

                        foreach (var quest in questions)
                        {
                            html += $"<p>{Convert.ToInt32(quest.Number)}. " + quest.Text + "\r\n<p>Тип вопроса:" + (await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name + "</p>";

                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                {
                                    byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                    string base64 = Convert.ToBase64String(array);
                                    html += $"<p><img style=\"width:auto; height:150px;\" src='data:image/jpg;base64,{base64}'/></p>";
                                }
                            }
                            if ((await ms.Answer.GetAnswerDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                foreach (AnswerDto answer in await ms.Answer.GetAnswerDtosByQuestionId(quest.Id))
                                {
                                    if (!answer.ImagePath.IsNullOrEmpty())
                                    {
                                        if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                        {
                                            byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath);
                                            string base64 = Convert.ToBase64String(array);
                                            html += $"<p><img style=\"width:auto; height:150px;\" src='data:image/png;base64,{base64}'/></p>";
                                        }
                                    }
                                    if (answer.Correct.Value)
                                    {
                                        html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\">X</span>{answer.Number}. {answer.Text}</span>";
                                    }
                                    else
                                    {
                                        html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\"></span>{answer.Number}. {answer.Text}</span>";
                                    }
                                }
                            }
                            if ((await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                foreach (SubsequenceDto sub in await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id))
                                {
                                    html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\">{sub.Number}</span>{sub.Text}</span>";
                                    //html += "<p> "+sub.Number + " " + sub.Text + "</p>";
                                }
                            }
                            if ((await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                List<FirstSecondPartDto> list = await ms.GetFirstSecondPartDtoByQuestion(quest.Id);
                                foreach (FirstSecondPartDto dto in list)
                                {
                                    html += "<p>" + dto.FirstPartText + " - " + dto.SecondPartText + "</p>";
                                }
                            }
                        }
                        html += "<hr>\r\n    <p>Дата прохождения теста:_________________________________________</p>\r\n    <p>ФИО:_________________________________________</p>\r\n    <p>Подпись:___________________</p>";
                        html += "</div>";
                        //var doc = new PdfDocument();
                        PdfGenerator.AddPdfPages(doc, html, PageSize.A4);

                        byte[] res = null;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            doc.Save(ms);
                            res = ms.ToArray();
                        }
                        return File(res, "application/pdf", test.Name + ".pdf");
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        /*[HttpPost("GetWordTest")]
         public async Task<IActionResult> GetWordTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
         {
             if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty())
             {
                 TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                 if (token != null)
                 {
                     if (await ms.IsTokenAdminExpired(token))
                     {
                         return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                     }
                     else
                     {
                         logger.LogInformation($"/admin-api/GetPdfTest :id={id.Id}");
                         await ms.Log.SaveLog(new Log
                         {
                             UrlPath = "admin-api/GetPdfTest",
                             UserId = token.IdAdmin,
                             UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                             DataTime = DateTime.Now,
                             Params = $"Id теста ={id.Id}"
                         });

                         MemoryStream stream = new MemoryStream();
                         DocX doc = DocX.Create(stream);

                         Test? test = await ms.Test.GetTestById(id.Id);
                         if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });

                         Paragraph par1 = doc.InsertParagraph();
                         par1.Append($"Название: {test.Name}").Font("Times New Roman").FontSize(14).Color(Color.Black).Bold();
                         Paragraph par2 = doc.InsertParagraph();
                         par2.Append($"Категория теста: { (await ms.TestType.GetCompetenceById(test.IdCompetence.Value)).Name}").Font("Times New Roman").FontSize(14).Color(Color.Black).Bold();
                         Paragraph par3 = doc.InsertParagraph();
                         par3.Append($"Кол-во баллов: {test.Weight}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                         Paragraph par4 = doc.InsertParagraph();
                         par4.Append($"Описание: {test.Description}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                         Paragraph par5 = doc.InsertParagraph();
                         par5.Append($"инструкция: {test.Instruction}").Font("Times New Roman").FontSize(14).Color(Color.Black);

                         List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);
                         questions = questions.OrderBy(x => x.Number).ToList();

                         foreach (var quest in questions)
                         {
                             Paragraph par6 = doc.InsertParagraph();
                             par6.Append($"{quest.Number}. {quest.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                             Paragraph par7 = doc.InsertParagraph();
                             par7.Append($"Тип вопроса: {(await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name}").Font("Times New Roman").FontSize(14).Color(Color.Black);

                             if (!quest.ImagePath.IsNullOrEmpty())
                             {
                                 if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                 {
                                     byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                     string base64 = Convert.ToBase64String(array);
                                     Xceed.Document.NET.Image image1 = doc.AddImage(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath.ToString());
                                 }
                             }

                             if ((await ms.Answer.GetAnswerDtosByQuestionId(quest.Id)).Count != 0)
                             {
                                 foreach (AnswerDto answer in await ms.Answer.GetAnswerDtosByQuestionId(quest.Id))
                                 {
                                     Paragraph par8 = doc.InsertParagraph();
                                     par8.Append($"☐{answer.Number}. {answer.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                                     if (!answer.ImagePath.IsNullOrEmpty())
                                     {
                                         if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                         {

                                             byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath);
                                             string base64 = Convert.ToBase64String(array);
                                             Xceed.Document.NET.Image image2 = doc.AddImage(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath.ToString());
                                         }
                                     }
                                 }
                             }
                             if ((await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id)).Count != 0)
                             {
                                 foreach (SubsequenceDto sub in await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id))
                                 {
                                     Paragraph par8 = doc.InsertParagraph();
                                     par8.Append($"☐{sub.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                                 }
                             }
                             if ((await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id)).Count != 0)
                             {
                                 List<FirstSecondPartDto> list = await ms.GetFirstSecondPartDtoByQuestion(quest.Id);
                                 List<string> fpText = new List<string>();
                                 List<string> spText = new List<string>();
                                 foreach (FirstSecondPartDto dto in list)
                                 {
                                     fpText.Add(dto.FirstPartText);
                                     spText.Add(dto.SecondPartText);
                                 }
                                 Random rnd = new Random();
                                 fpText.OrderBy(x => rnd.Next()).ToArray();
                                 spText.OrderBy(x => rnd.Next()).ToArray();

                                 for (int i = 0; i < fpText.Count; i++)
                                 {
                                     Paragraph par8 = doc.InsertParagraph();
                                     par8.Append($"{fpText[i]}         {spText[i]}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                                 }
                             }
                         }
                         Paragraph par9 = doc.InsertParagraph();
                         par9.Append($"Дата прохождения теста:______________________").Font("Times New Roman").FontSize(14).Color(Color.Black);
                         Paragraph par10 = doc.InsertParagraph();
                         par10.Append($"ФИО:_________________________________________").Font("Times New Roman").FontSize(14).Color(Color.Black);

                         doc.Save();
                         return File(stream.ToArray(), "application/octet-stream", $"{test.Name}.docx");
                     }
                 }
                 return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
             }
             return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
         }

         [HttpPost("GetWordCorrectTest")]
         public async Task<IActionResult> GetWordCorrectTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
         {
             if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty())
             {
                 TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                 if (token != null)
                 {
                     if (await ms.IsTokenAdminExpired(token))
                     {
                         return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                     }
                     else
                     {
                         logger.LogInformation($"/admin-api/GetPdfTest :id={id.Id}");
                         await ms.Log.SaveLog(new Log
                         {
                             UrlPath = "admin-api/GetPdfTest",
                             UserId = token.IdAdmin,
                             UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                             DataTime = DateTime.Now,
                             Params = $"Id теста ={id.Id}"
                         });

                         MemoryStream stream = new MemoryStream();
                         DocX doc = DocX.Create(stream);

                         Test? test = await ms.Test.GetTestById(id.Id);
                         if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });

                         Paragraph par1 = doc.InsertParagraph();
                         par1.Append($"Название: {test.Name}").Font("Times New Roman").FontSize(14).Color(Color.Black).Bold();
                         Paragraph par2 = doc.InsertParagraph();
                         par2.Append($"Категория теста: {(await ms.TestType.GetCompetenceById(test.IdCompetence.Value)).Name}").Font("Times New Roman").FontSize(14).Color(Color.Black).Bold();
                         Paragraph par3 = doc.InsertParagraph();
                         par3.Append($"Кол-во баллов: {test.Weight}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                         Paragraph par4 = doc.InsertParagraph();
                         par4.Append($"Описание: {test.Description}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                         Paragraph par5 = doc.InsertParagraph();
                         par5.Append($"инструкция: {test.Instruction}").Font("Times New Roman").FontSize(14).Color(Color.Black);

                         List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);
                         questions = questions.OrderBy(x => x.Number).ToList();

                         foreach (var quest in questions)
                         {
                             Paragraph par6 = doc.InsertParagraph();
                             par6.Append($"{quest.Number}. {quest.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                             Paragraph par7 = doc.InsertParagraph();
                             par7.Append($"Тип вопроса: {(await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name}").Font("Times New Roman").FontSize(14).Color(Color.Black);

                             if (!quest.ImagePath.IsNullOrEmpty())
                             {
                                 if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                 {
                                     byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                     string base64 = Convert.ToBase64String(array);
                                     Xceed.Document.NET.Image image1 = doc.AddImage(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath.ToString());
                                 }
                             }

                             if ((await ms.Answer.GetAnswerDtosByQuestionId(quest.Id)).Count != 0)
                             {
                                 foreach (AnswerDto answer in await ms.Answer.GetAnswerDtosByQuestionId(quest.Id))
                                 {
                                     if (answer.Correct.Value)
                                     {
                                         Paragraph par81 = doc.InsertParagraph();
                                         par81.Append($"☒{answer.Number}. {answer.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                                     }
                                     else
                                     {
                                         Paragraph par8 = doc.InsertParagraph();
                                         par8.Append($"☐{answer.Number}. {answer.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                                     }

                                     if (!answer.ImagePath.IsNullOrEmpty())
                                     {
                                         if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                         {

                                             byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath);
                                             string base64 = Convert.ToBase64String(array);
                                             Xceed.Document.NET.Image image2 = doc.AddImage(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath.ToString());
                                         }
                                     }
                                 }
                             }
                             if ((await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id)).Count != 0)
                             {
                                 foreach (SubsequenceDto sub in await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id))
                                 {
                                     Paragraph par8 = doc.InsertParagraph();
                                     par8.Append($"  {sub.Number} {sub.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                                 }
                             }
                             if ((await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id)).Count != 0)
                             {
                                 List<FirstSecondPartDto> list = await ms.GetFirstSecondPartDtoByQuestion(quest.Id);
                                 foreach (FirstSecondPartDto dto in list)
                                 {
                                     Paragraph par8 = doc.InsertParagraph();
                                     par8.Append($"{dto.FirstPartText} - {dto.SecondPartText}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                                 }
                             }
                         }
                         Paragraph par9 = doc.InsertParagraph();
                         par9.Append($"Дата прохождения теста:______________________").Font("Times New Roman").FontSize(14).Color(Color.Black);
                         Paragraph par10 = doc.InsertParagraph();
                         par10.Append($"ФИО:_________________________________________").Font("Times New Roman").FontSize(14).Color(Color.Black);

                         doc.Save();
                         return File(stream.ToArray(), "application/octet-stream", $"{test.Name}.docx");
                     }
                 }
                 return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
             }
             return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
         }*/

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("GetWordTest")]
        public async Task<IActionResult> GetWordTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetWordTest :id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetWordTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id теста ={id.Id}"
                        });

                        WordDocument doc = new WordDocument(CultureInfo.GetCultureInfo("ru-ru"));
                        doc.Styles.DocumentDefaults.RunProperties.Font.Ascii = "Times New Roman";
                        doc.Styles.DocumentDefaults.RunProperties.Font.HighAnsi = "Times New Roman";

                        Test? test = await ms.Test.GetTestById(id.Id);
                        if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });

                        var section = doc.Body.Sections.First();
                        var paragraph = section.AppendParagraph();
                        paragraph.AppendText($"Название: {test.Name}");
                        paragraph.AppendText("\n");
                        paragraph.AppendText($"\"Категория теста: {(await ms.TestType.GetCompetenceById(test.IdCompetence.Value)).Name}\"");
                        paragraph.AppendText("\n");
                        paragraph.AppendText($"Кол-во баллов: {test.Weight}");
                        paragraph.AppendText("\n");
                        paragraph.AppendText($"Описание: {test.Description}");
                        paragraph.AppendText("\n");
                        paragraph.AppendText($"Инструкция: {test.Instruction}");
                        paragraph.AppendText("\n");

                        List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);
                        questions = questions.OrderBy(x => x.Number).ToList();

                        foreach (var quest in questions)
                        {
                            var paragraph1 = section.AppendParagraph();
                            paragraph1.AppendText($"{quest.Number}. {quest.Text}");
                            paragraph1.AppendText("\n");
                            paragraph1.AppendText($"Тип вопроса: {(await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name}");
                            paragraph1.AppendText("\n");

                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                {
                                    FileStream file = new FileStream(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath, FileMode.Open);
                                    var image = doc.AddImage(file, KnownImageContentTypes.Jpg);
                                    var picture = section.WrapImageIntoInlinePicture(image, "quest", "", 200, 100);
                                    paragraph1.AppendPicture(picture);
                                }
                            }

                            if ((await ms.Answer.GetAnswerDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                var paragraph2 = section.AppendParagraph();
                                foreach (AnswerDto answer in await ms.Answer.GetAnswerDtosByQuestionId(quest.Id))
                                {
                                    if (!answer.ImagePath.IsNullOrEmpty())
                                    {
                                        if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath))
                                        {
                                            FileStream file = new FileStream(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath, FileMode.Open);
                                            var image = doc.AddImage(file, KnownImageContentTypes.Jpg);
                                            var picture = section.WrapImageIntoInlinePicture(image, "answer", "", 200, 100);
                                            paragraph2.AppendPicture(picture);
                                        }
                                    }
                                    paragraph2.AppendText($"☐{answer.Number}. {answer.Text}\n");
                                }
                                paragraph2.AppendText("\n");
                            }
                            if ((await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                var paragraph3 = section.AppendParagraph();
                                foreach (SubsequenceDto sub in await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id))
                                {
                                    paragraph3.AppendText($"☐{sub.Text}\n");
                                }
                                paragraph3.AppendText("\n");
                            }
                            if ((await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                List<FirstSecondPartDto> list = await ms.GetFirstSecondPartDtoByQuestion(quest.Id);
                                List<string> fpText = new List<string>();
                                List<string> spText = new List<string>();
                                foreach (FirstSecondPartDto dto in list)
                                {
                                    fpText.Add(dto.FirstPartText);
                                    spText.Add(dto.SecondPartText);
                                }
                                Random rnd = new Random();
                                fpText = fpText.OrderBy(x => rnd.Next()).ToList();
                                spText = spText.OrderBy(x => rnd.Next()).ToList();

                                var paragraph4 = section.AppendParagraph();
                                for (int i = 0; i < fpText.Count; i++)
                                {
                                    paragraph4.AppendText($"{fpText[i]}         {spText[i]}\n");
                                }
                                paragraph4.AppendText("\n");
                            }
                        }
                        var paragraph5 = section.AppendParagraph();
                        paragraph5.AppendText($"Дата прохождения теста:______________________");
                        paragraph5.AppendText("\n");
                        paragraph5.AppendText($"ФИО:_________________________________________");

                        byte[] res = null;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            doc.Save(ms);
                            res = ms.ToArray();
                        }

                        return File(res, "application/octet-stream", $"{test.Name}.docx");
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("GetWordCorrectTest")]
        public async Task<IActionResult> GetWordCorrectTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetWordCorrectTest :id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetWordCorrectTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id теста ={id.Id}"
                        });

                        WordDocument doc = new WordDocument(CultureInfo.GetCultureInfo("ru-ru"));
                        doc.Styles.DocumentDefaults.RunProperties.Font.Ascii = "Times New Roman";
                        doc.Styles.DocumentDefaults.RunProperties.Font.HighAnsi = "Times New Roman";

                        Test? test = await ms.Test.GetTestById(id.Id);
                        if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });

                        var section = doc.Body.Sections.First();
                        var paragraph = section.AppendParagraph();
                        paragraph.AppendText($"Название: {test.Name}");
                        paragraph.AppendText("\n");
                        paragraph.AppendText($"\"Категория теста: {(await ms.TestType.GetCompetenceById(test.IdCompetence.Value)).Name}\"");
                        paragraph.AppendText("\n");
                        paragraph.AppendText($"Кол-во баллов: {test.Weight}");
                        paragraph.AppendText("\n");
                        paragraph.AppendText($"Описание: {test.Description}");
                        paragraph.AppendText("\n");
                        paragraph.AppendText($"Инструкция: {test.Instruction}");
                        paragraph.AppendText("\n");

                        List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);
                        questions = questions.OrderBy(x => x.Number).ToList();

                        foreach (var quest in questions)
                        {
                            var paragraph1 = section.AppendParagraph();
                            paragraph1.AppendText($"{quest.Number}. {quest.Text}\n");
                            paragraph1.AppendText($"Тип вопроса: {(await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name}\n");

                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                {
                                    FileStream file = new FileStream(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath, FileMode.Open);
                                    var image = doc.AddImage(file, KnownImageContentTypes.Jpg);
                                    var picture = section.WrapImageIntoInlinePicture(image, "quest", "", 200, 100);
                                    paragraph.AppendPicture(picture);
                                }
                            }

                            if ((await ms.Answer.GetAnswerDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                var paragraph2 = section.AppendParagraph();
                                foreach (AnswerDto answer in await ms.Answer.GetAnswerDtosByQuestionId(quest.Id))
                                {
                                    if (!answer.ImagePath.IsNullOrEmpty())
                                    {
                                        if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath))
                                        {
                                            FileStream file = new FileStream(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath, FileMode.Open);
                                            var image = doc.AddImage(file, KnownImageContentTypes.Jpg);
                                            var picture = section.WrapImageIntoInlinePicture(image, "answer", "", 200, 100);
                                            paragraph2.AppendPicture(picture);
                                        }
                                    }
                                    if (answer.Correct.Value)
                                    {
                                        paragraph2.AppendText($"☒{answer.Number}. {answer.Text}\n");
                                    }
                                    else
                                    {
                                        paragraph2.AppendText($"☐{answer.Number}. {answer.Text}\n");
                                    }
                                }
                            }
                            if ((await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                var paragraph3 = section.AppendParagraph();
                                foreach (SubsequenceDto sub in await ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id))
                                {

                                    paragraph3.AppendText($" {sub.Number} {sub.Text}\n");
                                }
                            }
                            if ((await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                var paragraph4 = section.AppendParagraph();
                                List<FirstSecondPartDto> list = await ms.GetFirstSecondPartDtoByQuestion(quest.Id);
                                foreach (FirstSecondPartDto dto in list)
                                {
                                    paragraph4.AppendText($"{dto.FirstPartText} - {dto.SecondPartText}\n");
                                }
                            }
                        }
                        var paragraph5 = section.AppendParagraph();
                        paragraph5.AppendText($"Дата прохождения теста:______________________");
                        paragraph5.AppendText("\n");
                        var paragraph6 = section.AppendParagraph();
                        paragraph6.AppendText($"ФИО:_________________________________________");

                        byte[] res = null;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            doc.Save(ms);
                            res = ms.ToArray();
                        }

                        return File(res, "application/octet-stream", $"{test.Name}.docx");
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("AddTest")]
        public async Task<IActionResult> AddTest([FromHeader] string Authorization, [FromForm] AddPostTestModel? postModel)
        {
            if (postModel != null && !postModel.Test.IsNullOrEmpty())
            {
                AddTestModel? test = JsonConvert.DeserializeObject<AddTestModel>(postModel.Test);
                if (!Authorization.IsNullOrEmpty() && test != null && !test.Name.IsNullOrEmpty() && test.Weight.HasValue &&
                    test.CompetenceId.HasValue && test.Questions != null && test.Questions.Count != 0 &&
                    test.CompetenceId != 0)
                {
                    TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                    if (token != null)
                    {
                        if (await ms.IsTokenAdminExpired(token))
                        {
                            return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                        }
                        else
                        {
                            logger.LogInformation($"/admin-api/AddTest test: name={test.Name}, idCompetence={test.CompetenceId}," +
                                                  $"countOfQuestions={test.Questions.Count}");
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/AddTest",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id компетенции={test.CompetenceId}"
                            });

                            if (await ms.TestType.GetCompetenceById(test.CompetenceId.Value) == null)
                                return BadRequest(new { message = "Ошибка. Такой компетенции нет" });

                            string idTest = Guid.NewGuid().ToString();
                            Test newTest = new Test
                            {
                                Id = idTest,
                                Name = test.Name,
                                IdCompetence = test.CompetenceId,
                                Weight = test.Weight,
                                Generation = test.Generation,
                                Description = test.Description,
                                Instruction = test.Instruction
                            };
                            await ms.Test.SaveTest(newTest);

                            int testWeight = 0;
                            int countOfQuestions = 1;
                            foreach (AddQuestionModel quest in test.Questions)
                            {
                                if (quest.IdQuestionType == null || !quest.IdQuestionType.HasValue || await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value) == null)
                                    continue;

                                logger.LogInformation($"quest -> text={quest.Text} idType={quest.IdQuestionType} count={quest.Answers.Count}");
                                string idQuestion = Guid.NewGuid().ToString();
                                if (!quest.ImagePath.IsNullOrEmpty() && postModel.Files != null && postModel.Files.Count != 0)
                                {
                                    IFormFile file = postModel.Files.First(f => f.FileName.Equals(quest.ImagePath));
                                    string ext = Path.GetExtension(quest.ImagePath);
                                    if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                    {
                                        if (!System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath))
                                        {
                                            string saveImage = Path.Combine(environment.WebRootPath + "\\images\\", file.FileName);
                                            using (var upload = new FileStream(saveImage, FileMode.Create))
                                            {
                                                await file.CopyToAsync(upload);
                                            }
                                            await ms.Question.SaveQuestion(new Question
                                            {
                                                Id = idQuestion,
                                                Text = quest.Text,
                                                IdQuestionType = quest.IdQuestionType,
                                                ImagePath = quest.ImagePath,
                                                Number = Convert.ToByte(countOfQuestions),
                                                Weight = quest.Weight,
                                                IdTest = idTest
                                            });
                                        }
                                        else
                                        {
                                            string imagePath = Guid.NewGuid().ToString() + ext;
                                            string saveImage = Path.Combine(environment.WebRootPath + "\\images\\", imagePath);
                                            using (var upload = new FileStream(saveImage, FileMode.Create))
                                            {
                                                await file.CopyToAsync(upload);
                                            }
                                            await ms.Question.SaveQuestion(new Question
                                            {
                                                Id = idQuestion,
                                                Text = quest.Text,
                                                IdQuestionType = quest.IdQuestionType,
                                                ImagePath = imagePath,
                                                Number = Convert.ToByte(countOfQuestions),
                                                Weight = quest.Weight,
                                                IdTest = idTest
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    await ms.Question.SaveQuestion(new Question
                                    {
                                        Id = idQuestion,
                                        Text = quest.Text,
                                        IdQuestionType = quest.IdQuestionType,
                                        ImagePath = quest.ImagePath,
                                        Number = Convert.ToByte(countOfQuestions),
                                        Weight = quest.Weight,
                                        IdTest = idTest
                                    });
                                }
                                countOfQuestions++;
                                if (quest.Weight.HasValue && quest.Weight > 0)
                                {
                                    testWeight += quest.Weight.Value;
                                }
                                int countOfAnswer = 0;
                                foreach (JObject answer in quest.Answers)
                                {
                                    countOfAnswer++;
                                    /*AnswerDto answerDto = answer.Deserialize<AnswerDto>();
                                    SubsequenceDto subsequenceDto = answer.Deserialize<SubsequenceDto>();
                                    FirstSecondPartDto firstSecondPartDto = answer.Deserialize<FirstSecondPartDto>();*/

                                    if (quest.IdQuestionType.Value == 1 || quest.IdQuestionType.Value == 2)
                                    {
                                        AnswerDto answerDto = answer.ToObject<AnswerDto>();
                                        if (answerDto is AnswerDto && answerDto.Correct != null)
                                        {
                                            logger.LogInformation($"answerDto -> text={answerDto.Text}, correct={answerDto.Correct}");

                                            if (!answerDto.ImagePath.IsNullOrEmpty() && postModel.Files != null && postModel.Files.Count != 0)
                                            {
                                                IFormFile file = postModel.Files.First(f => f.FileName.Equals(answerDto.ImagePath));
                                                string ext = Path.GetExtension(answerDto.ImagePath);
                                                if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                                {
                                                    if (!System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("/images/" + answerDto.ImagePath).PhysicalPath))
                                                    {
                                                        string saveImage = Path.Combine(environment.WebRootPath + "\\images\\", answerDto.ImagePath);
                                                        using (var upload = new FileStream(saveImage, FileMode.Create))
                                                        {
                                                            await file.CopyToAsync(upload);
                                                        }

                                                        await ms.Answer.SaveAnswer(new Answer
                                                        {
                                                            Text = answerDto.Text,
                                                            IdQuestion = idQuestion,
                                                            Correct = answerDto.Correct,
                                                            Weight = answerDto.Weight,
                                                            Number = Convert.ToByte(countOfAnswer),
                                                            ImagePath = file.FileName
                                                        });
                                                    }
                                                    else
                                                    {
                                                        string imagePath = Guid.NewGuid().ToString() + ext;
                                                        string saveImage = Path.Combine(environment.WebRootPath + "\\images\\", imagePath);
                                                        using (var upload = new FileStream(saveImage, FileMode.Create))
                                                        {
                                                            await file.CopyToAsync(upload);
                                                        }

                                                        await ms.Answer.SaveAnswer(new Answer
                                                        {
                                                            Text = answerDto.Text,
                                                            IdQuestion = idQuestion,
                                                            Correct = answerDto.Correct,
                                                            Weight = answerDto.Weight,
                                                            Number = Convert.ToByte(countOfAnswer),
                                                            ImagePath = imagePath
                                                        });
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                await ms.Answer.SaveAnswer(new Answer
                                                {
                                                    Text = answerDto.Text,
                                                    IdQuestion = idQuestion,
                                                    Correct = answerDto.Correct,
                                                    Weight = answerDto.Weight,
                                                    Number = Convert.ToByte(countOfAnswer),
                                                    ImagePath = answerDto.ImagePath
                                                });
                                            }
                                        }
                                    }
                                    if (quest.IdQuestionType.Value == 4)
                                    {
                                        SubsequenceDto subsequenceDto = answer.ToObject<SubsequenceDto>();
                                        if (subsequenceDto is SubsequenceDto && subsequenceDto.Number != null && subsequenceDto.Number != 0)
                                        {
                                            logger.LogInformation($"subsequenceDto -> text={subsequenceDto.Text}, number={subsequenceDto.Number}");
                                            await ms.Subsequence.SaveSubsequence(new Subsequence
                                            {
                                                Text = subsequenceDto.Text,
                                                Number = subsequenceDto.Number,
                                                IdQuestion = idQuestion
                                            });
                                        }
                                    }
                                    if (quest.IdQuestionType.Value == 3)
                                    {
                                        FirstSecondPartDto firstSecondPartDto = answer.ToObject<FirstSecondPartDto>();

                                        if (firstSecondPartDto is FirstSecondPartDto &&
                                        !string.IsNullOrEmpty(firstSecondPartDto.FirstPartText) && !string.IsNullOrEmpty(firstSecondPartDto.SecondPartText))
                                        {
                                            logger.LogInformation($"firstSecondPartDto -> first={firstSecondPartDto.FirstPartText}, second={firstSecondPartDto.SecondPartText}");
                                            string firstPartId = Guid.NewGuid().ToString();
                                            await ms.FirstPart.SaveFirstPart(new FirstPart
                                            {
                                                Id = firstPartId,
                                                Text = firstSecondPartDto.FirstPartText,
                                                IdQuestion = idQuestion
                                            });
                                            await ms.SecondPart.SaveSecondPart(new SecondPart
                                            {
                                                Text = firstSecondPartDto.SecondPartText,
                                                IdFirstPart = firstPartId
                                            });
                                        }
                                    }
                                }
                            }
                            newTest.Weight = testWeight;
                            await ms.Test.SaveTest(newTest);
                            return Ok(new { message = "Добавление теста успешно" });
                        }
                    }
                    return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
                }
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("UpdateTest")]
        public async Task<IActionResult> UpdateTest([FromHeader] string Authorization, [FromForm] UpdatePostTestModel? updatePostModel)
        {
            if (updatePostModel != null && !updatePostModel.Test.IsNullOrEmpty())
            {
                UpdateTestModel? test = JsonConvert.DeserializeObject<UpdateTestModel>(updatePostModel.Test);
                if (!Authorization.IsNullOrEmpty() && test != null && !test.Name.IsNullOrEmpty() && test.Weight.HasValue &&
                    test.CompetenceId.HasValue && test.Questions != null && test.Questions.Count != 0 &&
                    test.CompetenceId != 0)
                {
                    TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                    if (token != null)
                    {
                        if (await ms.IsTokenAdminExpired(token))
                        {
                            return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                        }
                        else
                        {
                            logger.LogInformation($"/admin-api/UpdateTest test: id={test.Id} name={test.Name}, idCompetence={test.CompetenceId}," +
                                      $"countOfQuestions={test.Questions.Count}");
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/UpdateTest",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id Теста={test.Id}"
                            });

                            if (await ms.TestType.GetCompetenceById(test.CompetenceId.Value) == null)
                                return BadRequest(new { message = "Ошибка. Такой компетенции нет" });

                            if (await ms.Test.GetTestById(test.Id) == null)
                                return BadRequest(new { message = "Ошибка. Такого теста нет" });

                            await ms.Test.SaveTest(new Test
                            {
                                Id = test.Id,
                                Name = test.Name,
                                Weight = test.Weight,
                                Generation = test.Generation,
                                IdCompetence = test.CompetenceId,
                                Instruction = test.Instructuion,
                                Description = test.Description
                            });

                            List<QuestionDto> currentQuestions = await ms.Question.GetQuestionDtosByTest(test.Id);
                            foreach (QuestionModel quest in test.Questions)
                            {
                                logger.LogInformation($"quest -> text={quest.Text} idType={quest.IdQuestionType} count={quest.Answers.Count}");
                                if (!quest.ImagePath.IsNullOrEmpty())
                                {
                                    if (updatePostModel.Files != null && updatePostModel.Files.Count != 0 &&
                                        !System.IO.File.Exists(Path.Combine(environment.WebRootPath, quest.ImagePath)))
                                    {
                                        IFormFile file = updatePostModel.Files.First(f => f.FileName.Equals(quest.ImagePath));
                                        string saveImage = Path.Combine(environment.WebRootPath + "/images/", file.FileName);
                                        string ext = Path.GetExtension(saveImage);
                                        if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                        {
                                            using (var upload = new FileStream(saveImage, FileMode.Create))
                                            {
                                                await file.CopyToAsync(upload);
                                            }
                                        }
                                    }
                                }
                                if (quest.Id.IsNullOrEmpty())
                                {
                                    quest.Id = Guid.NewGuid().ToString();
                                    await ms.Question.SaveQuestion(new Question
                                    {
                                        Id = quest.Id,
                                        Text = quest.Text,
                                        IdQuestionType = quest.IdQuestionType,
                                        IdTest = test.Id,
                                        ImagePath = quest.ImagePath,
                                        Number = Convert.ToByte(quest.Number),
                                        Weight = quest.Weight
                                    });
                                }
                                else
                                {
                                    QuestionDto? questToDelete = currentQuestions.Find(x => x.Id.Equals(quest.Id));
                                    if (questToDelete != null)
                                    {
                                        currentQuestions.Remove(questToDelete);
                                    }
                                    await ms.Question.SaveQuestion(new Question
                                    {
                                        Id = quest.Id,
                                        Text = quest.Text,
                                        IdQuestionType = quest.IdQuestionType,
                                        ImagePath = quest.ImagePath,
                                        IdTest = test.Id,
                                        Number = Convert.ToByte(quest.Number),
                                        Weight = quest.Weight
                                    });
                                }
                                foreach (JObject answer in quest.Answers)
                                {
                                    AnswerDto answerDto = answer.ToObject<AnswerDto>();
                                    SubsequenceDto subsequenceDto = answer.ToObject<SubsequenceDto>();
                                    FirstPartDto firstPartDto = answer.ToObject<FirstPartDto>();
                                    SecondPartDto secondPartDto = answer.ToObject<SecondPartDto>();
                                    FirstSecondPartDto firstSecondPartDto = answer.ToObject<FirstSecondPartDto>();

                                    if (answerDto is AnswerDto && answerDto.Correct != null)
                                    {
                                        logger.LogInformation($"answerDto -> text={answerDto.Text}, correct={answerDto.Correct}");

                                        if (!answerDto.ImagePath.IsNullOrEmpty())
                                        {
                                            if (updatePostModel.Files != null && updatePostModel.Files.Count != 0 &&
                                                !System.IO.File.Exists(Path.Combine(environment.WebRootPath + "/images/", answerDto.ImagePath)))
                                            {
                                                IFormFile file = updatePostModel.Files.First(f => f.FileName.Equals(answerDto.ImagePath));
                                                string saveImage = Path.Combine(environment.WebRootPath + "/images/", file.FileName);
                                                string ext = Path.GetExtension(saveImage);
                                                if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                                {
                                                    using (var upload = new FileStream(saveImage, FileMode.Create))
                                                    {
                                                        await file.CopyToAsync(upload);
                                                    }
                                                }
                                            }
                                        }
                                        if (answerDto.IdAnswer.HasValue)
                                        {
                                            await ms.Answer.SaveAnswer(new Answer
                                            {
                                                Id = answerDto.IdAnswer.Value,
                                                Text = answerDto.Text,
                                                IdQuestion = quest.Id,
                                                Correct = answerDto.Correct,
                                                ImagePath = answerDto.ImagePath,
                                                Weight = answerDto.Weight
                                            });
                                        }
                                        else
                                        {
                                            await ms.Answer.SaveAnswer(new Answer
                                            {
                                                Text = answerDto.Text,
                                                IdQuestion = quest.Id,
                                                Correct = answerDto.Correct,
                                                ImagePath = answerDto.ImagePath,
                                                Weight = answerDto.Weight
                                            });
                                        }
                                    }
                                    if (subsequenceDto is SubsequenceDto && subsequenceDto.Number != null && subsequenceDto.Number != 0)
                                    {
                                        logger.LogInformation($"subsequenceDto -> text={subsequenceDto.Text}, number={subsequenceDto.Number}");
                                        await ms.Subsequence.SaveSubsequence(new Subsequence
                                        {
                                            Id = subsequenceDto.IdSubsequence,
                                            Text = subsequenceDto.Text,
                                            Number = subsequenceDto.Number,
                                            IdQuestion = quest.Id
                                        });
                                    }
                                    if (firstPartDto is FirstPartDto && !string.IsNullOrEmpty(firstPartDto.IdFirstPart) &&
                                        !string.IsNullOrEmpty(firstPartDto.Text))
                                    {
                                        logger.LogInformation($"firstPartDto -> Id={firstPartDto.IdFirstPart}, Text={firstPartDto.Text}");
                                        await ms.FirstPart.SaveFirstPart(new FirstPart
                                        {
                                            Id = firstPartDto.IdFirstPart,
                                            Text = firstPartDto.Text,
                                            IdQuestion = quest.Id
                                        });
                                    }
                                    if (secondPartDto is SecondPartDto && secondPartDto.IdSecondPart.HasValue &&
                                        !secondPartDto.Text.IsNullOrEmpty() && !secondPartDto.IdFirstPart.IsNullOrEmpty())
                                    {
                                        logger.LogInformation($"secondPatDto -> Id={secondPartDto.IdFirstPart}, text={firstPartDto.Text}, IdFirstPart={secondPartDto.IdFirstPart}");
                                        await ms.SecondPart.SaveSecondPart(new SecondPart
                                        {
                                            Id = secondPartDto.IdSecondPart.Value,
                                            Text = secondPartDto.Text,
                                            IdFirstPart = secondPartDto.IdFirstPart
                                        });
                                    }
                                    if (firstSecondPartDto is FirstSecondPartDto && firstSecondPartDto != null &&
                                        !string.IsNullOrEmpty(firstSecondPartDto.FirstPartText) && !string.IsNullOrEmpty(firstSecondPartDto.SecondPartText))
                                    {
                                        logger.LogInformation($"firstSecondPartDto -> first={firstSecondPartDto.FirstPartText}, second={firstSecondPartDto.SecondPartText}");
                                        string firstPartId = Guid.NewGuid().ToString();
                                        await ms.FirstPart.SaveFirstPart(new FirstPart
                                        {
                                            Id = firstPartId,
                                            Text = firstSecondPartDto.FirstPartText,
                                            IdQuestion = quest.Id
                                        });
                                        await ms.SecondPart.SaveSecondPart(new SecondPart
                                        {
                                            Text = firstSecondPartDto.SecondPartText,
                                            IdFirstPart = firstPartId
                                        });
                                    }
                                }
                            }
                            if (currentQuestions.Count != 0)
                            {
                                foreach (QuestionDto questionToDelete in currentQuestions)
                                {
                                    string questFilePath = environment.WebRootFileProvider.GetFileInfo("images/" + questionToDelete.ImagePath).PhysicalPath;
                                    if (!questionToDelete.ImagePath.IsNullOrEmpty() && System.IO.File.Exists(questFilePath))
                                    {
                                        System.IO.File.Delete(questFilePath);
                                    }
                                    List<Answer> answers = await ms.Answer.GetAnswersByQuestionId(questionToDelete.Id);
                                    foreach (Answer answer in answers)
                                    {
                                        string answerFilePath = environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath;
                                        if (!answer.ImagePath.IsNullOrEmpty() && System.IO.File.Exists(answerFilePath))
                                        {
                                            System.IO.File.Delete(answerFilePath);
                                        }
                                        await ms.Answer.DeleteAnswerById(answer.Id);
                                    }

                                    await ms.Subsequence.DeleteSubsequencesByQuestion(questionToDelete.Id);
                                    await ms.DeleteFirstAndSecondPartsByQuestion(questionToDelete.Id);

                                    await ms.Question.DeleteQuestionById(questionToDelete.Id);
                                }
                            }
                            return Ok(new { message = "Обновление теста успешно" });
                        }
                    }
                    return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
                }
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("GetQuestionsInTestPage")]
        public async Task<IActionResult> GetQuestionsInTestPage([FromHeader] string Authorization, [FromBody] StringIdModel? id, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id) && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetTests ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetTests",
                            UserId = token.IdAdmin,
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

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("AddQuestionInTest")]
        public async Task<IActionResult> AddQuestionInTest([FromHeader] string Authorization, [FromForm] AddPostQuestioModel? addQuestModel)
        {
            if (addQuestModel != null && !string.IsNullOrEmpty(addQuestModel.Question))
            {
                AddQuestionInTestModel? quest = JsonConvert.DeserializeObject<AddQuestionInTestModel>(addQuestModel.Question);
                if (!Authorization.IsNullOrEmpty() && quest != null && !string.IsNullOrEmpty(quest.IdTest) && !quest.Text.IsNullOrEmpty() &&
                    quest.IdQuestionType.HasValue && quest.Answers != null && quest.Answers.Count != 0 && quest.Weight.HasValue)
                {
                    TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                    if (token != null)
                    {
                        if (await ms.IsTokenAdminExpired(token))
                        {
                            return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                        }
                        else
                        {
                            logger.LogInformation($"/admin-api/AddQuestionInTest quest: idTest={quest.IdTest} text={quest.Text}, idQuestionType={quest.IdQuestionType}," +
                                      $"countOfQuestions={quest.Answers.Count}");
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/AddQuestionInTest",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id Теста={quest.IdTest}, Кол-во ответов={quest.Answers.Count}"
                            });

                            if (quest.IdQuestionType == null || !quest.IdQuestionType.HasValue || await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value) == null)
                                return BadRequest(new { message = "Ошибка. Нет такого типа вопроса" });

                            if (await ms.Test.GetTestById(quest.IdTest) == null)
                                return BadRequest(new { message = "Ошибка. Такого теста не существует" });

                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (addQuestModel.Files != null && addQuestModel.Files.Count != 0 &&
                                    !System.IO.File.Exists(Path.Combine(environment.WebRootPath, quest.ImagePath)))
                                {
                                    IFormFile file = addQuestModel.Files.First(f => f.FileName.Equals(quest.ImagePath));
                                    string saveImage = Path.Combine(environment.WebRootPath + "/images/", file.FileName);
                                    string ext = Path.GetExtension(saveImage);
                                    if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                    {
                                        using (var upload = new FileStream(saveImage, FileMode.Create))
                                        {
                                            await file.CopyToAsync(upload);
                                        }
                                    }
                                }
                            }

                            string questId = Guid.NewGuid().ToString();
                            int questNumber = (await ms.Question.GetQuestionDtosByTest(quest.IdTest)).Count + 1;
                            await ms.Question.SaveQuestion(new Question
                            {
                                Id = questId,
                                Text = quest.Text,
                                IdQuestionType = quest.IdQuestionType,
                                IdTest = quest.IdTest,
                                ImagePath = quest.ImagePath,
                                Number = Convert.ToByte(questNumber),
                                Weight = quest.Weight
                            });

                            int answerNumber = 1;
                            foreach (JObject answer in quest.Answers)
                            {
                                SubsequenceDto subsequenceDto = answer.ToObject<SubsequenceDto>();
                                FirstSecondPartDto firstSecondPartDto = answer.ToObject<FirstSecondPartDto>();

                                if (quest.IdQuestionType.Value == 1 || quest.IdQuestionType == 2)
                                {
                                    AnswerDto answerDto = answer.ToObject<AnswerDto>();
                                    if (answerDto is AnswerDto && answerDto.Correct != null)
                                    {
                                        logger.LogInformation($"answerDto -> text={answerDto.Text}, correct={answerDto.Correct}");

                                        if (!answerDto.ImagePath.IsNullOrEmpty())
                                        {
                                            if (addQuestModel.Files != null && addQuestModel.Files.Count != 0 &&
                                                !System.IO.File.Exists(Path.Combine(environment.WebRootPath + "/images/", answerDto.ImagePath)))
                                            {
                                                IFormFile file = addQuestModel.Files.First(f => f.FileName.Equals(answerDto.ImagePath));
                                                string saveImage = Path.Combine(environment.WebRootPath + "/images/", file.FileName);
                                                string ext = Path.GetExtension(saveImage);
                                                if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                                {
                                                    using (var upload = new FileStream(saveImage, FileMode.Create))
                                                    {
                                                        await file.CopyToAsync(upload);
                                                    }
                                                }
                                            }
                                        }
                                        if (answerDto.IdAnswer.HasValue)
                                        {
                                            await ms.Answer.SaveAnswer(new Answer
                                            {
                                                Id = answerDto.IdAnswer.Value,
                                                Number = answerNumber,
                                                Text = answerDto.Text,
                                                IdQuestion = questId,
                                                Correct = answerDto.Correct,
                                                ImagePath = answerDto.ImagePath,
                                                Weight = answerDto.Weight
                                            });
                                            answerNumber++;
                                        }
                                        else
                                        {
                                            await ms.Answer.SaveAnswer(new Answer
                                            {
                                                Text = answerDto.Text,
                                                IdQuestion = questId,
                                                Correct = answerDto.Correct,
                                                Number = answerNumber,
                                                ImagePath = answerDto.ImagePath,
                                                Weight = answerDto.Weight
                                            });
                                            answerNumber++;
                                        }
                                    }
                                }
                                if (quest.IdQuestionType.Value == 4)
                                {
                                    if (subsequenceDto is SubsequenceDto && subsequenceDto.Number != null && subsequenceDto.Number != 0)
                                    {
                                        logger.LogInformation($"subsequenceDto -> text={subsequenceDto.Text}, number={subsequenceDto.Number}");
                                        await ms.Subsequence.SaveSubsequence(new Subsequence
                                        {
                                            Id = subsequenceDto.IdSubsequence,
                                            Text = subsequenceDto.Text,
                                            Number = subsequenceDto.Number,
                                            IdQuestion = questId
                                        });
                                    }
                                }
                                if (quest.IdQuestionType.Value == 3)
                                {
                                    if (firstSecondPartDto is FirstSecondPartDto && firstSecondPartDto != null &&
                                        !string.IsNullOrEmpty(firstSecondPartDto.FirstPartText) && !string.IsNullOrEmpty(firstSecondPartDto.SecondPartText))
                                    {
                                        logger.LogInformation($"firstSecondPartDto -> first={firstSecondPartDto.FirstPartText}, second={firstSecondPartDto.SecondPartText}");
                                        string firstPartId = Guid.NewGuid().ToString();
                                        await ms.FirstPart.SaveFirstPart(new FirstPart
                                        {
                                            Id = firstPartId,
                                            Text = firstSecondPartDto.FirstPartText,
                                            IdQuestion = questId
                                        });
                                        await ms.SecondPart.SaveSecondPart(new SecondPart
                                        {
                                            Text = firstSecondPartDto.SecondPartText,
                                            IdFirstPart = firstPartId
                                        });
                                    }
                                }
                            }
                        }
                        return Ok(new { message = "Добавление вопроса успешно" });
                    }
                    return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
                }
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("UpdateQuestionInTest")]
        public async Task<IActionResult> UpdateQuestionInTest([FromHeader] string Authorization, [FromForm] AddPostQuestioModel? addQuestModel)
        {
            if (addQuestModel != null && !string.IsNullOrEmpty(addQuestModel.Question))
            {
                UpdateQuestionInTest? quest = JsonConvert.DeserializeObject<UpdateQuestionInTest>(addQuestModel.Question);
                if (!Authorization.IsNullOrEmpty() && quest != null && !string.IsNullOrEmpty(quest.IdQuestion) && !string.IsNullOrEmpty(quest.IdTest) && !quest.Text.IsNullOrEmpty() &&
                    quest.IdQuestionType.HasValue && quest.Answers != null && quest.Answers.Count != 0 && quest.Weight.HasValue)
                {
                    TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                    if (token != null)
                    {
                        if (await ms.IsTokenAdminExpired(token))
                        {
                            return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                        }
                        else
                        {
                            logger.LogInformation($"/admin-api/UpdateQuestionInTest quest: idTest={quest.IdTest} text={quest.Text}, idQuestionType={quest.IdQuestionType}," +
                                      $"countOfQuestions={quest.Answers.Count}");
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/UpdateQuestionInTest",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id Теста={quest.IdTest}, Кол-во ответов={quest.Answers.Count}"
                            });

                            if (await ms.Question.GetQuestionById(quest.IdQuestion) == null)
                                return BadRequest(new { message = "Ошибка. Такого вопроса нет" });

                            if (quest.IdQuestionType == null || !quest.IdQuestionType.HasValue || await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value) == null)
                                return BadRequest(new { message = "Ошибка. Нет такого типа вопроса" });

                            if (await ms.Test.GetTestById(quest.IdTest) == null)
                                return BadRequest(new { message = "Ошибка. Такого теста не существует" });

                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (addQuestModel.Files != null && addQuestModel.Files.Count != 0 &&
                                    !System.IO.File.Exists(Path.Combine(environment.WebRootPath, quest.ImagePath)))
                                {
                                    IFormFile file = addQuestModel.Files.First(f => f.FileName.Equals(quest.ImagePath));
                                    string saveImage = Path.Combine(environment.WebRootPath + "/images/", file.FileName);
                                    string ext = Path.GetExtension(saveImage);
                                    if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                    {
                                        using (var upload = new FileStream(saveImage, FileMode.Create))
                                        {
                                            await file.CopyToAsync(upload);
                                        }
                                    }
                                }
                            }

                            await ms.Question.SaveQuestion(new Question
                            {
                                Id = quest.IdQuestion,
                                Text = quest.Text,
                                IdQuestionType = quest.IdQuestionType,
                                IdTest = quest.IdTest,
                                ImagePath = quest.ImagePath,
                                Weight = quest.Weight
                            });

                            int answerNumber = 1;
                            foreach (JObject answer in quest.Answers)
                            {
                                SubsequenceDto subsequenceDto = answer.ToObject<SubsequenceDto>();
                                FirstSecondPartDto firstSecondPartDto = answer.ToObject<FirstSecondPartDto>();

                                if (quest.IdQuestionType.Value == 1 || quest.IdQuestionType == 2)
                                {
                                    AnswerDto answerDto = answer.ToObject<AnswerDto>();
                                    if (answerDto is AnswerDto && answerDto.Correct != null)
                                    {
                                        logger.LogInformation($"answerDto -> text={answerDto.Text}, correct={answerDto.Correct}");

                                        if (!answerDto.ImagePath.IsNullOrEmpty())
                                        {
                                            if (addQuestModel.Files != null && addQuestModel.Files.Count != 0 &&
                                                !System.IO.File.Exists(Path.Combine(environment.WebRootPath + "/images/", answerDto.ImagePath)))
                                            {
                                                IFormFile file = addQuestModel.Files.First(f => f.FileName.Equals(answerDto.ImagePath));
                                                string saveImage = Path.Combine(environment.WebRootPath + "/images/", file.FileName);
                                                string ext = Path.GetExtension(saveImage);
                                                if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                                {
                                                    using (var upload = new FileStream(saveImage, FileMode.Create))
                                                    {
                                                        await file.CopyToAsync(upload);
                                                    }
                                                }
                                            }
                                        }
                                        if (await ms.Answer.GetAnswerById(answerDto.IdAnswer.Value) != null)
                                        {
                                            await ms.Answer.SaveAnswer(new Answer
                                            {
                                                Id = answerDto.IdAnswer.Value,
                                                Number = answerNumber,
                                                Text = answerDto.Text,
                                                IdQuestion = quest.IdQuestion,
                                                Correct = answerDto.Correct,
                                                ImagePath = answerDto.ImagePath,
                                                Weight = answerDto.Weight
                                            });
                                            answerNumber++;
                                        }
                                    }
                                }
                                if (quest.IdQuestionType.Value == 4)
                                {
                                    if (subsequenceDto is SubsequenceDto && subsequenceDto.IdSubsequence != null && subsequenceDto.IdSubsequence != 0 && subsequenceDto.Number != null && subsequenceDto.Number != 0)
                                    {
                                        logger.LogInformation($"subsequenceDto -> text={subsequenceDto.Text}, number={subsequenceDto.Number}");
                                        if (await ms.Subsequence.GetSubsequenceById(subsequenceDto.IdSubsequence) != null)
                                        {
                                            await ms.Subsequence.SaveSubsequence(new Subsequence
                                            {
                                                Id = subsequenceDto.IdSubsequence,
                                                Text = subsequenceDto.Text,
                                                Number = subsequenceDto.Number,
                                                IdQuestion = quest.IdQuestion
                                            });
                                        }
                                    }
                                }
                                if (quest.IdQuestionType.Value == 3)
                                {
                                    if (firstSecondPartDto is FirstSecondPartDto && firstSecondPartDto != null && firstSecondPartDto.IdFirstPart != null && firstSecondPartDto.IdSecondPart != null &&
                                        firstSecondPartDto.IdSecondPart.Value != 0 && firstSecondPartDto.IdSecondPart != 0 &&
                                        !string.IsNullOrEmpty(firstSecondPartDto.FirstPartText) && !string.IsNullOrEmpty(firstSecondPartDto.SecondPartText))
                                    {
                                        logger.LogInformation($"firstSecondPartDto -> first={firstSecondPartDto.FirstPartText}, second={firstSecondPartDto.SecondPartText}");
                                        if (await ms.FirstPart.GetFirstPartById(firstSecondPartDto.IdFirstPart) != null && await ms.SecondPart.GetSecondPartById(firstSecondPartDto.IdSecondPart.Value) != null)
                                        {
                                            await ms.FirstPart.SaveFirstPart(new FirstPart
                                            {
                                                Id = firstSecondPartDto.IdFirstPart,
                                                Text = firstSecondPartDto.FirstPartText,
                                                IdQuestion = quest.IdQuestion
                                            });
                                            await ms.SecondPart.SaveSecondPart(new SecondPart
                                            {
                                                Id = firstSecondPartDto.IdSecondPart.Value,
                                                Text = firstSecondPartDto.SecondPartText,
                                                IdFirstPart = firstSecondPartDto.IdFirstPart
                                            });
                                        }
                                    }
                                }
                            }
                        }
                        return Ok(new { message = "Добавление вопроса успешно" });
                    }
                    return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
                }
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("DeleteQuestionInTest")]
        public async Task<IActionResult> DeleteQuestionInTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        Question? quest = await ms.Question.GetQuestionById(id.Id);
                        if (quest == null)
                        {
                            return NotFound(new { message = "Ошибка. Такого вопроса нет" });
                        }

                        logger.LogInformation($"/admin-api/DeleteQuestionInTest :id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteQuestionInTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"id вопроса={id.Id}"
                        });
                        if (!quest.ImagePath.IsNullOrEmpty())
                        {
                            string path = environment.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath;
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        List<Answer>? answers = await ms.Answer.GetAnswersByQuestionId(id.Id);
                        await ms.Answer.DeleteAnswersByQuestion(id.Id);
                        foreach (Answer answer in answers)
                        {
                            if (!answer.ImagePath.IsNullOrEmpty())
                            {
                                string path = environment.WebRootFileProvider.GetFileInfo("/images/" + answer.ImagePath).PhysicalPath;
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                            }
                        }
                        await ms.Subsequence.DeleteSubsequencesByQuestion(id.Id);
                        await ms.DeleteFirstAndSecondPartsByQuestion(id.Id);
                        await ms.Question.DeleteQuestionById(id.Id);

                        int number = 1;
                        foreach (var question in (await ms.Question.GetQuestionsByTest(quest.IdTest)).OrderBy(x => x.Number).ToList())
                        {
                            question.Number = Convert.ToByte(number);
                            await ms.Question.SaveQuestion(question);
                            number++;
                        }
                        return Ok(new { message = "Вопрос удален" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Test" })]
        [HttpPost("DeleteTest")]
        public async Task<IActionResult> DeleteTest([FromHeader] string Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteTest testId={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Теста={id.Id}"
                        });
                        if (await ms.Test.GetTestById(id.Id) != null)
                        {
                            await ms.DeleteTestById(id.Id, environment);
                            return Ok(new { message = "Тест удален успешно" });
                        }
                        return NotFound(new { message = "Ошибка. Тест не найден" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
        /*
         *  Purpose
         */
        [SwaggerOperation(Tags = new[] { "Admin/Purpose" })]
        [HttpGet("GetPurposes")]
        public async Task<IActionResult> GetPurposes([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPurposess ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPurposess",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        return Ok(await ms.TestPurpose.GetAllPurposeAdminModels());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Purpose" })]
        [HttpGet("GetPurposesPage")]
        public async Task<IActionResult> GetPurposesPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPurposess ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPurposess",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        var Allpurposes = (await ms.TestPurpose.GetAllPurposeAdminModels())
                                                    .OrderByDescending(x => x.DatatimePurpose);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allpurposes.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var purposes = Allpurposes
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(purposes);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Purpose" })]
        [HttpPost("GetPurposesByEmployeeId")]
        public async Task<IActionResult> GetPurposesByEmployeeId([FromHeader] string Authorization, [FromBody] StringIdModel id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPurposessByEmployeeId id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPurposesByEmployeeId",
                            UserId = $"",
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Сотрудника={id.Id}"
                        });

                        List<TestPurposeDto> purposes = (await ms.TestPurpose.GetAllTestPurposeDtos())
                                         .Where(x => x != null && x.IdEmployee != null && x.IdTest != null && x.IdEmployee.Equals(id.Id)).ToList();

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
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Purpose" })]
        [HttpPost("GetPurposesPageByEmployeeId")]
        public async Task<IActionResult> GetPurposesPageByEmployeeId([FromHeader] string Authorization, [FromBody] StringIdModel id, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !id.Id.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPurposessByEmployeeId id={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPurposesByEmployeeId",
                            UserId = $"",
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Сотрудника={id.Id}"
                        });

                        var Allpurposess = (await ms.TestPurpose.GetAllTestPurposeDtos())
                                                    .Where(x => x.IdEmployee.Equals(id.Id))
                                                    .OrderByDescending(x => x.DatatimePurpose);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allpurposess.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var purposes = Allpurposess
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        if (Allpurposess.Count() == 0)
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
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Purpose" })]
        [HttpPost("AddPurpose")]
        public async Task<IActionResult> AddPurpose([FromHeader] string Authorization, [FromBody] AddTestPurposeModel? purpose)
        {
            if (!Authorization.IsNullOrEmpty() && purpose != null && !purpose.IdTest.IsNullOrEmpty() &&
                !purpose.IdEmployee.IsNullOrEmpty())//&& !purpose.DatatimePurpose.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if (await ms.Test.GetTestById(purpose.IdTest) == null)
                            return BadRequest(new { message = "Ошибка. Такого теста нет" });

                        Employee? employee = await ms.Employee.GetEmployeeById(purpose.IdEmployee);
                        if (employee == null)
                            return NotFound(new { message = "Ошибка. Такого сотрудника нет" });

                        if (await ms.TestPurpose.GetTestPurposeByEmployeeTestId(purpose.IdTest, purpose.IdEmployee) != null)
                            return BadRequest(new { message = "Ошибка. Этот тест уже назначен этому пользованелю" });

                        Subdivision? subdivision = await ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value);
                        if (subdivision != null)
                        {
                            if (await ms.CompetenciesForGroup.GetCompetenciesForGroupByEmployeeTestId(purpose.IdTest, subdivision.IdGroupPositions.Value) == null)
                            {
                                return NotFound(new { message = $"Ошибка. Этот тест не назначен группе:id группы={subdivision.IdGroupPositions}" });
                            }
                        }


                        logger.LogInformation($"/admin-api/AddPurpose employeeId={purpose.IdEmployee}, testId={purpose.IdTest}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddPurpose",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Сотрудника={purpose.IdEmployee}, Id теста={purpose.IdTest}"
                        });
                        await ms.TestPurpose.SaveTestPurpose(purpose);
                        return Ok(new { message = "Тест назначен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        /*[HttpPost("AddPurposesBySubdivision")]
        public async Task<IActionResult> AddPurposesBySubdivision([FromHeader] string Authorization, string? testId, int? idSubdivision, string? time)
        {
            if (!Authorization.IsNullOrEmpty() && !testId.IsNullOrEmpty() && idSubdivision.HasValue && !time.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddPurposesBySubdivision testId={testId}, idSubdivision={idSubdivision}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddPurpose",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Отдела={idSubdivision}, Id теста={testId}, Дата сдачи={time}"
                        });
                        await ms.TestPurpose.SavePurposeBySubdivisionId(testId, idSubdivision.Value, DateTime.Parse(time));
                        return Ok(new { message = "Тесты назначены" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        /*[HttpPost("AddPurposesByRange")]
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
        }*/

        [SwaggerOperation(Tags = new[] { "Admin/Purpose" })]
        [HttpPost("UpdatePurpose")]
        public async Task<IActionResult> UpdatePurpose([FromHeader] string Authorization, [FromBody] UpdateTestPurposeModel? purpose)
        {
            if (!Authorization.IsNullOrEmpty() && purpose != null && purpose.Id.HasValue &&
                !purpose.IdTest.IsNullOrEmpty())// && !purpose.DatatimePurpose.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if (await ms.TestPurpose.GetTestPurposeById(purpose.Id.Value) != null)
                        {
                            Employee employee = await ms.Employee.GetEmployeeById(purpose.IdEmployee);
                            if (employee == null)
                                return NotFound(new { message = "Ошибка. Такого сотрудника нет" });

                            Subdivision subdivision = await ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value);
                            if (subdivision != null)
                            {
                                if (await ms.CompetenciesForGroup.GetCompetenciesForGroupByEmployeeTestId(purpose.IdTest, subdivision.IdGroupPositions.Value) == null)
                                {
                                    return NotFound(new { message = $"Ошибка. Этот тест не назначен группе:id группы={subdivision.IdGroupPositions}" });
                                }
                            }

                            logger.LogInformation($"/admin-api/UpdatePurpose purposeId={purpose.Id}");
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/UpdatePurpose",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id Сотрудника={purpose.IdEmployee}, Id теста={purpose.IdTest}"
                            });
                            await ms.TestPurpose.SaveTestPurpose(purpose);
                            return Ok(new { message = "Назначение обновлено" });
                        }
                        return NotFound(new { message = "Ошибка. Такого назначения не существует" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Purpose" })]
        [HttpPost("DeletePurpose")]
        public async Task<IActionResult> DeletePurpose([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeletePurpose purposeId={id.Id}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeletePurpose",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Назначения={id.Id}"
                        });
                        if (await ms.TestPurpose.GetTestPurposeById(id.Id.Value) != null)
                        {
                            await ms.TestPurpose.DeleteTestPurposeById(id.Id.Value);
                            return Ok(new { message = "Назначение удалено" });
                        }
                        return NotFound(new { message = "Ошибка. Такого назначения не существует" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        /*
         *  Result
         */
        //todo edit query GetResults
        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpGet("#GetEmployeeResultsOfSubcompetence")]
        public async Task<IActionResult> GetEmployeeResultsOfSubcompetence([FromHeader] string Authorization)//, [FromBody] ResultQuerryModel query)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetEmployeeResultsOfSubcompetence ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetEmployeeResultsOfSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        List<EmployeeResultSubcompetenceDto> list = await ms.EmployeeResultSubcompetence.GetAllEmployeeResultSubcompetenceDtos();

                        return Ok(list);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpGet("#DeleteEmployeeResultsOfSubcompetence")]
        public async Task<IActionResult> DeleteEmployeeResultsOfSubcompetence([FromHeader] string Authorization)//, [FromBody] ResultQuerryModel query)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetEmployeeResultsOfSubcompetence ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetEmployeeResultsOfSubcompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        List<EmployeeResultSubcompetenceDto> list = await ms.EmployeeResultSubcompetence.GetAllEmployeeResultSubcompetenceDtos();
                        foreach(EmployeeResultSubcompetenceDto dto in list)
                        {
                            await ms.EmployeeResultSubcompetence.DeleteEmployeeResultSubcompetenceById(dto.Id);
                        }
                        return Ok(new { message = "Резульаты по подкомпентенциям удалены"});
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpGet("GetResultWithSubcompetences")]
        public async Task<IActionResult> GetResultWithSubcompetences([FromHeader] string Authorization)//, [FromBody] ResultQuerryModel query)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetResultSubcompetences ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetResultSubCompetences",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        List<EmployeeResultSubcompetenceModel> results = (await ms.GetAllEmployeeResultSubcompetenceModels())
                                                                            .OrderByDescending(x => x.Id)
                                                                            .ToList();
                        /*foreach (var result in results)
                        {
                            var testScores = await ms.TestScore.GetTestScoresByTest(result.Result.Test.Id);
                            if (testScores.Any())
                            {
                                int countOfScores = testScores.Count;
                                foreach (var score in testScores)
                                {
                                    int employeeScore = result.ScoreFrom.Value;
                                    if (employeeScore >= score.MinValue && employeeScore <= score.MaxValue)
                                    {
                                        EmployeeResultSubcompetenceModel model = results.Find(x => x.Equals(result));

                                        if (model != null)
                                        {

                                            model.ResultLevel = score.Description;
                                            model.NumberPoints = (double)((double)3 / countOfScores) * score.NumberPoints;

                                            break;
                                        }
                                    }
                                }
                            }

                            foreach (var subcompetenceResult in result.SubcompetenceResults) {
                                var subcompetenceScores = await ms.SubcompetenceScore.GetSubcompetenceScoresBySubCompetence(subcompetenceResult.Subcompetence.Id.Value);
                                if (subcompetenceScores.Any())
                                {
                                    int countOfScores = subcompetenceScores.Count;
                                    foreach (var subcompetenceScore in subcompetenceScores)
                                    {
                                        int employeeSubcompetenceScore = subcompetenceResult.Result;
                                        if (employeeSubcompetenceScore >= subcompetenceScore.MinValue &&
                                            employeeSubcompetenceScore <= subcompetenceScore.MaxValue)
                                        {
                                            subcompetenceResult.Description = subcompetenceScore.Description;
                                            subcompetenceResult.NumberPoints = (3 / countOfScores) * subcompetenceScore.NumberPoints.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                        }*/
                        
                        //list.ForEach(x =>
                        //        x.ResultLevel = RateLogic.RateLogic.GetLevelTestPoit(x.Result.Test.Id, x.ScoreFrom.Value));

                        return Ok(await ms.CalculateEmployeeresultsWithSubcompetence(results));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpGet("GetResultWithSubcompetencesPage")]
        public async Task<IActionResult> GetResultWithSubcompetencesPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)//, [FromBody] ResultQuerryModel query)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetResults ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetResults",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });


                        var Allresults = (await ms.GetAllEmployeeResultSubcompetenceModels())
                                                    .OrderByDescending(x => x.Id)
                                                    .ToList();

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allresults.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var results = Allresults
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(await ms.CalculateEmployeeresultsWithSubcompetence(results));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpPost("GetResultWithSubcompetencesByEmployee")]
        public async Task<IActionResult> GetResultWithSubcompetencesByEmployee([FromHeader] string? Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetResultWithSubcompetencesByEmployee");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetResultWithSubcompetencesByEmployee",
                        UserId = $"{token.IdAdmin}",
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"Id={id.Id}"
                    });

                    if (await ms.Employee.GetEmployeeById(id.Id) == null)
                    {
                        return NotFound(new { message = "Ошибка. Такого пользователя нет" });
                    }

                    List<EmployeeResultSubcompetenceModel>? results = (await ms.GetAllEmployeeResultSubcompetenceModelsByEmployee(id.Id))
                                                                            .OrderByDescending(x => x.Id)
                                                                            .ToList();

                    if (results != null && results.Count != 0)
                    { 
                        return Ok(await ms.CalculateEmployeeresultsWithSubcompetence(results));
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

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpPost("GetResultWithSubcompetencesPageByEmployee")]
        public async Task<IActionResult> GetResultWithSubcompetencesPageByEmployee([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams, [FromBody] StringIdModel? id)//, [FromBody] ResultQuerryModel query)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetResultWithSubcompetencesPageByEmployee");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetResultWithSubcompetences",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        if (await ms.Employee.GetEmployeeById(id.Id) == null)
                        {
                            return NotFound(new { message = "Ошибка. Такого пользователя нет" });
                        }

                        var Allresults = (await ms.GetAllEmployeeResultSubcompetenceModelsByEmployee(id.Id))
                                                    .OrderByDescending(x => x.Id)
                                                    .ToList();

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allresults.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var results = Allresults
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(await ms.CalculateEmployeeresultsWithSubcompetence(results));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpPost("GetResultWithSubcompetencesByEmployeeResultId")]
        public async Task<IActionResult> GetResultWithSubcompetencesByEmployeeResultId([FromHeader] string? Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetResultWithSubcompetences");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetResultWithSubcompetences",
                        UserId = $"{token.IdAdmin}",
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"Id={id.Id}"
                    });

                    if (await ms.EmployeeResult.GetEmployeeResultById(id.Id.Value) == null)
                    {
                        return NotFound(new { message = "Ошибка. Такого результата пользователя нет" });
                    }

                    List<EmployeeResultSubcompetenceModel>? results = (await ms.GetAllEmployeeResultSubcompetenceModelsByEmployeeResultId(id.Id.Value))
                                                                            .OrderByDescending(x => x.Id)
                                                                            .ToList(); ;

                    if (results != null && results.Count != 0)
                    {   
                        return Ok(await ms.CalculateEmployeeresultsWithSubcompetence(results));
                    }
                    else
                    {
                        return BadRequest(new { message = "Результат не найден" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpGet("GetResults")]
        public async Task<IActionResult> GetResults([FromHeader] string Authorization)//, [FromBody] ResultQuerryModel query)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetResults ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetResults",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        List<EmployeeResultModel> list = (await ms.GetAllEmployeeResultModels())
                                                            .OrderByDescending(x => x.Id)
                                                            .ToList();

                        //list.ForEach(x =>
                        //    x.ResultLevel = RateLogic.RateLogic.GetLevelTestPoit(x.Result.Test.Id, x.ScoreFrom.Value));
                       

                        /*if (query.IdSubdivision.HasValue && query.IdSubdivision != 0)
                        {
                            list = list.Where(x => x.Employee.Subdivision.Id == query.IdSubdivision).ToList();
                        }
                        if (!query.FirstName.IsNullOrEmpty())
                        {
                            list = list.Where(x => x.Employee.FirstName.Contains(query.FirstName)).ToList();
                        }
                        if (!query.SecondName.IsNullOrEmpty())
                        {
                            list = list.Where(x => x.Employee.SecondName.Contains(query.SecondName)).ToList();
                        }
                        if (!query.LastName.IsNullOrEmpty())
                        {
                            list = list.Where(x => x.Employee.LastName.Contains(query.LastName)).ToList();
                        }
                        if (!query.SortType.IsNullOrEmpty())
                        {
                            if (query.SortType.Equals("fname↑"))//↑
                            {
                                list = list.OrderBy(x => x.Employee.FirstName).ToList();
                            }
                            if (query.SortType.Equals("fname↓"))//↓
                            {
                                list = list.OrderByDescending(x => x.Employee.FirstName).ToList();
                            }

                            if (query.SortType.Equals("sname↑"))//↑
                            {
                                list = list.OrderBy(x => x.Employee.SecondName).ToList();
                            }
                            if (query.SortType.Equals("sname↓"))//↓
                            {
                                list = list.OrderByDescending(x => x.Employee.SecondName).ToList();
                            }

                            if (query.SortType.Equals("lname↑"))//↑
                            {
                                list = list.OrderBy(x => x.Employee.LastName).ToList();
                            }
                            if (query.SortType.Equals("lname↓"))//↓
                            {
                                list = list.OrderByDescending(x => x.Employee.LastName).ToList();
                            }

                            if (query.SortType.Equals("score↑"))//↑
                            {
                                list = list.OrderBy(x => x.ScoreFrom).ToList();
                            }
                            if (query.SortType.Equals("score↓"))//↓
                            {
                                list = list.OrderByDescending(x => x.ScoreFrom).ToList();
                            }
                        }*/
                        return Ok(await ms.CalculateEmployeeResults(list));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpGet("GetResultsPage")]
        public async Task<IActionResult> GetResultsPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)//, [FromBody] ResultQuerryModel query)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetResults ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetResults",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        var Allresults = (await ms.GetAllEmployeeResultModels())
                                                    .OrderByDescending(x => x.Id);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Allresults.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var results = Allresults
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(await ms.CalculateEmployeeResults(results));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpPost("GetResultsBySubdivision")]
        public async Task<IActionResult> GetResultsBySubdivision([FromHeader] string Authorization, [FromBody] IntIdModel id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && id.Id > 0)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetResults ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetResults",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        if (await ms.Subdivision.GetSubdivisionById(id.Id.Value) == null)
                            return BadRequest(new { message = "Ошибка. Такой должности нет" });

                        List<EmployeeResultModel> results = (await ms.GetAllEmployeeResultModels())
                                                            .Where(x => x!= null &&
                                                                   x.Employee != null &&
                                                                   x.Employee.Subdivision.Id != null &&
                                                                   x.Employee.Subdivision.Id.Equals(id.Id.Value))
                                                            .OrderByDescending(x => x.Id)
                                                            .ToList();

                        return Ok(await ms.CalculateEmployeeResults(results));
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpPost("GetResultsByEmployee")]
        public async Task<IActionResult> GetResultsByEmployee([FromHeader] string? Authorization, [FromBody] StringIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetTestResultsByEmployee");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetTestResultsByEmployee",
                        UserId = $"{token.IdAdmin}",
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"EmployeeId={id.Id}"
                    });

                    if (await ms.Employee.GetEmployeeById(id.Id) == null)
                    {
                        return NotFound(new { message = "Ошибка. Такого пользователя нет" });
                    }

                    List<EmployeeResultModel>? results = (await ms.GetAllEmployeeResultModelsByEmployeeId(id.Id))
                                                            .OrderByDescending(x => x.Id)
                                                            .ToList();


                    if (results != null && results.Count != 0)
                    { 
                        return Ok(await ms.CalculateEmployeeResults(results));
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

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpPost("GetResultsPageByEmployee")]
        public async Task<IActionResult> GetResultsPageByEmployee([FromHeader] string? Authorization, [FromBody] StringIdModel? id, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && id != null && !string.IsNullOrEmpty(id.Id) && pageParams != null && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetTestResultsByEmployee");
                    await ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetTestResultsByEmployee",
                        UserId = $"{token.IdAdmin}",
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"EmployeeId={id.Id}"
                    });

                    if (await ms.Employee.GetEmployeeById(id.Id) == null)
                    {
                        return NotFound(new { message = "Ошибка. Такого пользователя нет" });
                    }

                    var allResults = (await ms.GetAllEmployeeResultModelsByEmployeeId(id.Id))
                                                    .OrderByDescending(x => x.Id);

                    var pageHeader = new PageHeader(pageParams.PageNumber.Value, allResults.Count(), pageParams.ItemsPerPage.Value);
                    Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                    var results = allResults
                        .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                        .Take(pageParams.ItemsPerPage.Value)
                        .ToList();


                    if (allResults != null && allResults.Count() != 0)
                    {
                        return Ok(await ms.CalculateEmployeeResults(results));
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

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpPost("GetEmployeeResultAnswers")]
        public async Task<IActionResult> GetEmployeeResultAnswers([FromHeader] string Authorization, [FromBody] StringIdModel ResultId)//, [FromBody] ResultQuerryModel query)
        {
            if (!Authorization.IsNullOrEmpty() && ResultId != null && !ResultId.Id.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetEmployeeResultAnswers");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetEmployeeResultAnswers",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id результата={ResultId.Id}"
                        });

                        Result result = (await ms.Result.GetResultById(ResultId.Id));
                        if (result == null)
                            return NotFound(new { message = "Ошибка. такого результата нет" });
                        EmployeeResult employeeResult = (await ms.EmployeeResult.GetEmployeeResultByResultId(result.Id));
                        List<EmployeeAnswer>? employeeAnswers = await ms.EmployeeAnswer.GetAllEmployeeAnswersByResultId(result.Id);
                        List<EmployeeSubsequence>? employeeSubs = await ms.EmployeeSubsequence.GetAllEmployeeSubsequencesByResultId(result.Id);
                        List<EmployeeMatching>? employeeMatchs = await ms.EmployeeMatching.GetAllEmployeeMatchingsByResultId(result.Id);


                        Test? test = await ms.Test.GetTestById(result.IdTest);
                        if (test == null)
                            return NotFound(new { message = "Ошибка. Тест не найден" });

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
                        return Ok(testDto);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpPost("DeleteResults")]
        public async Task<IActionResult> DeleteResults([FromHeader] string Authorization)//!!!
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteResults");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "user-api/DeleteResults",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        List<EmployeeResultDto> employeeResults = await ms.EmployeeResult.GetAllEmployeeResultDtos();
                        foreach (EmployeeResultDto result in employeeResults)
                        {
                            await ms.EmployeeAnswer.DeleteEmployeeAnswersByResult(result.IdResult);
                            await ms.EmployeeMatching.DeleteEmployeeMatchingByResult(result.IdResult);
                            await ms.EmployeeSubsequence.DeleteEmployeeSubsequenceByResult(result.IdResult);
                            await ms.EmployeeResult.DeleteEmployeeResultById(result.Id.Value);
                            await ms.Result.DeleteResultById(result.IdResult);
                        }
                        return Ok(new { message = "Результаты удалены" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Result" })]
        [HttpPost("DeleteResult")]
        public async Task<IActionResult> DeleteResult([FromHeader] string Authorization, [FromBody] IntIdModel resultId)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteResult ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteResult",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        EmployeeResult? employeeResults = await ms.EmployeeResult.GetEmployeeResultById(resultId.Id.Value);
                        if (employeeResults != null)
                        {
                            await ms.EmployeeAnswer.DeleteEmployeeAnswersByResult(employeeResults.IdResult);
                            await ms.EmployeeMatching.DeleteEmployeeMatchingByResult(employeeResults.IdResult);
                            await ms.EmployeeSubsequence.DeleteEmployeeSubsequenceByResult(employeeResults.IdResult);
                            await ms.Result.DeleteResultById(employeeResults.IdResult);
                            await ms.EmployeeResult.DeleteEmployeeResultById(resultId.Id.Value);
                            return Ok(new { message = "Результаты удалены" });
                        }
                        return BadRequest(new { message = "Ошибка. Такого результата нет" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        /*
         *  Log
         */
        [SwaggerOperation(Tags = new[] { "Admin/Log" })]
        [HttpGet("GetLogs")]
        public async Task<IActionResult> GetLogs([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/Getlogs ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/Getlogs",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        return Ok(await ms.Log.GetAllLogDtos());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [SwaggerOperation(Tags = new[] { "Admin/Log" })]
        [HttpGet("GetLogsPage")]
        public async Task<IActionResult> GetLogsPage([FromHeader] string Authorization, [FromQuery] PageParamsModel pageParams)
        {
            if (!Authorization.IsNullOrEmpty() && pageParams != null && pageParams.PageNumber.HasValue && pageParams.ItemsPerPage.HasValue)
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/Getlogs ");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/Getlogs",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        var Alllogs = (await ms.Log.GetAllLogDtos())
                                                    .OrderByDescending(x => x.Id);

                        var pageHeader = new PageHeader(pageParams.PageNumber.Value, Alllogs.Count(), pageParams.ItemsPerPage.Value);
                        Response.Headers.Add("PageHeader", JsonConvert.SerializeObject(pageHeader));

                        var logs = Alllogs
                            .Skip((pageParams.PageNumber.Value - 1) * pageParams.ItemsPerPage.Value)
                            .Take(pageParams.ItemsPerPage.Value)
                            .ToList();

                        return Ok(logs);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
    }
}
