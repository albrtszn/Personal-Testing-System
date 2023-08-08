using CRUD.implementations;
using DataBase.Repository.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;
using Personal_Testing_System.Services;
using System;

namespace Personal_Testing_System.Services
{
    public class MasterService
    {
        private AnswerService answerService;
        private EmployeeAnswerService employeeAnswerService;
        private EmployeeMatchingService employeeMatchingService;
        private EmployeeService employeeService;
        private EmployeeSubsequenceService employeeSubsequenceService;
        private FirstPartService firstPartService;
        private QuestionService questionService;
        private QuestionTypeService questionTypeService;
        private SecondPartService secondPartService;
        private SubdivisionService subdivisionService;
        private SubsequenceService subsequenceService;
        private TestPurposeService testPurposeService;
        private TestService testService;
        private CompetenceService competenceService;
        private AdminService adminService;
        private ResultService resultService;
        private EmployeeResultService employeeResultService;
        private LogService logService;
        private TokenEmployeeService tokenEmployeeService;
        private TokenAdminService tokenAdminService;

        public MasterService(AnswerService _answerService, EmployeeAnswerService _employeeAnswerService,
                       EmployeeMatchingService _employeeMatchingService, EmployeeService _employeeService,
                       EmployeeSubsequenceService _employeeSubsequenceService, FirstPartService _firstPartService,
                       QuestionService _questionService, QuestionTypeService _questionTypeServiceService, SecondPartService _secondPartService,
                       SubdivisionService _subdivisionPartService, SubsequenceService _subsequenceService,
                       TestPurposeService _testPurposeService, TestService _testService,
                       CompetenceService _competenceService, AdminService _adminService,
                       ResultService _resultService, EmployeeResultService _employeeResultService,
                       LogService _logService, TokenEmployeeService _tokenEmployeeService, TokenAdminService _tokenAdminService)
        {
            answerService = _answerService;
            employeeAnswerService = _employeeAnswerService;
            employeeMatchingService = _employeeMatchingService;
            employeeService = _employeeService;
            employeeSubsequenceService = _employeeSubsequenceService;
            firstPartService = _firstPartService;
            questionService = _questionService;
            questionTypeService = _questionTypeServiceService;
            secondPartService = _secondPartService;
            subdivisionService = _subdivisionPartService;
            subsequenceService = _subsequenceService;
            testPurposeService = _testPurposeService;
            testService = _testService;
            competenceService = _competenceService;
            adminService = _adminService;
            resultService = _resultService;
            employeeResultService = _employeeResultService;
            logService = _logService;
            tokenEmployeeService = _tokenEmployeeService;
            tokenAdminService = _tokenAdminService;
        }
        //public UserService Users { get { return ; } }

        public AnswerService Answer { get { return answerService; } }
        public EmployeeAnswerService EmployeeAnswer { get { return employeeAnswerService; } }
        public EmployeeMatchingService EmployeeMatching { get { return employeeMatchingService; } }
        public EmployeeService Employee { get { return employeeService; } }
        public EmployeeSubsequenceService EmployeeSubsequence { get { return employeeSubsequenceService; } }
        public FirstPartService FirstPart { get { return firstPartService; } }
        public QuestionService Question { get { return questionService; } }
        public QuestionTypeService QuestionType { get { return questionTypeService; } }
        public SecondPartService SecondPart { get { return secondPartService; } }
        public SubdivisionService Subdivision { get { return subdivisionService; } }
        public SubsequenceService Subsequence { get { return subsequenceService; } }
        public TestPurposeService TestPurpose { get { return testPurposeService; } }
        public TestService Test { get { return testService; } }
        public CompetenceService TestType { get { return competenceService; } }
        public AdminService Admin { get { return adminService; } }
        public ResultService Result { get { return resultService; } }
        public EmployeeResultService EmployeeResult { get { return employeeResultService; } }
        public LogService Log { get { return logService; } }
        public TokenEmployeeService TokenEmployee { get { return tokenEmployeeService; } }
        public TokenAdminService TokenAdmin { get { return tokenAdminService; } }
        /*
         *  Logic
         */

        /*
         *  TokenEmployee
         */
        private double hoursToExpireEmployeeToken = 2.0;
        public bool IsTokenEmployeeExpired(TokenEmployee tokenEmployee)
        {
            DateTime? dateTime = tokenEmployee.IssuingTime.Value;
            if (dateTime.Value.AddHours(hoursToExpireEmployeeToken) <= DateTime.Now)
            {
                TokenEmployee.DeleteTokenEmployeeById(tokenEmployee.Id);
                return true;
            }
            else
            {
                return false;
            }
        }

        private double hoursToExpireAdminToken = 5.0;
        public bool IsTokenAdminExpired(TokenAdmin tokenAdmin)
        {
            DateTime? dateTime = tokenAdmin.IssuingTime.Value;
            if (dateTime.Value.AddHours(hoursToExpireAdminToken) <= DateTime.Now)
            {
                TokenAdmin.DeleteTokenAdminById(tokenAdmin.Id);
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
         *  First & Second Parts
         */
        public List<FirstSecondPartDto> GetFirstSecondPartDtoByQuestion(string id)
        {
            List<FirstSecondPartDto> firstSecondPartDtoList = new List<FirstSecondPartDto>();
            FirstPart.GetAllFirstParts().Where(x => x.IdQuestion.Equals(id)).ToList()
                .ForEach(x => firstSecondPartDtoList.Add(new FirstSecondPartDto
                {
                    FirstPartText = x.Text,
                    SecondPartText = SecondPart.GetSecondPartDtoByFirstPartId(x.Id).Text
                }));
            return firstSecondPartDtoList;
        }

        public void DeleteFirstAndSecondPartsByQuestion(string idQuestion)
        {
            List<FirstPart> listFP = FirstPart.GetAllFirstParts().Where(x => x.IdQuestion.Equals(idQuestion)).ToList();
            listFP.ForEach(x => SecondPart.DeleteSecondPartById(SecondPart.GetSecondPartDtoByFirstPartId(x.Id).IdSecondPart.Value));
            listFP.ForEach(x => FirstPart.DeleteFirstPartById(x.Id));
        }
        /*
         *  Test
         */
        public void DeleteTestById(string id, IWebHostEnvironment env)
        {
            foreach (Question quest in Question.GetAllQuestions().Where(x => x.IdTest.Equals(id)).ToList())
            {
                List<Answer> answerList = Answer.GetAllAnswers().Where(x => x.IdQuestion.Equals(quest.Id)).ToList();
                foreach (Answer answer in answerList)
                {
                    string answerFilePath = env.WebRootFileProvider.GetFileInfo("/images/" + answer.ImagePath).PhysicalPath;
                    if (!string.IsNullOrEmpty(answer.ImagePath) && System.IO.File.Exists(answerFilePath))
                    {
                        System.IO.File.Delete(answerFilePath);
                    }
                }
                answerList.ForEach(x => Answer.DeleteAnswerById(x.Id));

                Subsequence.GetAllSubsequences().Where(x => x.IdQuestion.Equals(quest.Id)).ToList()
                      .ForEach(x => Subsequence.DeleteSubsequenceById(x.Id));
                List<FirstPart> list = FirstPart.GetAllFirstParts().Where(x => x.IdQuestion.Equals(quest.Id)).ToList();
                list.ForEach(x => SecondPart.DeleteSecondPartById(SecondPart.GetSecondPartDtoByFirstPartId(x.Id).IdSecondPart.Value));
                list.ForEach(x => FirstPart.DeleteFirstPartById(x.Id));
                string path = env.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                Question.DeleteQuestionById(quest.Id);
            }
            Test.DeleteTestById(id);
        }
        /*
         *  Result 
         */
        private ResultModel GetResultModelById(Result result)
        {
            return new ResultModel
            {
                Id = result.Id,
                Test = Test.GetTestGetModelById(result.IdTest),
                StartDate = result.StartDate.ToString(),
                StartTime = result.StartTime.ToString(),
                Duration = result.Duration,
                EndTime = result.EndTime.ToString(),
                Description = result.Description
            };
        }
        /*
         *  EmployeeResult
         */
        private EmployeeResultModel ConvertToEmployeeResultModel(EmployeeResult EmployeeResult)
        {
            return new EmployeeResultModel
            {
                Id = EmployeeResult.Id,
                ScoreFrom = EmployeeResult.ScoreFrom,
                ScoreTo = EmployeeResult.ScoreTo,
                Employee = employeeService.GetEmployeeModelById(EmployeeResult.IdEmployee),
                Result = GetResultModelById(Result.GetResultById(EmployeeResult.IdResult))
            };
        }
        public EmployeeResultModel GetEmployeeResultModelById(int id)
        {
            return ConvertToEmployeeResultModel(EmployeeResult.GetEmployeeResultById(id));
        }
        public List<EmployeeResultModel> GetAllEmployeeResultModels()
        {
            List<EmployeeResultModel> list = new List<EmployeeResultModel>();
            EmployeeResult.GetAllEmployeeResults().ForEach(x => list.Add(ConvertToEmployeeResultModel(x)));
            return list;
        }
        public List<EmployeeResultModel> GetAllEmployeeResultModelsByEmployeeId(string employeeId)
        {
            List<EmployeeResultModel> list = new List<EmployeeResultModel>();
            EmployeeResult.GetAllEmployeeResults().Where(x => x.IdEmployee.Equals(employeeId)).ToList()
                          .ForEach(x => list.Add(ConvertToEmployeeResultModel(x)));
            return list;
        }
    }
}
