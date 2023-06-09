using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class TestPurposeService
    {
        private ITestPurposeRepo testPurposeRepo;
        public TestPurposeService(ITestPurposeRepo _testPurposeRepo)
        {
            this.testPurposeRepo = _testPurposeRepo;
        }
        public void DeleteTestPurposeById(int id)
        {
            testPurposeRepo.DeleteTestPurposeById(id);
        }

        public List<TestPurpose> GetAllTestPurposes()
        {
            return testPurposeRepo.GetAllTestPurposes();
        }

        public TestPurpose GetTestPurposeById(int id)
        {
            return testPurposeRepo.GetTestPurposeById(id);
        }

        public void SaveTestPurpose(TestPurpose TestPurposeToSave)
        {
            testPurposeRepo.SaveTestPurpose(TestPurposeToSave);
        }
    }
}
