using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class TestPurposeService
    {
        private ITestPurposeRepo TestPurposeRepo;
        private EmployeeService employeeService;
        private TestService testService;
        public TestPurposeService(ITestPurposeRepo _testPurposeRepo, EmployeeService employeeService,
                                  TestService testService)
        {
            this.TestPurposeRepo = _testPurposeRepo;
            this.employeeService = employeeService;
            this.testService = testService;
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

        private async Task<PurposeAdminModel> ConvertToPurposeTestPurposeModel(TestPurpose purpose)
        {
            return new PurposeAdminModel
            {
                Id = purpose.Id,
                Employee = await employeeService.GetEmployeeModelById(purpose.IdEmployee),
                Test = await testService.GetTestGetModelById(purpose.IdTest),
                DatatimePurpose = purpose.DatatimePurpose.ToString()
            };
        }

        public async Task<bool> DeleteTestPurposeById(int id)
        {
            return await TestPurposeRepo.DeleteTestPurposeById(id);
        }

        public async Task<bool> DeleteTestPurposeByEmployeeId(string testId, string employeeId)
        {
            List<TestPurposeDto> list = await GetAllTestPurposeDtos();
            await DeleteTestPurposeById(list.Find(x => x.IdTest.Equals(testId) && x.IdEmployee.Equals(employeeId)).Id.Value);
            return true;
        }

        public async Task<TestPurpose?> GetTestPurposeByEmployeeTestId(string testId, string employeeId)
        {
            List<TestPurpose> list = await GetAllTestPurposes();
            if (list == null || list.Count == 0) return null;
            return (list.Find(x => x.IdTest.Equals(testId) && x.IdEmployee.Equals(employeeId)));
        }

        public async Task<List<TestPurpose>> GetAllTestPurposes()
        {
            return await TestPurposeRepo.GetAllTestPurposes();
        }

        public async Task<List<TestPurposeDto>> GetAllTestPurposeDtos()
        {
            List<TestPurposeDto> TestPurposes = new List<TestPurposeDto>();
            List<TestPurpose> list = await TestPurposeRepo.GetAllTestPurposes();
            list.ForEach(x => TestPurposes.Add(ConvertToTestPurposeDto(x)));
            return TestPurposes;
        }

        public async Task<List<PurposeAdminModel>> GetAllPurposeAdminModels()
        {
            List<PurposeAdminModel> list = new List<PurposeAdminModel>();
            List<TestPurpose> purposes = await TestPurposeRepo.GetAllTestPurposes();
            foreach(TestPurpose purpose in purposes)
            {
                list.Add(await ConvertToPurposeTestPurposeModel(purpose));
            }
            return list;
        }

        public async Task<TestPurpose> GetTestPurposeById(int id)
        {
            return await TestPurposeRepo.GetTestPurposeById(id);
        }

        public async Task<TestPurposeDto> GetTestPurposeDtoById(int id)
        {
            return ConvertToTestPurposeDto(await TestPurposeRepo.GetTestPurposeById(id));
        }

        public async Task<PurposeAdminModel >GetTestPurposeTestPurposeModelById(int id) 
        { 
            return await ConvertToPurposeTestPurposeModel(await TestPurposeRepo.GetTestPurposeById(id));
        }
        public async Task<bool> SaveTestPurpose(TestPurpose TestPurposeToSave)
        {
            return await TestPurposeRepo.SaveTestPurpose(TestPurposeToSave);
        }

        public async Task<bool> SaveTestPurpose(TestPurposeDto TestPurposeDtoToSave)
        {
            return await TestPurposeRepo.SaveTestPurpose(ConvertToTestPurpose(TestPurposeDtoToSave));
        }

        public async Task<bool> SaveTestPurpose(AddTestPurposeModel purpose)
        {
            await TestPurposeRepo.SaveTestPurpose(new TestPurpose
            {
                IdEmployee = purpose.IdEmployee,
                IdTest = purpose.IdTest,
                DatatimePurpose = DateTime.Now
                //DatatimePurpose = DateTime.Parse(purpose.DatatimePurpose)
            });
            return true;
        }

        public async Task<bool> SaveTestPurpose(UpdateTestPurposeModel purpose)
        {
            await TestPurposeRepo.SaveTestPurpose(new TestPurpose
            {
                Id = purpose.Id.Value,
                IdEmployee = purpose.IdEmployee,
                IdTest = purpose.IdTest,
                DatatimePurpose = DateTime.Now
                //DatatimePurpose = DateTime.Parse(purpose.DatatimePurpose)
            });
            return true;
        }

        //logic

        public async Task<bool> SavePurposeBySubdivisionId(string testId, int subdivisionId, DateTime time)
        {
            List<EmployeeDto> employees = await employeeService.GetAllEmployeeDtos();
            foreach (EmployeeDto employee in employees)
            {
                if (employee.IdSubdivision == subdivisionId)
                {
                    if (await GetTestPurposeByEmployeeTestId(testId, employee.Id) == null)
                    {
                        await TestPurposeRepo.SaveTestPurpose(new TestPurpose
                        {
                            IdEmployee = employee.Id,
                            IdTest = testId,
                            DatatimePurpose = time
                        });
                    }
                }
            }
            return true;
        }
    }
}
