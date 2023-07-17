using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class EmployeeMatchingService
    {
        private IEmployeeMatchingRepo employeeMatchingRepo;
        public EmployeeMatchingService(IEmployeeMatchingRepo _employeeMatchingRepo)
        {
            this.employeeMatchingRepo = _employeeMatchingRepo;
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

        public void DeleteEmployeeMatchingById(int id)
        {
            employeeMatchingRepo.DeleteEmployeeMatchingById(id);
        }

        public void DeleteEmployeeMatchingByResult(string idResult)
        {
            List<EmployeeMatching> list = GetAllEmployeeMatchings();
            foreach (EmployeeMatching match in list)
            {
                if (match.IdResult.Equals(idResult))
                {
                    DeleteEmployeeMatchingById(match.Id);
                }
            }
        }

        public List<EmployeeMatching> GetAllEmployeeMatchings()
        {
            return employeeMatchingRepo.GetAllEmployeeMatchings();
        }

        public List<EmployeeMatchingDto> GetAllEmployeeMatchingDtos()
        {
            List<EmployeeMatchingDto> list = new List<EmployeeMatchingDto>();
            GetAllEmployeeMatchings().ForEach(x => list.Add(ConvertToEmployeeMatchingDto(x)));
            return list;
        }

        public EmployeeMatching GetEmployeeMatchingById(int id)
        {
            return employeeMatchingRepo.GetEmployeeMatchingById(id);
        }

        public EmployeeMatchingDto GetEmployeeMatchingDtoById(int id)
        {
            return ConvertToEmployeeMatchingDto(employeeMatchingRepo.GetEmployeeMatchingById(id));
        }

        public void SaveEmployeeMatching(EmployeeMatching EmployeeMatchingToSave)
        {
            employeeMatchingRepo.SaveEmployeeMatching(EmployeeMatchingToSave);
        }
    }
}
