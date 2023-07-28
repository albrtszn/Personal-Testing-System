using DataBase.Repository.Models;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;
using Personal_Testing_System.Services;
using System;
using System.Buffers.Text;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using static System.Net.WebRequestMethods;

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
            if (!Directory.Exists(environment.WebRootPath+"/images"))
            {
                Directory.CreateDirectory(environment.WebRootPath + "/images");
            }
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
            PdfGenerator.AddPdfPages(doc,html,PdfSharpCore.PageSize.A4);
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel? loginModel)
        {
            logger.LogInformation($"/admin-api/Login :login={loginModel.Login}, password={loginModel.Password}");

            if (loginModel == null) return BadRequest(new { message = "Поля не заполнены" });
            if (string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Password))
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
        /*
         *  Subdivision
         */
        [HttpGet("GetSubdivisions")]
        public async Task<IActionResult> GetSubdivisions()
        {
            logger.LogInformation($"/admin-api/GetSubdivisions");

            return Ok(ms.Subdivision.GetAllSubdivisionDtos());
        }

        [HttpPost("AddSubdivision")]
        public async Task<IActionResult> AddSubdivision(SubdivisionModel? sub)
        {
            logger.LogInformation($"/admin-api/AddSubdivision :name={sub.Name}");

            if (sub != null && !string.IsNullOrEmpty(sub.Name))
            {
                if (ms.Subdivision.GetAllSubdivisions().Find(x => x.Name.Equals(sub.Name)) != null)
                {
                    return BadRequest(new { message = "Ошибка. Такой отдел уже есть" });
                }
                else
                {
                    ms.Subdivision.SaveSubdivisionDto(sub);
                    return Ok(new { message = "Отдел добавлен" });
                }
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("UpdateSubdivision")]
        public async Task<IActionResult> UpdateSubdivision(SubdivisionDto? sub)
        {
            logger.LogInformation($"/admin-api/UpdateSubdivision :id={sub.Id}, name={sub.Name}");

            if (sub != null && !sub.Name.IsNullOrEmpty())
            {
                if (ms.Subdivision.GetSubdivisionById(sub.Id.Value) == null)
                {
                    return BadRequest(new { message = "Ошибка. Такого отдела нет" });
                }
                else
                {
                    ms.Subdivision.SaveSubdivisionDto(sub);
                    return Ok(new { message = "Отдел обновлен" });
                }
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("DeleteSubdivision")]
        public async Task<IActionResult> DeleteSubdivision(int? id)
        {
            if (!id.HasValue) return BadRequest(new { message = "Поле не заполнено" });
            if (ms.Subdivision.GetSubdivisionById(id.Value) != null)
            {
                ms.Subdivision.DeleteSubdivisionById(id.Value);
                return Ok(new { message = "Отдел удален" });
            }
            return BadRequest(new { message = "Ошибка. Такого отдела не существует" });
        }
        /*
         *  Employee
         */
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
            if (id.IsNullOrEmpty()) return BadRequest(new { message = "Ошибка. Поле не заполнено" });
            EmployeeModel? model = ms.Employee.GetEmployeeModelById(id);
            if (model != null)
            {
                return Ok(model);
            }
            return NotFound(new { message = "Сотрудник не найден" });
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee(AddEmployeeModel? employee)
        {
            logger.LogInformation($"/admin-api/AddEmployee :fn={employee.FirstName}, sn={employee.SecondName}, " +
                                  $" ln={employee.LastName}, idSubdivision={employee.IdSubdivision}");
            if (employee != null && !string.IsNullOrEmpty(employee.FirstName) && !
                string.IsNullOrEmpty(employee.SecondName) && !string.IsNullOrEmpty(employee.LastName) &&
                !string.IsNullOrEmpty(employee.Login) && !string.IsNullOrEmpty(employee.Password) &&
                employee.IdSubdivision.HasValue)
            {
                ms.Employee.SaveEmployee(employee);
                return Ok(new { message = "Сотрудник добавлен" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(EmployeeDto? employee)
        {
            logger.LogInformation($"/admin-api/UpdateEmployee :id={employee.Id}, fn={employee.FirstName}, sn={employee.SecondName}, " +
                                  $" ln={employee.LastName}, idSubdivision={employee.IdSubdivision}");
            if (employee != null && !string.IsNullOrEmpty(employee.Id) && !string.IsNullOrEmpty(employee.FirstName) && ms.Employee.GetEmployeeById(employee.Id) != null &&
                !string.IsNullOrEmpty(employee.SecondName) && !string.IsNullOrEmpty(employee.LastName) &&
                !string.IsNullOrEmpty(employee.Login) && !string.IsNullOrEmpty(employee.Password) &&
                employee.IdSubdivision.HasValue)
            {
                if (ms.Employee.GetEmployeeById(employee.Id) == null)
                {
                    return BadRequest(new { message = "Ошибка. Такого пользователя не существует" });
                }
                else
                {
                    ms.Employee.SaveEmployee(employee);
                    return Ok(new { message = "Сотрудник обновлен" });
                }
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены или такого сотрудника нет" });
        }

        [HttpPost("DeleteEmployee")]
        public async Task<IActionResult> UpdateEmployee(string? employeeId)
        {
            if (employeeId.IsNullOrEmpty()) return BadRequest(new { message = "Ошибка. Поле не заполнено" });
            if (ms.Employee.GetEmployeeById(employeeId) != null)
            {
                ms.Employee.DeleteEmployeeById(employeeId);
                return Ok(new { message = "Сотрудник удален" });
            }
            return BadRequest(new { message = "Ошибка. Пользователь не найден" });
        }
        /*
         *  Competence
         */
        [HttpGet("GetCompetences")]
        public async Task<IActionResult> GetCompetences()
        {
            return Ok(ms.TestType.GetAllCompetenceDtos());
        }

        [HttpGet("GetCompetence")]
        public async Task<IActionResult> GetCompetence(int? competenceId)
        {
            logger.LogInformation($"/admin-api/GetCompetence :Id={competenceId}");
            if (!competenceId.HasValue) return BadRequest(new { messsgae = "Ошибка. Поле не заполнено" });
            Competence? competence = ms.TestType.GetCompetenceById(competenceId.Value);
            if (competence != null)
            {
                return Ok(ms.TestType.GetCompetenceDtoById(competenceId.Value));
            }
            return NotFound(new { message = "Ошибка. Компетенция не найдена" });
        }

        [HttpPost("AddCompetence")]
        public async Task<IActionResult> AddCompetence(AddCompetenceModel? competenceDto)
        {
            logger.LogInformation($"/admin-api/AddCompetence :Name={competenceDto.Name}");
            if (!string.IsNullOrEmpty(competenceDto.Name))
            {
                ms.TestType.SaveCompetence(competenceDto);
                return Ok(new { message = "Компетенция добавлена" });
            }
            return BadRequest(new { message = "Ошибка при добавлении компетенции" });
        }

        [HttpPost("UpdateCompetence")]
        public async Task<IActionResult> UpdateCompetence(CompetenceDto? competenceDto)
        {
            logger.LogInformation($"/admin-api/UpdateCompetence :Id={competenceDto.Id}, Name={competenceDto.Name}");

            if (competenceDto == null && !competenceDto.Id.HasValue && !competenceDto.Name.IsNullOrEmpty())
            {
                return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
            }
            if (ms.TestType.GetCompetenceById(competenceDto.Id.Value) != null)
            {
                ms.TestType.SaveCompetence(competenceDto);
                return Ok(new { message = "Компетенция обновлена" });
            }
            return BadRequest(new { message = "ошибка. Такой компетенции не существует" });
        }

        [HttpPost("DeleteCompetence")]
        public async Task<IActionResult> DeleteCompetence(int? competenceId)
        {
            logger.LogInformation($"/admin-api/DeleteCompetence :competenceId={competenceId}");

            if (!competenceId.HasValue) return BadRequest(new { message = "Ошибка. Поле не заполнено" });
            if (ms.TestType.GetCompetenceById(competenceId.Value) != null)
            {
                ms.TestType.DeleteCompetenceById(competenceId.Value);
                return Ok(new { message = "Компетенция удалена" });
            }
            return BadRequest(new { message = "Ошибка. Компетенция не найдена" });
        }
        /*
         *  Test
         */
        [HttpGet("GetTests")]
        public async Task<IActionResult> GetTests()
        {
            logger.LogInformation($"/admin-api/GetTests ");
            return Ok(ms.Test.GetAllTestGetModels());
        }

        [HttpGet("GetTest")]
        public async Task<IActionResult> GetTest(string? id)
        {
            logger.LogInformation($"/admin-api/GetTest :id={id}");
            if (!string.IsNullOrEmpty(id))
            {
                Test? test = ms.Test.GetTestById(id);
                if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });
                List<Question> questions = ms.Question.GetAllQuestions()
                    .Where(x => x.IdTest.Equals(id)).ToList();

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
                            secondPartDtos.Add(ms.SecondPart.GetSecondPartDtoByFirstPartId(firstPart.IdFirstPart));
                        }
                        createQuestionDto.Answers.AddRange(secondPartDtos);
                    }

                    testDto.Questions.Add(createQuestionDto);
                }
                return Ok(testDto);
            }
            return BadRequest(new { message = "Ошибка. Поле не заполнено" });
        }

        [HttpGet("GetPdfTest")]
        public async Task<IActionResult> GetPdfTest(string? id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Test? test = ms.Test.GetTestById(id);
                if (test == null) return NotFound(new { message = "Ошибка. Тест не найден" });
                string html = "<div><h1 style=\"text-align: left;\">Название Теста: " + test.Name +"</h1>\r\n " +
                              "<h2 style=\"text-align: left;\">Компетенция: "+ ms.TestType.GetCompetenceById(test.IdCompetence.Value).Name +"</h2>\r\n<hr>";
                List<Question> questions = ms.Question.GetAllQuestions()
                    .Where(x => x.IdTest.Equals(id)).ToList();

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
                        if (System.IO.File.Exists(environment.WebRootFileProvider.GetFileInfo("images/"+quest.ImagePath).PhysicalPath))
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
                                    byte[] imageBytes = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/" + quest.ImagePath).PhysicalPath);
                                    //html += "<style>\r\n    img.logo { \r\n        width:auto;\r\n        height:50px;\r\n        content: url('data:image/jpeg;base64," + Convert.ToBase64String(imageBytes) + "')\r\n    }\r\n</style>";
                                    //string path = "https://" + Request.Host+"/"+answer.ImagePath;
                                    /*html += " img.logo { width:110px;height:110px;content: url('data:image/jpeg;base64," + Convert.ToBase64String(imageBytes) + "')} ";
                                    html += "<p>&#10065 " + "<img class=\"logo\"></img>" +
                                            answer.Text + "</p>";*/
                                    byte[] array = System.IO.File.ReadAllBytes(environment.WebRootFileProvider.GetFileInfo("images/logo.jpg").PhysicalPath);
                                    string base64 = Convert.ToBase64String(array);
                                    html += $"<img src='data:image/png;base64,{base64}'/> <p>&#10065{answer.Text}</p>";
                                }
                            }
                            else
                            {
                                html += "<p>&#10065 " + answer.Text + "</p>";
                            }
                        }
                    }
                    if (ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id).Count != 0)
                    {
                        foreach (SubsequenceDto sub in ms.Subsequence.GetSubsequenceDtosByQuestionId(quest.Id))
                        {
                            html += "<p>&#10065 " +sub.Text + "</p>";
                        }
                    }
                    if (ms.FirstPart.GetAllFirstPartDtosByQuestionId(quest.Id).Count != 0)
                    {
                        List<FirstSecondPartDto> list = ms.GetFirstSecondPartDtoByQuestion(quest.Id);
                        string[,] array = new string[2,list.Count];
                        int position = 0;
                        foreach(FirstSecondPartDto dto in list)
                        {
                            array[0,position] = dto.FirstPartText;
                            array[1,position] = dto.SecondPartText;
                            position++;
                        }
                        Random rnd = new Random();
                        //array = array.OrderBy(x => rnd.Next()).ToArray();
                        
                        for (int i = 0; i < array.GetLength(0);i++)
                        {
                            html += "<p style=\"white-space: pre;\">" + array[0,i] +"                          " + array[1,i] +"</p>";
                        }
                    }
                }
                html += "<hr>\r\n    <p>Дата прохождения теста:_________________________________________</p>\r\n    <p>ФИО:_________________________________________</p>\r\n    <p>Подпись:___________________</p>";
                html += "</div>";
                //var doc = new PdfDocument();
                PdfGenerator.AddPdfPages(doc,html,PageSize.A4);

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
                return NotFound(new { message = "Ошибка. Тест не найден" });
            }

        [HttpPost("AddTest")]
        public async Task<IActionResult> AddTest([FromForm] AddPostTestModel? postModel)
        {
            AddTestModel? test = JsonConvert.DeserializeObject<AddTestModel>(postModel.Test);

            if (test != null && !test.Name.IsNullOrEmpty() &&
                test.CompetenceId.HasValue && test.Questions.Count != 0) 
            {
                logger.LogInformation($"/user-api/AddTest test: name={test.Name}, idCompetence={test.CompetenceId}," +
                      $"countOfQuestions={test.Questions.Count}");

                string idTest = Guid.NewGuid().ToString();
                ms.Test.SaveTest(new Test
                {
                    Id = idTest,
                    Name = test.Name,
                    IdCompetence = test.CompetenceId
                });

                foreach (QuestionModel quest in test.Questions)
                {
                    logger.LogInformation($"quest -> text={quest.Text} idType={quest.IdQuestionType} count={quest.Answers.Count}");

                    string idQuestion = Guid.NewGuid().ToString();

                    if (!quest.ImagePath.IsNullOrEmpty())
                    {
                        IFormFile file = postModel.Files.First(f => f.FileName.Equals(quest.ImagePath));
                        string saveImage = Path.Combine(environment.WebRootPath+"/images/", file.FileName);
                        string ext = Path.GetExtension(saveImage);
                        if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                        {
                            using (var upload = new FileStream(saveImage, FileMode.Create))
                            {
                                await file.CopyToAsync(upload);
                            }
                            /*using (FileStream fs = System.IO.File.Create(environmentironment.WebRootPath + file.FileName))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            };*/
                ms.Question.SaveQuestion(new Question
                            {
                                Id = idQuestion,
                                Text = quest.Text,
                                IdQuestionType = quest.IdQuestionType,
                                ImagePath = file.FileName,
                                IdTest = idTest
                            });
                        }
                    }
                    else
                    {

                        ms.Question.SaveQuestion(new Question
                        {
                            Id = idQuestion,
                            Text = quest.Text,
                            IdQuestionType = quest.IdQuestionType,
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

                            if (!answerDto.ImagePath.IsNullOrEmpty())
                            {
                                IFormFile file = postModel.Files.GetFile(answerDto.ImagePath);
                                string saveImage = Path.Combine(environment.WebRootPath + "/images/", file.FileName);
                                string ext = Path.GetExtension(saveImage);
                                if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                {
                                    using (var upload = new FileStream(saveImage, FileMode.Create))
                                    {
                                        await file.CopyToAsync(upload);
                                    }
                                    /*using (FileStream fs = new FileStream(environmentironment.WebRootPath + file.FileName, FileMode.Create))
                                    {
                                        file.CopyTo(fs);
                                        fs.Flush();
                                    };*/
                                    ms.Answer.SaveAnswer(new Answer
                                    {
                                        Text = answerDto.Text,
                                        IdQuestion = idQuestion,
                                        Correct = answerDto.Correct,
                                        ImagePath = file.FileName
                                    });
                                }
                            }
                            else
                            {
                                ms.Answer.SaveAnswer(new Answer
                                {
                                    Text = answerDto.Text,
                                    IdQuestion = idQuestion,
                                    Correct = answerDto.Correct
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
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("UpdateTest")]
        public async Task<IActionResult> UpdateTest([FromForm] UpdatePostTestModel? updatePostModel)
        {
            UpdateTestModel? test = JsonConvert.DeserializeObject<UpdateTestModel>(updatePostModel.Test);


            if (test != null && !test.Id.IsNullOrEmpty() && !test.Name.IsNullOrEmpty() && ms.Test.GetTestById(test.Id) != null &&
                !test.Name.IsNullOrEmpty() && test.CompetenceId.HasValue &&
                test.Questions.Count != 0)
            {
                logger.LogInformation($"/user-api/AddTest test: name={test.Name}, idCompetence={test.CompetenceId}," +
                                      $"countOfQuestions={test.Questions.Count}");

                if (ms.Test.GetTestById(test.Id) == null) 
                    return BadRequest(new { message = "Ошибка. Такого теста не существует" });

                ms.Test.SaveTest(new Test
                {
                    Id = test.Id,
                    Name = test.Name,
                    IdCompetence = test.CompetenceId,
                    Weight = test.Questions.Count
                });

                foreach (QuestionModel quest in test.Questions)
                {
                    logger.LogInformation($"quest -> text={quest.Text} idType={quest.IdQuestionType} count={quest.Answers.Count}");

                    if (!quest.ImagePath.IsNullOrEmpty())
                    {
                        IFormFile file = updatePostModel.Files.First(f => f.FileName.Equals(quest.ImagePath));
                        string saveImage = Path.Combine(environment.WebRootPath, file.FileName);
                        string ext = Path.GetExtension(saveImage);
                        if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                        {
                            using (var upload = new FileStream(saveImage, FileMode.Create))
                            {
                                await file.CopyToAsync(upload);
                            }
                            ms.Question.SaveQuestion(new Question
                            {
                                Id = quest.Id,
                                Text = quest.Text,
                                IdQuestionType = quest.IdQuestionType,
                                ImagePath = file.FileName,
                                IdTest = test.Id
                            });
                        }
                    }
                    else
                    {
                        ms.Question.SaveQuestion(new Question
                        {
                            Id = quest.Id,
                            Text = quest.Text,
                            IdQuestionType = quest.IdQuestionType,
                            IdTest = test.Id
                        });
                    }
                    foreach (JObject answer in quest.Answers)
                    {
                        AnswerDto answerDto = answer.ToObject<AnswerDto>();
                        SubsequenceDto subsequenceDto = answer.ToObject<SubsequenceDto>();
                        FirstPartDto firstPartDto = answer.ToObject<FirstPartDto>();
                        SecondPartDto secondPartDto = answer.ToObject<SecondPartDto>();

                        if (answerDto is AnswerDto && answerDto.Correct != null)
                        {
                            logger.LogInformation($"answerDto -> text={answerDto.Text}, correct={answerDto.Correct}");

                            if (!answerDto.ImagePath.IsNullOrEmpty())
                            {
                                IFormFile file = updatePostModel.Files.GetFile(answerDto.ImagePath);
                                string saveImage = Path.Combine(environment.WebRootPath, file.FileName);
                                string ext = Path.GetExtension(saveImage);
                                if (ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png"))
                                {
                                    using (var upload = new FileStream(saveImage, FileMode.Create))
                                    {
                                        await file.CopyToAsync(upload);
                                    }
                                    ms.Answer.SaveAnswer(new Answer
                                    {
                                        Text = answerDto.Text,
                                        IdQuestion = quest.Id,
                                        Correct = answerDto.Correct,
                                        ImagePath = file.FileName
                                    });
                                }
                            }
                            else
                            {
                                ms.Answer.SaveAnswer(new Answer
                                {
                                    Text = answerDto.Text,
                                    IdQuestion = quest.Id,
                                    Correct = answerDto.Correct
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
                                IdQuestion = firstPartDto.IdQuestion
                            });
                        }
                        if (secondPartDto is SecondPartDto && secondPartDto.IdSecondPart.HasValue &&
                            !secondPartDto.Text.IsNullOrEmpty() && secondPartDto.IdFirstPart.IsNullOrEmpty())
                        {
                            logger.LogInformation($"secondPatDto -> Id={secondPartDto.IdFirstPart}, text={firstPartDto.Text}, IdFirstPart={secondPartDto.IdFirstPart}");
                            ms.SecondPart.SaveSecondPart(new SecondPart
                            {
                                Id = secondPartDto.IdSecondPart.Value,
                                Text = secondPartDto.Text,
                                IdFirstPart = secondPartDto.IdFirstPart
                            });
                        }
                    }
                }
                return Ok(new { message = "Обновление теста успешно" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены " });
        }

        [HttpPost("DeleteTest")]
        public async Task<IActionResult> DeleteTest(string? id)
        {
            if (id.IsNullOrEmpty()) return BadRequest(new { message = "Ошибка. Поле не заполнено" });
            if (ms.Test.GetTestById(id) != null)
            {
                ms.DeleteTestById(id);
                return Ok(new { message = "Тест удален успешно" });
            }
            return NotFound(new { message = "Ошибка. Тест не найден" });
        }
        /*
         *  Purpose
         */
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
                return BadRequest(new { message = "Ошибка. Поле не заполнено" });
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
            return BadRequest(new { message = "Ошибка." });
        }

        [HttpPost("AddPurpose")]
        public async Task<IActionResult> AddPurpose(AddTestPurposeModel? purpose)
        {
            logger.LogInformation($"/admin-api/AddPurpose employeeId={purpose.IdEmployee}, testId={purpose.IdTest}, datatime={purpose.DatatimePurpose}");
            if (purpose != null && !purpose.IdTest.IsNullOrEmpty() &&
                !purpose.IdEmployee.IsNullOrEmpty() && !purpose.DatatimePurpose.IsNullOrEmpty())
            {
                ms.TestPurpose.SaveTestPurpose(purpose);
                return Ok(new { message = "Тест назначен" });
            }
            return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
        }

        [HttpPost("AddPurposesBySubdivision")]
        public async Task<IActionResult> AddPurposesBySubdivision(string? testId, int? idSubdivision, string? time)
        {
            logger.LogInformation($"/admin-api/AddPurposesBySubdivision testId={testId}, idSubdivision={idSubdivision}");
            if (!testId.IsNullOrEmpty() && idSubdivision.HasValue && !time.IsNullOrEmpty())
            {
                ms.TestPurpose.SavePurposeBySubdivisionId(testId, idSubdivision.Value, DateTime.Parse(time));
                return Ok(new { message = "Тесты назначены" });
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

        [HttpPost("UpdatePurpose")]
        public async Task<IActionResult> UpdatePurpose(TestPurposeDto? purpose)
        {
            if (purpose == null || !purpose.Id.HasValue || purpose.IdTest.IsNullOrEmpty() || purpose.DatatimePurpose.IsNullOrEmpty())
                return BadRequest(new { message = "Ошибка. Не все поля заполнены" });
            if (ms.TestPurpose.GetTestPurposeById(purpose.Id.Value) != null)
            {
                ms.TestPurpose.SaveTestPurpose(purpose);
                return Ok(new { message = "Назначение обновлено" });
            }
            return NotFound(new { message = "Ошибка. Такого назначения не существует" });
        }

        [HttpPost("DeletePurpose")]
        public async Task<IActionResult> DeletePurpose(int? purposeId)
        {
            if (!purposeId.HasValue) return BadRequest(new { message = "Ошибка.Не все поля заполнены" });
            if (ms.TestPurpose.GetTestPurposeById(purposeId.Value) != null)
            {
                ms.TestPurpose.DeleteTestPurposeById(purposeId.Value);
                return Ok(new { message = "Назначение удалено" });
            }
            return NotFound(new { message = "Ошибка. Такого назначения не существует" });
        }

        /*
         *  Result
         */
        /*[HttpGet("GetResults")]
        public async Task<IActionResult> GetResults()
        {
            return Ok(ms.EmployeeResult.GetAllEmployeeResultModels());
        }*/

        [HttpGet("GetResults")]
        public async Task<IActionResult> GetResults(int? idSubdivision, string? FirstName,
                                                    string? SecondName, string? LastName,
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
                    list = list.OrderBy(x => x.ScoreFrom).ToList();
                }
                if (sortType.Equals("score↓"))//↓
                {
                    list = list.OrderByDescending(x => x.ScoreFrom).ToList();
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
                ms.EmployeeAnswer.DeleteEmployeeAnswersByResult(result.IdResult);
                ms.EmployeeMatching.DeleteEmployeeMatchingByResult(result.IdResult);
                ms.EmployeeSubsequence.DeleteEmployeeSubsequenceByResult(result.IdResult);
                ms.Result.DeleteResultById(result.IdResult);
                ms.EmployeeResult.DeleteEmployeeResultById(result.Id.Value);
            }
            return Ok(new { message = "Результаты удалены" });
        }

        [HttpPost("DeleteResult")]
        public async Task<IActionResult> DeleteResult(int resultId)
        {
            EmployeeResult employeeResults = ms.EmployeeResult.GetEmployeeResultById(resultId);
            ms.EmployeeAnswer.DeleteEmployeeAnswersByResult(employeeResults.IdResult);
            ms.EmployeeMatching.DeleteEmployeeMatchingByResult(employeeResults.IdResult);
            ms.EmployeeSubsequence.DeleteEmployeeSubsequenceByResult(employeeResults.IdResult);
            ms.Result.DeleteResultById(employeeResults.IdResult);
            ms.EmployeeResult.DeleteEmployeeResultById(resultId);
            return Ok(new { message = "Результаты удалены" });
        }
    }
}
