using CRUD.implementations;
using DataBase.Repository.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;
using Personal_Testing_System.Services;
using SixLabors.ImageSharp;
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
        private ProfileService profileService;
        private GroupPositionService groupPositionService;
        private CompetenciesForGroupService competenciesForGroupService;
        
        private IConfiguration config;

        public MasterService(AnswerService _answerService, EmployeeAnswerService _employeeAnswerService,
                       EmployeeMatchingService _employeeMatchingService, EmployeeService _employeeService,
                       EmployeeSubsequenceService _employeeSubsequenceService, FirstPartService _firstPartService,
                       QuestionService _questionService, QuestionTypeService _questionTypeServiceService, SecondPartService _secondPartService,
                       SubdivisionService _subdivisionPartService, SubsequenceService _subsequenceService,
                       TestPurposeService _testPurposeService, TestService _testService,
                       CompetenceService _competenceService, AdminService _adminService,
                       ResultService _resultService, EmployeeResultService _employeeResultService,
                       LogService _logService, TokenEmployeeService _tokenEmployeeService, TokenAdminService _tokenAdminService,
                       ProfileService _profileService, GroupPositionService _groupPositionService, CompetenciesForGroupService _competenciesForGroupService, IConfiguration _config)
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
            profileService = _profileService;
            groupPositionService = _groupPositionService;
            competenciesForGroupService = _competenciesForGroupService;

            config = _config;
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
        public ProfileService Profile { get { return profileService; } }
        public GroupPositionService GroupPosition { get { return groupPositionService; } }
        public CompetenciesForGroupService CompetenciesForGroup { get { return competenciesForGroupService; } }
        /*
         *  Logic
         */

        /*
         *  TokenEmployee
         */
        private double hoursToExpireEmployeeToken = 2.0;
        public async Task<bool> IsTokenEmployeeExpired(TokenEmployee tokenEmployee)
        {
            double ttl = config.GetValue<double>("TokenTimeToLiveInHours:EmployeeToken");
            if (ttl > 0)
            {
                hoursToExpireEmployeeToken = ttl;
            }

            DateTime? dateTime = tokenEmployee.IssuingTime.Value;
            if (dateTime.Value.AddHours(hoursToExpireEmployeeToken) <= DateTime.Now)
            {
                await TokenEmployee.DeleteTokenEmployeeById(tokenEmployee.Id);
                return true;
            }
            else
            {
                return false;
            }
        }

        private double hoursToExpireAdminToken = 5.0;
        public async Task<bool> IsTokenAdminExpired(TokenAdmin tokenAdmin)
        {
            double ttl = config.GetValue<double>("TokenTimeToLiveInHours:AdminToken");
            if (ttl > 0)
            {
                hoursToExpireAdminToken = ttl;
            }

            DateTime? dateTime = tokenAdmin.IssuingTime.Value;
            if (dateTime.Value.AddHours(hoursToExpireAdminToken) <= DateTime.Now)
            {
                await TokenAdmin.DeleteTokenAdminById(tokenAdmin.Id);
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
        public async Task<List<FirstSecondPartDto>> GetFirstSecondPartDtoByQuestion(string id)
        {
            List<FirstSecondPartDto> firstSecondPartDtoList = new List<FirstSecondPartDto>();
            List<FirstPart> list = (await FirstPart.GetAllFirstParts()).Where(x => x.IdQuestion.Equals(id)).ToList();
            foreach (FirstPart fp in list)
            {
                firstSecondPartDtoList.Add(new FirstSecondPartDto
                {
                    FirstPartText = fp.Text,
                    SecondPartText = (await SecondPart.GetSecondPartDtoByFirstPartId(fp.Id)).Text
                });
            }
            return firstSecondPartDtoList;
        }

        public async Task<bool> DeleteFirstAndSecondPartsByQuestion(string idQuestion)
        {
            List<FirstPart> listFP = (await FirstPart.GetAllFirstParts()).Where(x => x.IdQuestion.Equals(idQuestion)).ToList();
            if (listFP.Count > 0)
            {
                foreach (FirstPart fp in listFP)
                {
                    int secondPartId = (await SecondPart.GetSecondPartDtoByFirstPartId(fp.Id)).IdSecondPart.Value;
                    await SecondPart.DeleteSecondPartById(secondPartId);

                    await FirstPart.DeleteFirstPartById(fp.Id);
                }
            }
            return true;
        }
        /*
         *  Test
         */
        public async Task<bool> DeleteTestById(string id, IWebHostEnvironment env)
        {
            foreach (Question quest in (await Question.GetAllQuestions()).Where(x => x.IdTest.Equals(id)).ToList())
            {
                List<Answer> answerList = (await Answer.GetAllAnswers()).Where(x => x.IdQuestion.Equals(quest.Id)).ToList();
                foreach (Answer answer in answerList)
                {
                    string answerFilePath = env.WebRootFileProvider.GetFileInfo("/images/" + answer.ImagePath).PhysicalPath;
                    if (!string.IsNullOrEmpty(answer.ImagePath) && System.IO.File.Exists(answerFilePath))
                    {
                        System.IO.File.Delete(answerFilePath);
                    }
                }
                answerList.ForEach(async x => await Answer.DeleteAnswerById(x.Id));

                (await Subsequence.GetAllSubsequences()).Where(x => x.IdQuestion.Equals(quest.Id)).ToList()
                      .ForEach(async x => await Subsequence.DeleteSubsequenceById(x.Id));
                List<FirstPart> list = (await FirstPart.GetAllFirstParts()).Where(x => x.IdQuestion.Equals(quest.Id)).ToList();
                list.ForEach(async x => await SecondPart.DeleteSecondPartById((await SecondPart.GetSecondPartDtoByFirstPartId(x.Id)).IdSecondPart.Value));
                list.ForEach(async x => await FirstPart.DeleteFirstPartById(x.Id));
                string path = env.WebRootFileProvider.GetFileInfo("/images/" + quest.ImagePath).PhysicalPath;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                await Question.DeleteQuestionById(quest.Id);
            }
            await Test.DeleteTestById(id);
            return true;
        }
        /*
         *  Result 
         */
        private async Task<ResultModel> GetResultModelById(Result result)
        {
            return new ResultModel
            {
                Id = result.Id,
                Test = await Test.GetTestGetModelById(result.IdTest),
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
        private async Task<EmployeeResultModel> ConvertToEmployeeResultModel(EmployeeResult EmployeeResult)
        {
            return new EmployeeResultModel
            {
                Id = EmployeeResult.Id,
                ScoreFrom = EmployeeResult.ScoreFrom,
                ScoreTo = EmployeeResult.ScoreTo,
                Employee = await employeeService.GetEmployeeModelById(EmployeeResult.IdEmployee),
                Result = await GetResultModelById( await Result.GetResultById(EmployeeResult.IdResult))
            };
        }
        public async Task<EmployeeResultModel> GetEmployeeResultModelById(int id)
        {
            return await ConvertToEmployeeResultModel(await EmployeeResult.GetEmployeeResultById(id));
        }
        public async Task<List<EmployeeResultModel>> GetAllEmployeeResultModels()
        {
            List<EmployeeResultModel> list = new List<EmployeeResultModel>();
            (await EmployeeResult.GetAllEmployeeResults()).ForEach(async x => list.Add(await ConvertToEmployeeResultModel(x)));
            return list;
        }
        public async Task<List<EmployeeResultModel>> GetAllEmployeeResultModelsByEmployeeId(string employeeId)
        {
            List<EmployeeResultModel> list = new List<EmployeeResultModel>();
            (await EmployeeResult.GetAllEmployeeResults()).Where(x => x.IdEmployee.Equals(employeeId)).ToList()
                          .ForEach(async x => list.Add( await ConvertToEmployeeResultModel(x)));
            return list;
        }
    }
}
