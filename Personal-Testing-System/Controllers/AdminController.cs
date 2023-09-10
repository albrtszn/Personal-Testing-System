﻿using DataBase;
using DataBase.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;
using Personal_Testing_System.Services;
using System.Drawing;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Xceed.Document.NET;
using Xceed.Words.NET;

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
        public AdminController(ILogger<AdminController> _logger, MasterService _masterService,
                               IWebHostEnvironment _environmentironment)//, EFDbContext db)
        {
            logger = _logger;
            ms = _masterService;
            environment = _environmentironment;
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
        /*
         *  TEST METHODS
         */
        [HttpPost("InitDB")]
        public async Task<IActionResult> InitDB()
        {
            if (!(await ms.Subdivision.GetAllSubdivisions()).Any())
            {
                await ms.Subdivision.SaveSubdivision(new Subdivision
                {
                    Name = "Отдел кадров"
                });

                await ms.Subdivision.SaveSubdivision(new Subdivision
                {
                    Name = "Инженерный цех"
                });
            }
            await ms.Admin.GetAllAdmins();
            if (!(await ms.Admin.GetAllAdmins()).Any())
            {
                await ms.Admin.SaveAdmin(new Admin
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Евгений",
                    SecondName = "Жма",
                    LastName = "Дворцов",
                    Login = "admin",
                    Password = "password",
                    //DateOfBirth = DateOnly.Parse("01.01.2000"),
                    IdSubdivision = (await ms.Subdivision.GetAllSubdivisions()).Find(x => x.Name.Equals("Отдел кадров")).Id
                });
            }

            if (!(await ms.TestType.GetAllCompetences()).Any())
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
        }

        [HttpGet("TestGetAdmins")]
        public async Task<IActionResult> TestGetAdmins()
        {
            return Ok(ms.Admin.GetAllAdminDtos());
        }
        [HttpGet("TestGetAdminTokens")]
        public async Task<IActionResult> TestGetAdminTokens()
        {
            return Ok(ms.TokenAdmin.GetAllTokenAdmins());
        }

        [HttpPost("DeleteEmployeeTokens")]
        public async Task<IActionResult> DeleteEmployeeTokens()
        {
            (await ms.TokenAdmin.GetAllTokenAdmins()).ForEach(async x => await ms.TokenAdmin.DeleteTokenAdminById(x.Id));
            if (!(await ms.TokenAdmin.GetAllTokenAdmins()).Any())
            {
                return Ok(new { messsage = "Токены удалены" });
            }
            return BadRequest(new { messsage = "Ошибка при удалении токенов" });
        }
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
         *  Subdivision
         */
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

        [HttpPost("AddSubdivision")]
        public async Task<IActionResult> AddSubdivision([FromHeader] string Authorization, [FromBody]SubdivisionModel? sub)
        {
            if (!Authorization.IsNullOrEmpty() && sub != null && !string.IsNullOrEmpty(sub.Name))
            {
                TokenAdmin? token = await ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (await ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    if ((await ms.Subdivision.GetAllSubdivisions()).Find(x => x.Name.Equals(sub.Name)) != null)
                    {
                        return BadRequest(new { message = "Ошибка. Такой отдел уже есть" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddSubdivision :name={sub.Name}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddSubdivision",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Имя Отдела={sub.Name}"
                        });
                        await ms.Subdivision.SaveSubdivision(sub);
                        return Ok(new { message = "Отдел добавлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("UpdateSubdivision")]
        public async Task<IActionResult> UpdateSubdivision([FromHeader] string Authorization, [FromBody]AddSubdivisionModel? sub)
        {
            if (!Authorization.IsNullOrEmpty() && sub != null && !string.IsNullOrEmpty(sub.Name))
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
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateSubdivision :id={sub.Id}, name={sub.Name}");
                        await ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateSubdivision",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id отдела={sub.Id}, Имя Отдела={sub.Name}"
                        });
                        await ms.Subdivision.SaveSubdivision(sub);
                        return Ok(new { message = "Отдел обновлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("DeleteSubdivision")]
        public async Task<IActionResult> DeleteSubdivision([FromHeader] string Authorization, [FromBody]IntIdModel? id)
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
                        if (ms.Subdivision.GetSubdivisionById(id.Id.Value) != null)
                        {
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/DeleteSubdivision",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id отдела={id.Id}"
                            });
                            ms.Subdivision.DeleteSubdivisionById(id.Id.Value);
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
                        return Ok(ms.Employee.GetAllEmployeeModels());
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

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
                        if (ms.Employee.GetEmployeeById(id.Id) != null)
                        {
                            EmployeeModel? model = await ms.Employee.GetEmployeeModelById(id.Id);
                            return Ok(model);
                        }
                        return NotFound(new { message = "Сотрудник не найден" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });            
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromHeader] string Authorization, [FromBody] AddEmployeeModel? employee)
        {
            if (!Authorization.IsNullOrEmpty() && 
                employee != null && !string.IsNullOrEmpty(employee.FirstName) && 
                !string.IsNullOrEmpty(employee.SecondName) && !string.IsNullOrEmpty(employee.LastName) &&
                !string.IsNullOrEmpty(employee.Login) && !string.IsNullOrEmpty(employee.Password) &&
                employee.IdSubdivision.HasValue && employee.IdSubdivision != 0 && 
                ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value) != null)
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
                        ms.Employee.SaveEmployee(employee);
                        return Ok(new { message = "Сотрудник добавлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromHeader] string Authorization, [FromBody] EmployeeDto? employee)
        {
            if (!Authorization.IsNullOrEmpty() && employee != null && !string.IsNullOrEmpty(employee.Id) && 
                !string.IsNullOrEmpty(employee.FirstName) && !string.IsNullOrEmpty(employee.SecondName) && 
                !string.IsNullOrEmpty(employee.LastName) && !string.IsNullOrEmpty(employee.Login) && 
                !string.IsNullOrEmpty(employee.Password) && employee.IdSubdivision.HasValue &&
                ms.Subdivision.GetSubdivisionById(employee.IdSubdivision.Value) != null)
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
                                UrlPath = "admin-api/AddEmployee",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id сотрудника={employee.Id}, Имя сотрудника ={employee.FirstName}, фамилия={employee.SecondName}, отчество={employee.LastName}"
                            });
                            await ms.Employee.SaveEmployee(employee);
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
                        if (await ms.Employee.GetEmployeeById(id.Id) != null)
                        {
                            await ms.Employee.DeleteEmployeeById(id.Id);
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
         *  Admin
         */
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

        [HttpPost("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin([FromHeader] string Authorization, [FromBody] AdminDto? admin)
        {
            if (!Authorization.IsNullOrEmpty() && admin != null && !string.IsNullOrEmpty(admin.Id) &&
                !string.IsNullOrEmpty(admin.FirstName) && !string.IsNullOrEmpty(admin.SecondName) && !string.IsNullOrEmpty(admin.LastName) &&
                !string.IsNullOrEmpty(admin.Login) && !string.IsNullOrEmpty(admin.Password) && admin.IdSubdivision.HasValue && admin.IdSubdivision > 0 )
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
                            await ms.Admin.SaveAdmin(admin);
                            return Ok(new { message = "Администратор добавлен" });
                        }else
                        {
                            return NotFound(new { message = "Ошибка. Такого пользователя не существует" });
                        }
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin([FromHeader] string Authorization, [FromBody] AddAdminModel? admin)
        {
            if (!Authorization.IsNullOrEmpty() && admin != null && 
                !string.IsNullOrEmpty(admin.FirstName) && !string.IsNullOrEmpty(admin.SecondName) && !string.IsNullOrEmpty(admin.LastName) &&
                !string.IsNullOrEmpty(admin.Login) && !string.IsNullOrEmpty(admin.Password) && admin.IdSubdivision.HasValue && admin.IdSubdivision > 0)
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
                        await ms.Admin.SaveAdmin(admin);
                        return Ok(new { message = "Администратор добавлен" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });

        }

        [HttpPost("DeleteAdmins")]
        public async Task<IActionResult> DeleteAdmins([FromHeader] string Authorization, [FromBody] StringIdModel? id)
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

        [HttpPost("DeleteCompetence")]
        public async Task<IActionResult> DeleteCompetence([FromHeader] string Authorization, [FromBody] IntIdModel? id)
        {
            if (!Authorization.IsNullOrEmpty() && id != null )
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
         *  Test
         */
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

                            TestModel testDto = new TestModel
                            {
                                Id = test.Id,
                                Name = test.Name,
                                Weight = test.Weight,
                                Instruction = test.Instruction,
                                Description = test.Description,
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
                                    ImagePath = quest.ImagePath,
                                    Number = Convert.ToInt32(quest.Number),
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
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

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
                        List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);
                            //.Where(x => x.IdTest.Equals(id)).ToList();

                        //!var Renderer = new IronPdf.ChromePdfRenderer();
                        var doc = new PdfDocument();

                        foreach (var quest in questions)
                        {
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
                                    html += "<p>Вопрос:" + quest.Text + "\r\n<p>Тип вопроса:" + (await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name + "</p>";
                                    html += $"<p><img style=\"width:300px; height:auto;\" src='data:image/jpg;base64,{base64}'/></p>";
                                }
                            }
                            else
                            {
                                html += "<p>Вопрос:" + quest.Text + "</p><p>Тип вопроса:" + (await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name + "</p>";
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
                                    html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\"></span>{answer.Text}</span>";
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
                                string[,] array = new string[2, list.Count];
                                int position = 0;
                                foreach (FirstSecondPartDto dto in list)
                                {
                                    array[0, position] = dto.FirstPartText;
                                    array[1, position] = dto.SecondPartText;
                                    position++;
                                }
                                Random rnd = new Random();
                                //array = array.OrderBy(x => rnd.Next()).ToArray();

                                for (int i = 0; i < array.GetLength(0); i++)
                                {
                                    html += "<p style=\"white-space: pre;\">" + array[0, i] + "                          " + array[1, i] + "</p>";
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
                        List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);

                        var doc = new PdfDocument();

                        foreach (var quest in questions)
                        {
                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                {
                                    byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                    string base64 = Convert.ToBase64String(array);
                                    html += "<p>Вопрос:" + quest.Text + "\r\n<p>Тип вопроса:" + (await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name + "</p>";
                                    html += $"<p><img style=\"width:auto; height:150px;\" src='data:image/jpg;base64,{base64}'/></p>";
                                }
                            }
                            else
                            {
                                html += "<p>Вопрос:" + quest.Text + "</p>\r\n<p>Тип вопроса:" +(await ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value)).Name + "</p>";
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
                                        html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\">X</span>{answer.Text}</span>";
                                    }
                                    else
                                    {
                                        html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\"></span>{answer.Text}</span>";
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
                                string[,] array = new string[2, list.Count];
                                int position = 0;
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
                        // todo await
                        par2.Append($"Категория теста: { (await ms.TestType.GetCompetenceById(test.IdCompetence.Value)).Name}").Font("Times New Roman").FontSize(14).Color(Color.Black).Bold();
                        Paragraph par3 = doc.InsertParagraph();
                        par3.Append($"Кол-во баллов: {test.Weight}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                        Paragraph par4 = doc.InsertParagraph();
                        par4.Append($"Описание: {test.Description}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                        Paragraph par5 = doc.InsertParagraph();
                        par5.Append($"инструкция: {test.Instruction}").Font("Times New Roman").FontSize(14).Color(Color.Black);

                        List<Question> questions = await ms.Question.GetQuestionsByTest(id.Id);

                        foreach (var quest in questions)
                        {
                            Paragraph par6 = doc.InsertParagraph();
                            par6.Append($"{quest.Number}. {quest.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                            Paragraph par7 = doc.InsertParagraph();
                            //todo await
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
                                    par8.Append($"⬜{answer.Number}. {answer.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
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
                                    par8.Append($"⬜{sub.Text}").Font("Times New Roman").FontSize(14).Color(Color.Black);
                                }
                            }
                            if ((await ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id)).Count != 0)
                            {
                                List<FirstSecondPartDto> list = await ms.GetFirstSecondPartDtoByQuestion(quest.Id);
                                string[,] array = new string[2, list.Count];
                                int position = 0;
                                foreach (FirstSecondPartDto dto in list)
                                {
                                    array[0, position] = dto.FirstPartText;
                                    array[1, position] = dto.SecondPartText;
                                    position++;
                                }
                                Random rnd = new Random();
                                //array = array.OrderBy(x => rnd.Next()).ToArray();

                                for (int i = 0; i < array.GetLength(0); i++)
                                {
                                    Paragraph par8 = doc.InsertParagraph();
                                    par8.Append($"{array[0, i]}     {array[1, i]}").Font("Times New Roman").FontSize(14).Color(Color.Black);
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

        [HttpPost("AddTest")]
        public async Task<IActionResult> AddTest([FromHeader] string Authorization, [FromForm] AddPostTestModel? postModel)
        {
            if (postModel != null && !postModel.Test.IsNullOrEmpty())
            {
                AddTestModel? test = JsonConvert.DeserializeObject<AddTestModel>(postModel.Test);
                if (!Authorization.IsNullOrEmpty() && test != null && !test.Name.IsNullOrEmpty() && test.Weight.HasValue &&
                    test.CompetenceId.HasValue && test.Questions!=null && test.Questions.Count != 0 &&
                    test.CompetenceId != 0 && await ms.TestType.GetCompetenceById(test.CompetenceId.Value) != null)
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
                                Params= $"Название теста={test.Name}, Id компетенции={test.CompetenceId}"
                            });
                            string idTest = Guid.NewGuid().ToString();
                            await ms.Test.SaveTest(new Test
                            {
                                Id = idTest,
                                Name = test.Name,
                                IdCompetence = test.CompetenceId,
                                Weight = test.Weight,
                                Description = test.Description,
                                Instruction = test.Instruction
                            });
                            int countOfQuestions = 1;
                            foreach (AddQuestionModel quest in test.Questions)
                            {
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
                                        IdTest = idTest
                                    });
                                }
                                countOfQuestions++;
                                int countOfAnswer = 0;
                                foreach (JObject answer in quest.Answers)
                                {
                                    countOfAnswer++;
                                    /*AnswerDto answerDto = answer.Deserialize<AnswerDto>();
                                    SubsequenceDto subsequenceDto = answer.Deserialize<SubsequenceDto>();
                                    FirstSecondPartDto firstSecondPartDto = answer.Deserialize<FirstSecondPartDto>();*/

                                    AnswerDto answerDto = answer.ToObject<AnswerDto>();
                                    SubsequenceDto subsequenceDto = answer.ToObject<SubsequenceDto>();
                                    FirstSecondPartDto firstSecondPartDto = answer.ToObject<FirstSecondPartDto>();

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
                            return Ok(new { message = "Добавление теста успешно" });
                        }
                    }
                    return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
                }
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("UpdateTest")]
        public async Task<IActionResult> UpdateTest([FromHeader] string Authorization, [FromForm] UpdatePostTestModel? updatePostModel)
        {
            if (updatePostModel != null && !updatePostModel.Test.IsNullOrEmpty())
            {
                UpdateTestModel? test = JsonConvert.DeserializeObject<UpdateTestModel>(updatePostModel.Test);
                if (!Authorization.IsNullOrEmpty() && test != null && !test.Name.IsNullOrEmpty() && test.Weight.HasValue &&
                    test.CompetenceId.HasValue && test.Questions != null && test.Questions.Count != 0 &&
                    test.CompetenceId != 0 && ms.TestType.GetCompetenceById(test.CompetenceId.Value) != null)
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
                                Params = $"Id Теста={test.Id}, Название Теста={test.Name}, Кол-во вопросов={test.Questions.Count}"
                            });

                            if (await ms.Test.GetTestById(test.Id) == null)
                                return BadRequest(new { message = "Ошибка. Такого теста не существует" });

                            await ms.Test.SaveTest(new Test
                            {
                                Id = test.Id,
                                Name = test.Name,
                                Weight = test.Weight,
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
                                        string saveImage = Path.Combine(environment.WebRootPath+"/images/", file.FileName);
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
                                        Number = Convert.ToByte(quest.Number)
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
                                        Number = Convert.ToByte(quest.Number)
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
                                                !System.IO.File.Exists(Path.Combine(environment.WebRootPath+"/images/", answerDto.ImagePath)))
                                            {
                                                IFormFile file = updatePostModel.Files.First(f => f.FileName.Equals(answerDto.ImagePath));
                                                string saveImage = Path.Combine(environment.WebRootPath+"/images/", file.FileName);
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

        [HttpPost("GetPurposesByEmployeeId")]
        public async Task<IActionResult> GetPurposesByEmployeeId([FromHeader]string Authorization, [FromBody] StringIdModel id)
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
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("AddPurpose")]
        public async Task<IActionResult> AddPurpose([FromHeader] string Authorization, [FromBody] AddTestPurposeModel? purpose)
        {
            if (!Authorization.IsNullOrEmpty() && purpose != null && !purpose.IdTest.IsNullOrEmpty() &&
                !purpose.IdEmployee.IsNullOrEmpty() && !purpose.DatatimePurpose.IsNullOrEmpty())
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
                                return BadRequest(new { message = "Ошибка. Пользователя с эти Id нет" });

                        if (await ms.Employee.GetEmployeeById(purpose.IdEmployee) == null)
                            return BadRequest(new { message = "Ошибка. Теста с этим Id нет" });

                        if (await ms.TestPurpose.GetTestPurposeByEmployeeTestId(purpose.IdTest,purpose.IdEmployee) != null)
                                return BadRequest(new { message = "Ошибка. Этот тест уже назначен этому пользованелю" });

                        logger.LogInformation($"/admin-api/AddPurpose employeeId={purpose.IdEmployee}, testId={purpose.IdTest}, datatime={purpose.DatatimePurpose}");
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

        [HttpPost("AddPurposesBySubdivision")]
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

        [HttpPost("UpdatePurpose")]
        public async Task<IActionResult> UpdatePurpose([FromHeader] string Authorization, [FromBody] UpdateTestPurposeModel? purpose)
        {
            if (!Authorization.IsNullOrEmpty() && purpose != null && purpose.Id.HasValue &&
                !purpose.IdTest.IsNullOrEmpty() && !purpose.DatatimePurpose.IsNullOrEmpty())
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
                            logger.LogInformation($"/admin-api/UpdatePurpose purposeId={purpose.Id}");
                            await ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/UpdatePurpose",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id Сотрудника={purpose.IdEmployee}, Id теста={purpose.IdTest}, Дата сдачи={purpose.DatatimePurpose}"
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
        [HttpPost("GetResults")]
        public async Task<IActionResult> GetResults([FromHeader] string Authorization, [FromBody] ResultQuerryModel query)
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

                        List<EmployeeResultModel> list = await ms.GetAllEmployeeResultModels();
                        if (query.IdSubdivision.HasValue && query.IdSubdivision != 0)
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
                        }
                        return Ok(list);
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

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
                            UrlPath = "user-api/DeletePurpose",
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
                            await ms.Result.DeleteResultById(result.IdResult);
                            await ms.EmployeeResult.DeleteEmployeeResultById(result.Id.Value);
                        }
                        return Ok(new { message = "Результаты удалены" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

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
    }
}
