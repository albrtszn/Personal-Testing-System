using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class TestResultService
    {
        private ITestResultRepo testResultRepo;
        public TestResultService(ITestResultRepo _testResultRepo)
        {
            this.testResultRepo = _testResultRepo;
        }
        public void DeleteById(int id)
        {
            testResultRepo.DeleteTestResultById(id);
        }

        public List<TestResult> GetAllTestResults()
        {
            return testResultRepo.GetAllTestResults(); 
        }

        public TestResult GetTestResultById(int id)
        {
            return testResultRepo.GetTestResultById(id);
        }

        public void SaveTestResult(TestResult TestResultToSave)
        {
            testResultRepo.SaveTestResult(TestResultToSave);
        }
    }
}
