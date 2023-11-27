using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class TestService
    {
        private ITestRepo TestRepo;
        private IQuestionRepo QuestionRepo;
        private CompetenceService competenceService;
        public TestService(ITestRepo _testRepo, CompetenceService competenceService, IQuestionRepo questionRepo)
        {
            this.TestRepo = _testRepo;
            this.competenceService = competenceService;
            this.QuestionRepo = questionRepo;
        }

        private async Task<TestGetModel> ConvertToGetTestModel(Test test)
        {
            return new TestGetModel
            {
                Id = test.Id,
                Name = test.Name,
                CompetenceId = test.IdCompetence,
                QuestionsCount = (await QuestionRepo.GetAllQuestionsByTestId(test.Id)).Count
            };
        }

        private async Task<TestDto> ConvertToTestDto(Test test)
        {
            return new TestDto
            {
                Id = test.Id,
                Name = test.Name,
                Weight = test.Weight,
                IdCompetence = test.IdCompetence,
                Description = test.Description,
                Instruction = test.Instruction,
                Generation = test.Generation
            };
        }

        private async Task<Test> ConvertToTest(TestDto test)
        {
            return new Test
            {
                Id = test.Id,
                Name = test.Name,
                Weight = test.Weight,
                IdCompetence = test.IdCompetence,
                Description = test.Description,
                Instruction = test.Instruction,
                Generation = test.Generation
            };
        }

        public async Task<bool> DeleteTestById(string id)
        {
            return await TestRepo.DeleteTestById(id);
        }

        public async Task<List<Test>> GetAllTests()
        {
            return await TestRepo.GetAllTests();
        }

        public async Task<TestGetModel> GetTestModelById(string idTest)
        {
            Test test = await GetTestById(idTest);
            return await ConvertToGetTestModel(test);
        }

        public async Task<List<TestGetModel>> GetAllTestGetModels()
        {
            List<TestGetModel> list = new List<TestGetModel>();
            List<Test> tests = (await TestRepo.GetAllTests());
            foreach (Test test in tests)
            {
                list.Add(await ConvertToGetTestModel(test));
            }
            return list;
        }

        public async Task<Test> GetTestById(string id)
        {
            return await TestRepo.GetTestById(id);
        }

        public async Task<TestDto> GetTestDtoById(string id)
        {
            return await ConvertToTestDto(await TestRepo.GetTestById(id));
        }

        public async Task<TestGetModel> GetTestGetModelById(string id)
        {
            var test = await TestRepo.GetTestById(id);
            if (test == null) return null;
            return await ConvertToGetTestModel(test);
        }

        public async Task<bool> SaveTest(Test TestToSave)
        {
            return await TestRepo.SaveTest(TestToSave);
        }

        public async Task<bool> SaveTest(TestDto TestDtoToSave)
        {
            return await TestRepo.SaveTest(await ConvertToTest(TestDtoToSave));
        }
    }
}
