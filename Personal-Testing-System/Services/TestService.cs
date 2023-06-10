using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class TestService
    {
        private ITestRepo testRepo;
        public TestService(ITestRepo _testRepo)
        {
            this.testRepo = _testRepo;
        }
        public void DeleteTestById(int id)
        {
            testRepo.DeleteTestById(id);
        }

        public List<Test> GetAllTests()
        {
            return testRepo.GetAllTests();
        }

        public Test GetTestById(string id)
        {
            return testRepo.GetTestById(id);
        }

        public void SaveTest(Test TestToSave)
        {
            testRepo.SaveTest(TestToSave);
        }
    }
}
