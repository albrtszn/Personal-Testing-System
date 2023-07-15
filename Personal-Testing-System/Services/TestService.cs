using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class TestService
    {
        private ITestRepo testRepo;
        private CompetenceService competenceService;
        public TestService(ITestRepo _testRepo, CompetenceService competenceService)
        {
            this.testRepo = _testRepo;
            this.competenceService = competenceService;
        }

        private TestGetModel ConvertToGetTestModel(Test test)
        {
            CompetenceDto competenceDto = competenceService.GetCompetenceDtoById(test.IdCompetence.Value);
            return new TestGetModel
            {
                Id = test.Id,
                Name = test.Name,
                Competence = competenceDto
            };
        }

        public void DeleteTestById(int id)
        {
            testRepo.DeleteTestById(id);
        }

        public List<Test> GetAllTests()
        {
            return testRepo.GetAllTests();
        }



        public List<TestGetModel> GetAllTestGetModels()
        {
            List<TestGetModel> list = new List<TestGetModel>();
            testRepo.GetAllTests().ForEach(x => list.Add(ConvertToGetTestModel(x)));
            return list;
        }

        public Test GetTestById(string id)
        {
            return testRepo.GetTestById(id);
        }


        public TestGetModel GetTestGetModelById(string id)
        {
            return ConvertToGetTestModel(testRepo.GetTestById(id));
        }

        public void SaveTest(Test TestToSave)
        {
            testRepo.SaveTest(TestToSave);
        }
    }
}
