using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class TestPurposeService
    {
        private ITestPurposeRepo testPurposeRepo;
        private EmployeeService employeeSercvice;
        private TestService testSercvice;
        public TestPurposeService(ITestPurposeRepo _testPurposeRepo, EmployeeService employeeSercvice,
                                  TestService testSercvice)
        {
            this.testPurposeRepo = _testPurposeRepo;
            this.employeeSercvice = employeeSercvice;
            this.testSercvice = testSercvice;
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

        private PurposeAdminModel ConvertToPurposeAdminModel(TestPurpose purpose)
        {
            return new PurposeAdminModel
            {
                Id = purpose.Id,
                Employee = employeeSercvice.GetEmployeeModelById(purpose.IdEmployee),
                Test = testSercvice.GetTestGetModelById(purpose.IdTest),
                DatatimePurpose = purpose.DatatimePurpose.ToString()
            };
        }

        public void DeleteTestPurposeById(int id)
        {
            testPurposeRepo.DeleteTestPurposeById(id);
        }

        public void DeleteTestPurposeByEmployeeId(string testId, string employeeId)
        {
            List<TestPurposeDto> list = GetAllTestPurposeDtos();
            DeleteTestPurposeById(list.Find(x => x.IdTest.Equals(testId) && x.IdEmployee.Equals(employeeId)).Id.Value);
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

        public List<PurposeAdminModel> GetAllPurposeAdminModels()
        {
            List<PurposeAdminModel> list = new List<PurposeAdminModel>();
            testPurposeRepo.GetAllTestPurposes().ForEach(x => list.Add(ConvertToPurposeAdminModel(x)));
            return list;
        }

        public TestPurpose GetTestPurposeById(int id)
        {
            return testPurposeRepo.GetTestPurposeById(id);
        }

        public TestPurposeDto GetTestPurposeDtoById(int id)
        {
            return ConvertToTestPurposeDto(testPurposeRepo.GetTestPurposeById(id));
        }

        public PurposeAdminModel GetTestPurposeAdminModelById(int id) 
        { 
            return ConvertToPurposeAdminModel(testPurposeRepo.GetTestPurposeById(id));
        }
        public void SaveTestPurpose(TestPurpose TestPurposeToSave)
        {
            testPurposeRepo.SaveTestPurpose(TestPurposeToSave);
        }

        public void SaveTestPurposeDto(TestPurposeDto TestPurposeDtoToSave)
        {
            testPurposeRepo.SaveTestPurpose(ConvertToTestPurpose(TestPurposeDtoToSave));
        }

        //logic

        public void SavePurposeBySubdivisionId(string testId, int subdivisionId, DateTime time)
        {
            List<EmployeeDto> employees = employeeSercvice.GetAllEmployeeDtos();
            foreach (EmployeeDto employee in employees)
            {
                if (employee.IdSubdivision == subdivisionId)
                {
                    testPurposeRepo.SaveTestPurpose(new TestPurpose
                    {
                        IdEmployee = employee.Id,
                        IdTest = testId,
                        DatatimePurpose = time
                    });
                }
            }
        }
    }
}
