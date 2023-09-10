using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class EmployeeMatchingService
    {
        private IEmployeeMatchingRepo EmployeeMatchingRepo;
        public EmployeeMatchingService(IEmployeeMatchingRepo _employeeMatchingRepo)
        {
            this.EmployeeMatchingRepo = _employeeMatchingRepo;
        }

        public EmployeeMatching ConvertToEmployeeMatching(EmployeeMatchingDto dto)
        {
            return new EmployeeMatching
            {
                Id = dto.Id,
                IdResult = dto.IdResult,
                IdFirstPart = dto.IdFirstPart,
                IdSecondPart = dto.IdSecondPart
            };
        }

        public EmployeeMatchingDto ConvertToEmployeeMatchingDto(EmployeeMatching match)
        {
            return new EmployeeMatchingDto
            {
                Id = match.Id,
                IdResult = match.IdResult,
                IdFirstPart = match.IdFirstPart,
                IdSecondPart = match.IdSecondPart
            };
        }

        public async Task<bool> DeleteEmployeeMatchingById(int id)
        {
            return await EmployeeMatchingRepo.DeleteEmployeeMatchingById(id);
        }

        public async Task<bool> DeleteEmployeeMatchingByResult(string idResult)
        {
            List<EmployeeMatching> list = await GetAllEmployeeMatchings();
            foreach (EmployeeMatching match in list)
            {
                if (match.IdResult.Equals(idResult))
                {
                    await DeleteEmployeeMatchingById(match.Id);
                }
            }
            return true;
        }

        public async Task<List<EmployeeMatching>> GetAllEmployeeMatchings()
        {
            return await EmployeeMatchingRepo.GetAllEmployeeMatchings();
        }

        public async Task<List<EmployeeMatchingDto>> GetAllEmployeeMatchingDtos()
        {
            List<EmployeeMatchingDto> EmployeeMatchings = new List<EmployeeMatchingDto>();
            List<EmployeeMatching> list = await EmployeeMatchingRepo.GetAllEmployeeMatchings();
            list.ForEach(x => EmployeeMatchings.Add(ConvertToEmployeeMatchingDto(x)));
            return EmployeeMatchings;
        }

        public async Task<EmployeeMatching> GetEmployeeMatchingById(int id)
        {
            return await EmployeeMatchingRepo.GetEmployeeMatchingById(id);
        }

        public async Task<EmployeeMatchingDto> GetEmployeeMatchingDtoById(int id)
        {
            return ConvertToEmployeeMatchingDto(await EmployeeMatchingRepo.GetEmployeeMatchingById(id));
        }

        public async Task<bool> SaveEmployeeMatching(EmployeeMatching EmployeeMatchingToSave)
        {
            return await EmployeeMatchingRepo.SaveEmployeeMatching(EmployeeMatchingToSave);
        }
    }
}
