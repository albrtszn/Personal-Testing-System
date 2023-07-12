using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class TestPurposeService
    {
        private ITestPurposeRepo testPurposeRepo;
        public TestPurposeService(ITestPurposeRepo _testPurposeRepo)
        {
            this.testPurposeRepo = _testPurposeRepo;
        }

        private TestPurpose ConvertToTestPurpose(TestPurposeDto testPurposeDto)
        {
            return new TestPurpose
            {
                Id = testPurposeDto.Id.Value,
                IdEmployee = testPurposeDto.IdEmployee,
                IdTest = testPurposeDto.IdTest,
                DatatimePurpose = DateTime.Parse(testPurposeDto.DatatimePurpose)
            };
        }

        private TestPurposeDto ConvertToTestPurposeDto(TestPurpose testPurpose)
        {
            return new TestPurposeDto
            {
                Id = testPurpose.Id,
                IdEmployee = testPurpose.IdEmployee,
                IdTest = testPurpose.IdTest,
                DatatimePurpose = testPurpose.DatatimePurpose.ToString()
            };
        }

        public void DeleteTestPurposeById(int id)
        {
            testPurposeRepo.DeleteTestPurposeById(id);
        }

        public List<TestPurpose> GetAllTestPurposes()
        {
            return testPurposeRepo.GetAllTestPurposes();
        }

        public List<TestPurposeDto> GetAllTestPurposeDtos()
        {
            List<TestPurposeDto> TestPurposes = new List<TestPurposeDto>();
            testPurposeRepo.GetAllTestPurposes().ForEach(x => TestPurposes.Add(ConvertToTestPurposeDto(x)));
            return TestPurposes;
        }

        public TestPurpose GetTestPurposeById(int id)
        {
            return testPurposeRepo.GetTestPurposeById(id);
        }

        public TestPurposeDto GetTestPurposeDtoById(int id)
        {
            return ConvertToTestPurposeDto(testPurposeRepo.GetTestPurposeById(id));
        }

        public void SaveTestPurpose(TestPurpose TestPurposeToSave)
        {
            testPurposeRepo.SaveTestPurpose(TestPurposeToSave);
        }

        public void SaveTestPurposeDto(TestPurposeDto TestPurposeDtoToSave)
        {
            testPurposeRepo.SaveTestPurpose(ConvertToTestPurpose(TestPurposeDtoToSave));
        }
    }
}
