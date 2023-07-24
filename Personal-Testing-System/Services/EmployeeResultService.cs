using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class EmployeeResultService
    {
        private IEmployeeResultRepo EmployeeResultRepo;
        private ResultService resultService;
        private EmployeeService employeeService;
        public EmployeeResultService(IEmployeeResultRepo _EmployeeResultRepo, ResultService _resultService, 
                                     EmployeeService _employeeService)
        {
            this.EmployeeResultRepo = _EmployeeResultRepo;
            this.resultService = _resultService;
            this.employeeService = _employeeService;
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
                Id = EmployeeResultDto.Id.Value,
                ScoreFrom = EmployeeResultDto.ScoreFrom.Value,
                ScoreTo = EmployeeResultDto.ScoreTo.Value,
                IdEmployee = EmployeeResultDto.IdEmployee,
                IdResult = EmployeeResultDto.IdResult
            };
        }
        private EmployeeResultModel ConvertToEmployeeResultModel(EmployeeResult EmployeeResult)
        {
            return new EmployeeResultModel
            {
                Id = EmployeeResult.Id,
                ScoreFrom = EmployeeResult.ScoreFrom,
                ScoreTo = EmployeeResult.ScoreTo,
                Employee = employeeService.GetEmployeeModelById(EmployeeResult.IdEmployee),
                Result = resultService.GetResultDtoById(EmployeeResult.IdResult)
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
        public List<EmployeeResultModel> GetAllEmployeeResultModels()
        {
            List<EmployeeResultModel> list = new List<EmployeeResultModel>();
            EmployeeResultRepo.GetAllEmployeeResults().ForEach(x => list.Add(ConvertToEmployeeResultModel(x)));
            return list;
        }

        public List<EmployeeResultDto> GetEmployeeResultDtosByEmployeeId(int id)
        {
            return GetAllEmployeeResultDtos().Where(x => x.IdEmployee.Equals(id)).ToList();
        }

        public EmployeeResult GetEmployeeResultById(int id)
        {
            return EmployeeResultRepo.GetEmployeeResultById(id);
        }

        public EmployeeResultModel GetEmployeeResultModelById(int id)
        {
            return ConvertToEmployeeResultModel(EmployeeResultRepo.GetEmployeeResultById(id));
        }

        public void SaveEmployeeResult(EmployeeResult EmployeeResultToSave)
        {
            EmployeeResultRepo.SaveEmployeeResult(EmployeeResultToSave);
        }
    }
}