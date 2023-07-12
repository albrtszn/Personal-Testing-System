using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class EmployeeResultService
    {
        private IEmployeeResultRepo EmployeeResultRepo;
        public EmployeeResultService(IEmployeeResultRepo _EmployeeResultRepo)
        {
            this.EmployeeResultRepo = _EmployeeResultRepo;
        }
        private EmployeeResultDto ConvertToEmployeeResultDto(EmployeeResult EmployeeResult)
        {
            return new EmployeeResultDto
            {
                Id = EmployeeResult.Id,
                IdEmployee = EmployeeResult.IdEmployee,
                IdResult = EmployeeResult.IdResult
            };
        }
        private EmployeeResult ConvertToEmployeeResult(EmployeeResultDto EmployeeResultDto)
        {
            return new EmployeeResult
            {
                Id = EmployeeResultDto.Id,
                IdEmployee = EmployeeResultDto.IdEmployee,
                IdResult = EmployeeResultDto.IdResult
            };
        }
        public void DeleteEmployeeResultById(int id)
        {
            EmployeeResultRepo.DeleteEmployeeResultById(id);
        }

        public List<EmployeeResult> GetAllEmployeeResults()
        {
            return EmployeeResultRepo.GetAllEmployeeResults();
        }

        public List<EmployeeResultDto> GetAllEmployeeResultDtos()
        {
            List<EmployeeResultDto> EmployeeResults = new List<EmployeeResultDto>();
            EmployeeResultRepo.GetAllEmployeeResults().ForEach(x => EmployeeResults.Add(ConvertToEmployeeResultDto(x)));
            return EmployeeResults;
        }

        public List<EmployeeResultDto> GetEmployeeResultDtosByEmployeeId(string id)
        {
            return GetAllEmployeeResultDtos().Where(x => x.IdEmployee.Equals(id)).ToList();
        }

        public EmployeeResult GetEmployeeResultById(string id)
        {
            return EmployeeResultRepo.GetEmployeeResultById(id);
        }

        public void SaveEmployeeResult(EmployeeResult EmployeeResultToSave)
        {
            EmployeeResultRepo.SaveEmployeeResult(EmployeeResultToSave);
        }
    }
}
