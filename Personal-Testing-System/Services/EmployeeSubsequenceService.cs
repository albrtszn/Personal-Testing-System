using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class EmployeeSubsequenceService
    {
        private IEmployeeSubsequenceRepo EmployeeSubsequenceRepo;
        public EmployeeSubsequenceService(IEmployeeSubsequenceRepo _employeeSubsequenceRepo)
        {
            this.EmployeeSubsequenceRepo = _employeeSubsequenceRepo;
        }

        private EmployeeSubsequence ConvertToEmployeeSubsequence(EmployeeSubsequenceDto dto)
        {
            return new EmployeeSubsequence
            {
                Id = dto.Id,
                IdSubsequence = dto.IdSubsequence,
                IdResult = dto.IdResult
            };
        }

        private EmployeeSubsequenceDto ConvertToEmployeeSubsequenceDto(EmployeeSubsequence sub)
        {
            return new EmployeeSubsequenceDto
            {
                Id = sub.Id,
                IdSubsequence = sub.IdSubsequence,
                IdResult = sub.IdResult
            };
        }

        public async Task<bool> DeleteEmployeeSubsequenceById(int id)
        {
            return await EmployeeSubsequenceRepo.DeleteEmployeeSubsequenceById(id);
        }

        public async Task<bool> DeleteEmployeeSubsequenceByResult(string idResult)
        {
            List<EmployeeSubsequence> list = await GetAllEmployeeSubsequences();
            foreach (EmployeeSubsequence sub in list)
            {
                if (sub.IdResult.Equals(idResult))
                {
                    await DeleteEmployeeSubsequenceById(sub.Id);
                }
            }
            return true;
        }

        public async Task<List<EmployeeSubsequence>> GetAllEmployeeSubsequences()
        {
            return await EmployeeSubsequenceRepo.GetAllEmployeeSubsequences();
        }

        public async Task<List<EmployeeSubsequenceDto>> GetAllEmployeeSubsequenceDtos()
        {
            List<EmployeeSubsequenceDto> EmployeeSubsequences = new List<EmployeeSubsequenceDto>();
            List<EmployeeSubsequence> list = await EmployeeSubsequenceRepo.GetAllEmployeeSubsequences();
            list.ForEach(x => EmployeeSubsequences.Add(ConvertToEmployeeSubsequenceDto(x)));
            return EmployeeSubsequences;
        }

        public async Task<EmployeeSubsequence> GetEmployeeSubsequenceById(int id)
        {
            return await EmployeeSubsequenceRepo.GetEmployeeSubsequenceById(id);
        }

        public async Task<EmployeeSubsequenceDto> GetEmployeeSubsequenceDtoById(int id)
        {
            return ConvertToEmployeeSubsequenceDto(await EmployeeSubsequenceRepo.GetEmployeeSubsequenceById(id));
        }

        public async Task<bool> SaveEmployeeSubsequence(EmployeeSubsequence EmployeeSubsequenceToSave)
        {
            return await EmployeeSubsequenceRepo.SaveEmployeeSubsequence(EmployeeSubsequenceToSave);
        }
    }
}
