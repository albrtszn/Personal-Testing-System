using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

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
                Id = EmployeeResultDto.Id.Value,
                ScoreFrom = EmployeeResultDto.ScoreFrom.Value,
                ScoreTo = EmployeeResultDto.ScoreTo.Value,
                IdEmployee = EmployeeResultDto.IdEmployee,
                IdResult = EmployeeResultDto.IdResult
            };
        }


        public async Task<bool> DeleteEmployeeResultById(int id)
        {
            return await EmployeeResultRepo.DeleteEmployeeResultById(id);
        }

        public async Task<List<EmployeeResult>> GetAllEmployeeResults()
        {
            return await EmployeeResultRepo.GetAllEmployeeResults();
        }

        public async Task<List<EmployeeResultDto>> GetAllEmployeeResultDtos()
        {
            List<EmployeeResultDto> EmployeeResults = new List<EmployeeResultDto>();
            List<EmployeeResult> list = await EmployeeResultRepo.GetAllEmployeeResults();
            list.ForEach(x => EmployeeResults.Add(ConvertToEmployeeResultDto(x)));
            return EmployeeResults;
        }


        public async Task<List<EmployeeResultDto>> GetEmployeeResultDtosByEmployeeId(int id)
        {
            return (await GetAllEmployeeResultDtos()).Where(x => x.IdEmployee.Equals(id)).ToList();
        }

        public async Task<EmployeeResult> GetEmployeeResultById(int id)
        {
            return await EmployeeResultRepo.GetEmployeeResultById(id);
        }
        public async Task<bool> SaveEmployeeResult(EmployeeResult EmployeeResultDtoToSave)
        {
            return await EmployeeResultRepo.SaveEmployeeResult(EmployeeResultDtoToSave);
        }
        public async Task<bool> SaveEmployeeResult(EmployeeResultDto EmployeeResultDtoToSave)
        {
            return await EmployeeResultRepo.SaveEmployeeResult(ConvertToEmployeeResult(EmployeeResultDtoToSave));
        }
    }
}