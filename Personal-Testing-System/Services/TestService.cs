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
        private CompetenceService competenceService;
        public TestService(ITestRepo _testRepo, CompetenceService competenceService)
        {
            this.TestRepo = _testRepo;
            this.competenceService = competenceService;
        }

        private async Task<TestGetModel> ConvertToGetTestModel(Test test)
        {
            CompetenceDto competenceDto = await competenceService.GetCompetenceDtoById(test.IdCompetence.Value);
            return new TestGetModel
            {
                Id = test.Id,
                Name = test.Name,
                Competence = competenceDto,
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
    }
}
