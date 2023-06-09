using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class TestTypeService
    {
        private ITestTypeRepo testTypeRepo;
        public TestTypeService(ITestTypeRepo _testTypeRepo)
        {
            this.testTypeRepo = _testTypeRepo;
        }
        public void DeleteTestTypeById(int id)
        {
            testTypeRepo.DeleteTestTypeById(id);
        }

        public List<TestType> GetAllTestTypes()
        {
            return testTypeRepo.GetAllTestTypes();
        }

        public TestType GetTestTypeById(int id)
        {
            return testTypeRepo.GetTestTypeById(id);
        }

        public void SaveTestType(TestType TestTypeToSave)
        {
            testTypeRepo.SaveTestType(TestTypeToSave);
        }
    }
}
