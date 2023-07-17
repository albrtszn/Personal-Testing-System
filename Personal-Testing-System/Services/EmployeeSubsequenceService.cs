using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class EmployeeSubsequenceService
    {
        private IEmployeeSubsequenceRepo employeeSubsequenceRepo;
        public EmployeeSubsequenceService(IEmployeeSubsequenceRepo _employeeSubsequenceRepo)
        {
            this.employeeSubsequenceRepo = _employeeSubsequenceRepo;
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

        public void DeleteEmployeeSubsequenceById(int id)
        {
            employeeSubsequenceRepo.DeleteEmployeeSubsequenceById(id);
        }

        public void DeleteEmployeeSubsequenceByResult(string idResult)
        {
            List<EmployeeSubsequence> list = GetAllEmployeeSubsequences();
            foreach (EmployeeSubsequence sub in list)
            {
                if (sub.IdResult.Equals(idResult))
                {
                    DeleteEmployeeSubsequenceById(sub.Id);
                }
            }
        }

        public List<EmployeeSubsequence> GetAllEmployeeSubsequences()
        {
            return employeeSubsequenceRepo.GetAllEmployeeSubsequences();
        }

        public List<EmployeeSubsequenceDto> GetAllEmployeeSubsequenceDtos()
        {
            List<EmployeeSubsequenceDto> list = new List<EmployeeSubsequenceDto>();
            GetAllEmployeeSubsequences().ForEach(x => list.Add(ConvertToEmployeeSubsequenceDto(x)));
            return list;
        }

        public EmployeeSubsequence GetEmployeeSubsequenceById(int id)
        {
            return employeeSubsequenceRepo.GetEmployeeSubsequenceById(id);
        }

        public EmployeeSubsequenceDto GetEmployeeSubsequenceDtoById(int id)
        {
            return ConvertToEmployeeSubsequenceDto(employeeSubsequenceRepo.GetEmployeeSubsequenceById(id));
        }

        public void SaveEmployeeSubsequence(EmployeeSubsequence EmployeeSubsequenceToSave)
        {
            employeeSubsequenceRepo.SaveEmployeeSubsequence(EmployeeSubsequenceToSave);
        }
    }
}
