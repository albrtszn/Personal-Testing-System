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
        private EmployeeResultSubcompetenceService employeeResultSubcompetenceServidce;
        private QuestionSubcompetenceService questionSubcompetenceService;
        private SubcompetenceService subcompetenceService;
        private GlobalConfigureService globalConfigureService;
        private MessageService messageService;
        private TestScoreService testScoreService;
        private SubcompetenceScoreService subcompetenceScoreService;
        
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
                       ProfileService _profileService, GroupPositionService _groupPositionService, CompetenciesForGroupService _competenciesForGroupService,
                       EmployeeResultSubcompetenceService _employeeResultSubcompetenceServidce, QuestionSubcompetenceService _questionSubcompetenceService,
                       SubcompetenceService _subcompetenceService, MessageService _messageService,
                       GlobalConfigureService _globalConfigureService, TestScoreService _testScoreService,
                       SubcompetenceScoreService _subcompetenceScoreService,
                        IConfiguration _config)
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
            employeeResultSubcompetenceServidce = _employeeResultSubcompetenceServidce;
            questionSubcompetenceService = _questionSubcompetenceService;
            subcompetenceService = _subcompetenceService;
            messageService = _messageService;
            globalConfigureService = _globalConfigureService;
            testScoreService = _testScoreService;
            subcompetenceScoreService = _subcompetenceScoreService;

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
        public EmployeeResultSubcompetenceService EmployeeResultSubcompetence { get { return employeeResultSubcompetenceServidce; } }
        public QuestionSubcompetenceService QuestionSubcompetence { get { return questionSubcompetenceService; } }
        public SubcompetenceService Subcompetence { get { return subcompetenceService; } }
        public MessageService Message { get { return messageService; } }
        public GlobalConfigureService GlobalConfigure { get { return globalConfigureService; } }
        public TestScoreService TestScore { get { return testScoreService; } }
        public SubcompetenceScoreService SubcompetenceScore { get { return subcompetenceScoreService; } }

        /*
         *  Logic
         */

        /*
         *  TokenEmployee
         */
        private double hoursToExpireEmployeeToken = 5.0;
        public async Task<bool> IsTokenEmployeeExpired(TokenEmployee tokenEmployee)
        {
            double ttl = config.GetValue<double>("TokenTimeToLiveInHours:EmployeeToken");
            if (ttl > 0)
            {
                hoursToExpireEmployeeToken = ttl;
            }

            DateTime? dateTime = tokenEmployee.IssuingTime.Value;
            if (dateTime.Value.AddHours(hoursToExpireEmployeeToken) <= DateTime.Now)// && DateTime.Now.Subtract(dateTime.Value).TotalHours >= 24)
            {
                await TokenEmployee.DeleteTokenEmployeeById(tokenEmployee.Id);
                return true;
            }
            else
            {
                tokenEmployee.IssuingTime = DateTime.Now;
                await TokenEmployee.SaveTokenEmployee(tokenEmployee);
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
                tokenAdmin.IssuingTime = DateTime.Now;
                await TokenAdmin.SaveTokenAdmin(tokenAdmin);
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
            List<Question> quests = (await Question.GetQuestionsByTest(id));
            foreach (Question quest in quests)
            {
                List<Answer> answerList = (await Answer.GetAnswersByQuestionId(quest.Id));
                foreach (Answer answer in answerList)
                {
                    string answerFilePath = env.WebRootFileProvider.GetFileInfo("/images/" + answer.ImagePath).PhysicalPath;
                    if (!string.IsNullOrEmpty(answer.ImagePath) && System.IO.File.Exists(answerFilePath))
                    {
                        System.IO.File.Delete(answerFilePath);
                    }
                    await Answer.DeleteAnswerById(answer.Id);
                }
                //answerList.ForEach(async x => await Answer.DeleteAnswerById(x.Id));

                List<Subsequence> subs = (await Subsequence.GetSubsequencesByQuestionId(quest.Id));
                foreach(Subsequence subsequence in subs)
                {
                    await Subsequence.DeleteSubsequenceById(subsequence.Id);
                }

                List<FirstPart> list = (await FirstPart.GetFirstPartsByQuestionId(quest.Id));
                foreach(FirstPart firstPart in list)
                {
                    SecondPart sPart = await SecondPart.GetSecondPartByFirstPartId(firstPart.Id);
                    await SecondPart.DeleteSecondPartById(sPart.Id);
                    await FirstPart.DeleteFirstPartById(firstPart.Id);
                }
                /*list.ForEach(async x => await SecondPart.DeleteSecondPartById((await SecondPart.GetSecondPartDtoByFirstPartId(x.Id)).IdSecondPart.Value));
                list.ForEach(async x => await FirstPart.DeleteFirstPartById(x.Id));*/
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
            if (result == null) return null;
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

        private async Task<List<ResultSubcompetenceModel>> GetResultSubcompetenceModels(string IdResult)
        {
            List<ResultSubcompetenceModel> list = new List<ResultSubcompetenceModel>();
            List<ElployeeResultSubcompetence> resultSubcompetences = (await EmployeeResultSubcompetence.GetAllEmployeeResultSubcompetences())
                    .Where(x => x!=null && x.IdResult != null && x.IdResult.Equals(IdResult))
                    .ToList();
            foreach(var resultSubcompetence in resultSubcompetences)
            {
                list.Add(new ResultSubcompetenceModel()
                {
                    Id = resultSubcompetence.Id,
                    IdResult = resultSubcompetence.IdResult,
                    Subcompetence = await Subcompetence.GetSubcompetenceDtoById(resultSubcompetence.IdSubcompetence.Value),
                    Result = resultSubcompetence.Result
                });
            }
            return list;
        }

        private async Task<EmployeeResultSubcompetenceModel> ConvertToEmployeeResultSubcompetenceModel(EmployeeResult EmployeeResult)
        {
            List<ResultSubcompetenceModel>? subCompetenceResults = await GetResultSubcompetenceModels(EmployeeResult.IdResult);
            return new EmployeeResultSubcompetenceModel
            {
                Id = EmployeeResult.Id,
                ScoreFrom = EmployeeResult.ScoreFrom,
                ScoreTo = EmployeeResult.ScoreTo,
                SubcompetenceResults = subCompetenceResults,
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
            foreach (EmployeeResult result in  await EmployeeResult.GetAllEmployeeResults())
            {
                list.Add(await ConvertToEmployeeResultModel(result));
            } 
            //().ForEach(async x => list.Add(await ConvertToEmployeeResultModel(x)));
            return list;
        }
        public async Task<List<EmployeeResultSubcompetenceModel>?> GetAllEmployeeResultSubcompetenceModels()
        {
            List<EmployeeResultSubcompetenceModel> list = new List<EmployeeResultSubcompetenceModel>();
            foreach (EmployeeResult result in await EmployeeResult.GetAllEmployeeResults())
            {
                list.Add(await ConvertToEmployeeResultSubcompetenceModel(result));
            }
            //().ForEach(async x => list.Add(await ConvertToEmployeeResultModel(x)));
            return list;
        }
        public async Task<List<EmployeeResultSubcompetenceModel>?> GetAllEmployeeResultSubcompetenceModelsByEmployee(string idEmployee)
        {
            List<EmployeeResultSubcompetenceModel> list = new List<EmployeeResultSubcompetenceModel>();
            foreach (EmployeeResult result in (await EmployeeResult.GetAllEmployeeResults()).Where(x=> x!=null && x.IdEmployee != null && x.IdEmployee.Equals(idEmployee)))
            {
                list.Add(await ConvertToEmployeeResultSubcompetenceModel(result));
            }
            //().ForEach(async x => list.Add(await ConvertToEmployeeResultModel(x)));
            return list;
        }
        public async Task<List<EmployeeResultSubcompetenceModel>?> GetAllEmployeeResultSubcompetenceModelsByEmployeeResultId(int idEmployeeResult)
        {
            List<EmployeeResultSubcompetenceModel> list = new List<EmployeeResultSubcompetenceModel>();
            foreach (EmployeeResult result in (await EmployeeResult.GetAllEmployeeResults()).Where(x => x != null && x.Id.Equals(idEmployeeResult)))
            {
                list.Add(await ConvertToEmployeeResultSubcompetenceModel(result));
            }
            //().ForEach(async x => list.Add(await ConvertToEmployeeResultModel(x)));
            return list;
        }

        public async Task<List<EmployeeResultModel>> GetAllEmployeeResultModelsByEmployeeId(string employeeId)
        {
            List<EmployeeResultModel> list = new List<EmployeeResultModel>();
            List<EmployeeResult> employeeResults = (await EmployeeResult.GetAllEmployeeResults())
                .Where(x => x!= null && x.IdEmployee !=null && x.IdEmployee.Equals(employeeId))
                .ToList();
            foreach (EmployeeResult res in employeeResults)
            {
                list.Add(await ConvertToEmployeeResultModel(res));
            }
            return list;
        }
    }
}
