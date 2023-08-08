using DataBase.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;
using Personal_Testing_System.Services;
using System.Net;
using TheArtOfDev.HtmlRenderer.PdfSharp;

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
                               IWebHostEnvironment _environmentironment)
        {
            logger = _logger;
            ms = _masterService;
            environment = _environmentironment;
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
         *  TestPDF
         */
        [HttpGet("GETPDF")]
        public async Task<IActionResult> GETPDF()
        {
            if (!System.IO.File.Exists(Path.Combine(environment.WebRootPath) + "\\images\\logo.jpg"))
            {
                return BadRequest("файл не найден");
            }

            var doc = new PdfDocument();
            string html = "";
            string url = "htpps://" + Request.Host.Value + "/images/logo.jpg";
            html += $"1<img src='{url}'/>";
            byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/logo.jpg").PhysicalPath);
            string base64 = Convert.ToBase64String(array);
            html += $"2<img src='data:image/png;base64,{base64}'/>";
            PdfGenerator.AddPdfPages(doc, html, PdfSharpCore.PageSize.A4);
            byte[] res = null;
            using (MemoryStream s = new MemoryStream())
            {
                doc.Save(s);
                res = s.ToArray();
            }
            return File(res, "application/pdf", "ADMIN_GETPDF.pdf");
            //environment.WebRootFileProvider.GetFileInfo("images/logo.jpg").PhysicalPath





            /*PdfDocument pdf = renderer.RenderHtmlAsPdf($"<img src='{"https://img1.goodfon.ru/original/320x240/4/f3/les-gory-minimalizm.jpg"}' />"+
                                                       $"<img src='{"https://" + Request.Host.Value + "/images/trashcan.jpg\""}' />");
            var Renderer = new IronPdf.ChromePdfRenderer();
            var pngBinaryData = System.IO.File.ReadAllBytes(Path.Combine(environmentironment.WebRootPath) + "\\images\\trashcan.jpg");
            var ImgDataURI = @"data:image/png;base64," + Convert.ToBase64String(pngBinaryData);
            var ImgHtml = $"<img src='{ImgDataURI}'>";
            using var pdfdoc = Renderer.RenderHtmlAsPdf(ImgHtml);

            //using var PDF = IronPdf.ChromePdfRenderer.StaticRenderUrlAsPdf(new Uri("https://en.wikipedia.org"));
            return File(pdfdoc.BinaryData, "application/pdf", "TESTPDF.Pdf");*/

            /*string someUrl = "https://" + Request.Host.Value + "/images/trashcan.jpg";
            byte[] imageBytes1 = null;
            using (var webClient = new WebClient())
            {
                imageBytes1 = webClient.DownloadData(someUrl);
            }
            string htmlContent = "";
            htmlContent += "<style>\r\n img.logo \r\n{ \r\nwidth:110px;\r\nheight:110px;\r\ncontent: url('data:image/jpeg;base64," + Convert.ToBase64String(imageBytes) + "')\r\n} \r\n</style>\r\n";
            string data = @"data:image/jpg;base64," + Convert.ToBase64String(imageBytes1);
            htmlContent += $"<img  src='{data}' />";
            htmlContent += "<img class='logo' src='data:image/jpeg;base64," + Convert.ToBase64String(imageBytes1) + "'/>";
            string imageUrl = "https://" + Request.Host.Value + "/images/trashcan.jpg";
            string imageUrl1 = "https://img1.goodfon.ru/original/320x240/4/f3/les-gory-minimalizm.jpg";
            htmlContent += "<img src='"+ imageUrl +"'/>";
            htmlContent += "<img src=\""+ imageUrl1 +"\"/>";
            //htmlContent +=
            PdfDocument doc = new PdfDocument();
            PdfGenerator.AddPdfPages(doc, htmlContent, PageSize.A4);
            byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms);
                res = ms.ToArray();
            }
            return File(res, "application/pdf", "TEST.pdf");*/
        }
        /*
        * 
        */

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
            ms.TokenAdmin.GetAllTokenAdmins().ForEach(x => ms.TokenAdmin.DeleteTokenAdminById(x.Id));
            if (!ms.TokenAdmin.GetAllTokenAdmins().Any())
            {
                return Ok(new { messsage = "Токены удалены" });
            }
            return BadRequest(new { messsage = "Ошибка при удалении токенов" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel? loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new { message = "Одно из полей пустое" });
            }
            logger.LogInformation($"/admin-api/Login : login={loginModel.Login}, Password={loginModel.Password} ");
            ms.Log.SaveLog(new Log
            {
                UrlPath = "admin-api/Login",
                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                DataTime = DateTime.Now,
                Params = $"логин={loginModel.Login}, пароль={loginModel.Password}"
            });

            AdminDto? adminDto = ms.Admin.GetAllAdminDtos()
                                  .Find(x => x.Login.Equals(loginModel.Login));

            if (adminDto == null)
            {
                return BadRequest(new { message = "Администратор не найден" });
            }
            else
            {
                if (loginModel.Password.Equals(adminDto.Password))
                {
                    TokenAdmin? tokenAdmin = ms.TokenAdmin.GetTokenEmployeeByAdminId(adminDto.Id);
                    if (tokenAdmin != null && !ms.IsTokenAdminExpired(tokenAdmin))
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
                        ms.TokenAdmin.SaveTokenAdmin(new TokenAdmin
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/LogOut : AuthHeader={Authorization}");
                    ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/LogOut",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                        Params = $"Id администратора={token.IdAdmin}"
                    });
                    ms.TokenAdmin.DeleteTokenAdminById(token.Id);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    logger.LogInformation($"/admin-api/GetSubdivisions : AuthHeader={Authorization}");
                    ms.Log.SaveLog(new Log
                    {
                        UrlPath = "admin-api/GetSubdivisions",
                        UserId = token.IdAdmin,
                        UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DataTime = DateTime.Now,
                    });
                    return Ok(ms.Subdivision.GetAllSubdivisionDtos());
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    if (ms.Subdivision.GetAllSubdivisions().Find(x => x.Name.Equals(sub.Name)) != null)
                    {
                        return BadRequest(new { message = "Ошибка. Такой отдел уже есть" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddSubdivision :name={sub.Name}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddSubdivision",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Имя Отдела={sub.Name}"
                        });
                        ms.Subdivision.SaveSubdivision(sub);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    if (ms.Subdivision.GetSubdivisionById(sub.Id.Value) == null)
                    {
                        return BadRequest(new { message = "Ошибка. Такого отдела нет" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateSubdivision :id={sub.Id}, name={sub.Name}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateSubdivision",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id отдела={sub.Id}, Имя Отдела={sub.Name}"
                        });
                        ms.Subdivision.SaveSubdivision(sub);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if (ms.Subdivision.GetSubdivisionById(id.Id.Value) != null)
                        {
                            ms.Log.SaveLog(new Log
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetEmployees ");
                        ms.Log.SaveLog(new Log
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetEmployee :id={id.Id}");

                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetEmployee",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Сотрудника={id.Id}"
                        });
                        if (ms.Employee.GetEmployeeById(id.Id) != null)
                        {
                            EmployeeModel? model = ms.Employee.GetEmployeeModelById(id.Id);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddEmployee :fn={employee.FirstName}, sn={employee.SecondName}, " +
                                               $" ln={employee.LastName}, idSubdivision={employee.IdSubdivision}");
                        ms.Log.SaveLog(new Log
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if (ms.Employee.GetEmployeeById(employee.Id) != null)
                        {
                            ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/AddEmployee",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id сотрудника={employee.Id}, Имя сотрудника ={employee.FirstName}, фамилия={employee.SecondName}, отчество={employee.LastName}"
                            });
                            ms.Employee.SaveEmployee(employee);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteEmployee :id={id.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteEmployee",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Сотрудника={id.Id}"
                        });
                        if (ms.Employee.GetEmployeeById(id.Id) != null)
                        {
                            ms.Employee.DeleteEmployeeById(id.Id);
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
         *  Competence
         */
        [HttpGet("GetCompetences")]
        public async Task<IActionResult> GetCompetences([FromHeader] string Authorization)
        {
            if (!Authorization.IsNullOrEmpty())
            {
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetences");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetences",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        return Ok(ms.TestType.GetAllCompetenceDtos());
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetCompetence :Id={id.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetCompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции теста={id.Id}"
                        });
                        if (ms.TestType.GetCompetenceById(id.Id.Value) != null)
                        {
                            CompetenceDto? competenceDto = ms.TestType.GetCompetenceDtoById(id.Id.Value);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddCompetence :Name={competence.Name}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddCompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Название компетенции теста={competence.Name}"
                        });
                        ms.TestType.SaveCompetence(competence);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/UpdateCompetence :Id={competence.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/UpdateCompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции теста={competence.Id}, Название={competence.Name}"
                        });
                        if (ms.TestType.GetCompetenceById(competence.Id.Value) != null)
                        {
                            ms.TestType.SaveCompetence(competence);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteCompetence :competenceId={id.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteCompetence",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id компетенции теста={id.Id}"
                        });
                        if (id.Id.HasValue && id.Id != 0 && ms.TestType.GetCompetenceById(id.Id.Value) != null)
                        {
                            ms.TestType.DeleteCompetenceById(id.Id.Value);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetTests ");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetTests",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        return Ok(ms.Test.GetAllTestGetModels());
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetTest :id={id.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetTest",
                            UserId = $"",
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Теста={id.Id}"
                        });
                        if (!string.IsNullOrEmpty(id.Id))
                        {
                            Test? test = ms.Test.GetTestById(id.Id);
                            if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });
                            List<Question> questions = ms.Question.GetAllQuestions()
                                .Where(x => x.IdTest.Equals(id.Id)).ToList();

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
                                    ImagePath = quest.ImagePath,
                                    Answers = new List<object>() { }
                                };
                                if (!quest.ImagePath.IsNullOrEmpty())
                                {
                                    if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                    {
                                        byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                        string base64 = Convert.ToBase64String(array);
                                        createQuestionDto.Base64Image = base64;
                                    }
                                }

                                if (ms.Answer.GetAnswerDtosByQuestionId(quest.Id).Count != 0)
                                {
                                    List<Answer> list = ms.Answer.GetAnswersByQuestionId(quest.Id);
                                    foreach (Answer answer in list)
                                    {
                                        AnswerModel model = new AnswerModel
                                        {
                                            IdAnswer = answer.Id,
                                            Text = answer.Text,
                                            IdQuestion = answer.IdQuestion,
                                            Correct = answer.Correct
                                        };
                                        if (!answer.ImagePath.IsNullOrEmpty())
                                        {
                                            if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                            {
                                                byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                                string base64 = Convert.ToBase64String(array);
                                                model.Base64Image = base64;
                                                model.ImagePath = answer.ImagePath;
                                            }
                                        }
                                        createQuestionDto.Answers.Add(model);
                                    }
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
                                        secondPartDtos.Add(ms.SecondPart.GetSecondPartDtoByFirstPartId(firstPart.IdFirstPart));
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPdfTest :id={id.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPdfTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id теста ={id.Id}"
                        });

                        Test? test = ms.Test.GetTestById(id.Id);
                        if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });
                        string html = "<h1 style=\"text-align: left;\">Название Теста: " + test.Name + "</h1> " +
                                      "<h2 style=\"text-align: left;\">Компетенция: " + ms.TestType.GetCompetenceById(test.IdCompetence.Value).Name + "</h2><hr>";
                        List<Question> questions = ms.Question.GetQuestionsByTest(id.Id);
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
                                    html += "<p>Вопрос:" + quest.Text + "\r\n<p>Тип вопроса:" + ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value).Name + "</p>";
                                    html += $"<p><img style=\"width:300px; height:auto;\" src='data:image/jpg;base64,{base64}'/></p>";
                                }
                            }
                            else
                            {
                                html += "<p>Вопрос:" + quest.Text + "</p><p>Тип вопроса:" + ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value).Name + "</p>";
                            }

                            if (ms.Answer.GetAnswerDtosByQuestionId(quest.Id).Count != 0)
                            {
                                foreach (AnswerDto answer in ms.Answer.GetAnswerDtosByQuestionId(quest.Id))
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
                            if (ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id).Count != 0)
                            {
                                foreach (SubsequenceDto sub in ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id))
                                {
                                    html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\"></span>{sub.Text}</span>";
                                }
                            }
                            if (ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id).Count != 0)
                            {
                                List<FirstSecondPartDto> list = ms.GetFirstSecondPartDtoByQuestion(quest.Id);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPdfCorerectTest :id={id.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPdfCorerectTest",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id теста ={id.Id}"
                        });

                        Test? test = ms.Test.GetTestById(id.Id);
                        if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });
                        string html = "";
                        html += "<div><h1 style=\"text-align: left;\">Название Теста: " + test.Name + "</h1>\r\n " +
                                      "<h2 style=\"text-align: left;\">Компетенция: " + ms.TestType.GetCompetenceById(test.IdCompetence.Value).Name + "</h2>\r\n<hr>";
                        List<Question> questions = ms.Question.GetQuestionsByTest(id.Id);

                        var doc = new PdfDocument();

                        foreach (var quest in questions)
                        {
                            if (!quest.ImagePath.IsNullOrEmpty())
                            {
                                if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                {
                                    byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                    string base64 = Convert.ToBase64String(array);
                                    html += "<p>Вопрос:" + quest.Text + "\r\n<p>Тип вопроса:" + ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value).Name + "</p>";
                                    html += $"<img style=\"width:auto; height:150px;\" src='data:image/jpg;base64,{base64}'/>";
                                }
                            }
                            else
                            {
                                html += "<p>Вопрос:" + quest.Text + "</p>\r\n<p>Тип вопроса:" + ms.QuestionType.GetQuestionTypeById(quest.IdQuestionType.Value).Name + "</p>";
                            }
                            if (ms.Answer.GetAnswerDtosByQuestionId(quest.Id).Count != 0)
                            {
                                foreach (AnswerDto answer in ms.Answer.GetAnswerDtosByQuestionId(quest.Id))
                                {
                                    if (!answer.ImagePath.IsNullOrEmpty())
                                    {
                                        if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath))
                                        {
                                            byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath);
                                            string base64 = Convert.ToBase64String(array);
                                            html += $"<img style=\"width:auto; height:150px;\" src='data:image/png;base64,{base64}'/>";
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
                            if (ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id).Count != 0)
                            {
                                foreach (SubsequenceDto sub in ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id))
                                {
                                    html += $"<span style=\"display: block; margin: 0px 0px 0px 0px;\"><span style=\" content: ''; display: inline-block; width: 15px; height: 15px; margin-right: 5px; border: 1px solid black;\">{sub.Number}</span>{sub.Text}</span>";
                                    //html += "<p> "+sub.Number + " " + sub.Text + "</p>";
                                }
                            }
                            if (ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id).Count != 0)
                            {
                                List<FirstSecondPartDto> list = ms.GetFirstSecondPartDtoByQuestion(quest.Id);
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

        [HttpPost("AddTest")]
        public async Task<IActionResult> AddTest([FromHeader] string Authorization, [FromForm] AddPostTestModel? postModel)
        {
            if (postModel != null && !postModel.Test.IsNullOrEmpty())
            {
                AddTestModel? test = JsonConvert.DeserializeObject<AddTestModel>(postModel.Test);
                if (!Authorization.IsNullOrEmpty() && test != null && !test.Name.IsNullOrEmpty() &&
                    test.CompetenceId.HasValue && test.Questions!=null && test.Questions.Count != 0 &&
                    test.CompetenceId != 0 && ms.TestType.GetCompetenceById(test.CompetenceId.Value) != null)
                {
                    TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                    if (token != null)
                    {
                        if (ms.IsTokenAdminExpired(token))
                        {
                            return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                        }
                        else
                        {
                            logger.LogInformation($"/admin-api/AddTest test: name={test.Name}, idCompetence={test.CompetenceId}," +
                                                  $"countOfQuestions={test.Questions.Count}");
                            ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/AddTest",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params= $"Название теста={test.Name}, Id компетенции={test.CompetenceId}"
                            });
                            string idTest = Guid.NewGuid().ToString();
                            ms.Test.SaveTest(new Test
                            {
                                Id = idTest,
                                Name = test.Name,
                                IdCompetence = test.CompetenceId,
                                Weight = test.Questions.Count
                            });

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
                                            ms.Question.SaveQuestion(new Question
                                            {
                                                Id = idQuestion,
                                                Text = quest.Text,
                                                IdQuestionType = quest.IdQuestionType,
                                                ImagePath = quest.ImagePath,
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
                                            ms.Question.SaveQuestion(new Question
                                            {
                                                Id = idQuestion,
                                                Text = quest.Text,
                                                IdQuestionType = quest.IdQuestionType,
                                                ImagePath = imagePath,
                                                IdTest = idTest
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    ms.Question.SaveQuestion(new Question
                                    {
                                        Id = idQuestion,
                                        Text = quest.Text,
                                        IdQuestionType = quest.IdQuestionType,
                                        ImagePath = quest.ImagePath,
                                        IdTest = idTest
                                    });
                                }
                                foreach (JObject answer in quest.Answers)
                                {
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

                                                    ms.Answer.SaveAnswer(new Answer
                                                    {
                                                        Text = answerDto.Text,
                                                        IdQuestion = idQuestion,
                                                        Correct = answerDto.Correct,
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

                                                    ms.Answer.SaveAnswer(new Answer
                                                    {
                                                        Text = answerDto.Text,
                                                        IdQuestion = idQuestion,
                                                        Correct = answerDto.Correct,
                                                        ImagePath = imagePath
                                                    });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ms.Answer.SaveAnswer(new Answer
                                            {
                                                Text = answerDto.Text,
                                                IdQuestion = idQuestion,
                                                Correct = answerDto.Correct,
                                                ImagePath = answerDto.ImagePath
                                            });
                                        }
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
                if (!Authorization.IsNullOrEmpty() && test != null && !test.Name.IsNullOrEmpty() &&
                    test.CompetenceId.HasValue && test.Questions != null && test.Questions.Count != 0 &&
                    test.CompetenceId != 0 && ms.TestType.GetCompetenceById(test.CompetenceId.Value) != null)
                {
                    TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                    if (token != null)
                    {
                        if (ms.IsTokenAdminExpired(token))
                        {
                            return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                        }
                        else
                        {
                            logger.LogInformation($"/admin-api/UpdateTest test: id={test.Id} name={test.Name}, idCompetence={test.CompetenceId}," +
                                      $"countOfQuestions={test.Questions.Count}");
                            ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/UpdateTest",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id Теста={test.Id}, Название Теста={test.Name}, Кол-во вопросов={test.Questions.Count}"
                            });

                            if (ms.Test.GetTestById(test.Id) == null)
                                return BadRequest(new { message = "Ошибка. Такого теста не существует" });

                            ms.Test.SaveTest(new Test
                            {
                                Id = test.Id,
                                Name = test.Name,
                                IdCompetence = test.CompetenceId,
                                Weight = test.Questions.Count
                            });

                            List<QuestionDto> currentQuestions = ms.Question.GetQuestionDtosByTest(test.Id);
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
                                    ms.Question.SaveQuestion(new Question
                                    {
                                        Id = quest.Id,
                                        Text = quest.Text,
                                        IdQuestionType = quest.IdQuestionType,
                                        ImagePath = quest.ImagePath,
                                        IdTest = test.Id
                                    });
                                }
                                else
                                {
                                    QuestionDto? questToDelete = currentQuestions.Find(x => x.Id.Equals(quest.Id));
                                    if (questToDelete != null)
                                    {
                                        currentQuestions.Remove(questToDelete);
                                    }
                                    ms.Question.SaveQuestion(new Question
                                    {
                                        Id = quest.Id,
                                        Text = quest.Text,
                                        IdQuestionType = quest.IdQuestionType,
                                        ImagePath = quest.ImagePath,
                                        IdTest = test.Id
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
                                                !System.IO.File.Exists(Path.Combine(environment.WebRootPath, answerDto.ImagePath)))
                                            {
                                                IFormFile file = updatePostModel.Files.GetFile(answerDto.ImagePath);
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
                                        else
                                        {
                                            ms.Answer.SaveAnswer(new Answer
                                            {
                                                Id = answerDto.IdAnswer.Value,
                                                Text = answerDto.Text,
                                                IdQuestion = quest.Id,
                                                Correct = answerDto.Correct,
                                                ImagePath = answerDto.ImagePath
                                            });
                                        }
                                    }
                                    if (subsequenceDto is SubsequenceDto && subsequenceDto.Number != null && subsequenceDto.Number != 0)
                                    {
                                        logger.LogInformation($"subsequenceDto -> text={subsequenceDto.Text}, number={subsequenceDto.Number}");
                                        ms.Subsequence.SaveSubsequence(new Subsequence
                                        {
                                            Id = subsequenceDto.IdSubsequence,
                                            Text = subsequenceDto.Text,
                                            Number = subsequenceDto.Number,
                                            IdQuestion = subsequenceDto.IdQuestion
                                        });
                                    }
                                    if (firstPartDto is FirstPartDto && !string.IsNullOrEmpty(firstPartDto.IdFirstPart) &&
                                        !string.IsNullOrEmpty(firstPartDto.Text))
                                    {
                                        logger.LogInformation($"firstPartDto -> Id={firstPartDto.IdFirstPart}, Text={firstPartDto.Text}");
                                        ms.FirstPart.SaveFirstPart(new FirstPart
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
                                        ms.SecondPart.SaveSecondPart(new SecondPart
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
                                        ms.FirstPart.SaveFirstPart(new FirstPart
                                        {
                                            Id = firstPartId,
                                            Text = firstSecondPartDto.FirstPartText,
                                            IdQuestion = quest.Id
                                        });
                                        ms.SecondPart.SaveSecondPart(new SecondPart
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
                                    List<Answer> answers = ms.Answer.GetAnswersByQuestionId(questionToDelete.Id);
                                    foreach (Answer answer in answers)
                                    {
                                        string answerFilePath = environment.WebRootFileProvider.GetFileInfo("images/" + answer.ImagePath).PhysicalPath;
                                        if (!answer.ImagePath.IsNullOrEmpty() && System.IO.File.Exists(answerFilePath))
                                        {
                                            System.IO.File.Delete(answerFilePath);
                                        }
                                        ms.Answer.DeleteAnswerById(answer.Id);
                                    }

                                    ms.Subsequence.DeleteSubsequencesByQuestion(questionToDelete.Id);
                                    ms.DeleteFirstAndSecondPartsByQuestion(questionToDelete.Id);

                                    ms.Question.DeleteQuestionById(questionToDelete.Id);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteTest testId={id.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteTest",
                            UserId = $"",
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Теста={id.Id}"
                        });
                        if (ms.Test.GetTestById(id.Id) != null)
                        {
                            ms.DeleteTestById(id.Id, environment);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPurposess ");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPurposess",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        return Ok(ms.TestPurpose.GetAllPurposeAdminModels());
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetPurposessByEmployeeId id={id.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetPurposesByEmployeeId",
                            UserId = $"",
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Сотрудника={id.Id}"
                        });

                        List<TestPurposeDto> purposes = ms.TestPurpose.GetAllTestPurposeDtos()
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
                                    Test = ms.Test.GetTestGetModelById(purpose.IdTest),
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if (ms.Test.GetTestById(purpose.IdTest) == null)
                                return BadRequest(new { message = "Ошибка. Пользователя с эти Id нет" });

                        if (ms.Employee.GetEmployeeById(purpose.IdEmployee) == null)
                            return BadRequest(new { message = "Ошибка. Теста с этим Id нет" });

                        if (ms.TestPurpose.GetTestPurposeByEmployeeTestId(purpose.IdTest,purpose.IdEmployee) != null)
                                return BadRequest(new { message = "Ошибка. Этот тест уже назначен этому пользованелю" });

                        logger.LogInformation($"/admin-api/AddPurpose employeeId={purpose.IdEmployee}, testId={purpose.IdTest}, datatime={purpose.DatatimePurpose}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddPurpose",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Сотрудника={purpose.IdEmployee}, Id теста={purpose.IdTest}"
                        });
                        ms.TestPurpose.SaveTestPurpose(purpose);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/AddPurposesBySubdivision testId={testId}, idSubdivision={idSubdivision}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/AddPurpose",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Отдела={idSubdivision}, Id теста={testId}, Дата сдачи={time}"
                        });
                        ms.TestPurpose.SavePurposeBySubdivisionId(testId, idSubdivision.Value, DateTime.Parse(time));
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        if (ms.TestPurpose.GetTestPurposeById(purpose.Id.Value) != null)
                        {
                            logger.LogInformation($"/admin-api/UpdatePurpose purposeId={purpose.Id}");
                            ms.Log.SaveLog(new Log
                            {
                                UrlPath = "admin-api/UpdatePurpose",
                                UserId = token.IdAdmin,
                                UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                                DataTime = DateTime.Now,
                                Params = $"Id Сотрудника={purpose.IdEmployee}, Id теста={purpose.IdTest}, Дата сдачи={purpose.DatatimePurpose}"
                            });
                            ms.TestPurpose.SaveTestPurpose(purpose);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeletePurpose purposeId={id.Id}");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeletePurpose",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                            Params = $"Id Назначения={id.Id}"
                        });
                        if (ms.TestPurpose.GetTestPurposeById(id.Id.Value) != null)
                        {
                            ms.TestPurpose.DeleteTestPurposeById(id.Id.Value);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/GetResults ");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/GetResults",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });

                        List<EmployeeResultModel> list = ms.GetAllEmployeeResultModels();
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteResults");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "user-api/DeletePurpose",
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now,
                        });
                        List<EmployeeResultDto> employeeResults = ms.EmployeeResult.GetAllEmployeeResultDtos();
                        foreach (EmployeeResultDto result in employeeResults)
                        {
                            ms.EmployeeAnswer.DeleteEmployeeAnswersByResult(result.IdResult);
                            ms.EmployeeMatching.DeleteEmployeeMatchingByResult(result.IdResult);
                            ms.EmployeeSubsequence.DeleteEmployeeSubsequenceByResult(result.IdResult);
                            ms.Result.DeleteResultById(result.IdResult);
                            ms.EmployeeResult.DeleteEmployeeResultById(result.Id.Value);
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
                TokenAdmin? token = ms.TokenAdmin.GetTokenAdminByToken(Authorization);
                if (token != null)
                {
                    if (ms.IsTokenAdminExpired(token))
                    {
                        return BadRequest(new { message = "Время сессии истекло. Авторизуйтесь для работы в системе" });
                    }
                    else
                    {
                        logger.LogInformation($"/admin-api/DeleteResult ");
                        ms.Log.SaveLog(new Log
                        {
                            UrlPath = "admin-api/DeleteResult",
                            UserId = token.IdAdmin,
                            UserIp = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            DataTime = DateTime.Now
                        });
                        EmployeeResult? employeeResults = ms.EmployeeResult.GetEmployeeResultById(resultId.Id.Value);
                        if (employeeResults != null)
                        {
                            ms.EmployeeAnswer.DeleteEmployeeAnswersByResult(employeeResults.IdResult);
                            ms.EmployeeMatching.DeleteEmployeeMatchingByResult(employeeResults.IdResult);
                            ms.EmployeeSubsequence.DeleteEmployeeSubsequenceByResult(employeeResults.IdResult);
                            ms.Result.DeleteResultById(employeeResults.IdResult);
                            ms.EmployeeResult.DeleteEmployeeResultById(resultId.Id.Value);
                            return Ok(new { message = "Результаты удалены" });
                        }
                        return BadRequest(new { message = "Ошибка. Такого результата нет" });
                    }
                }
                return BadRequest(new { message = "Ошибка. Вы не авторизованы в системе" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }
    }
}
