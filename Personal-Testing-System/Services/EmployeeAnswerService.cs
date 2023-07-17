using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class EmployeeAnswerService
    {
        private IEmployeeAnswerRepo employeeAnswerRepo;
        public EmployeeAnswerService(IEmployeeAnswerRepo _employeeAnswerRepo)
        {
            this.employeeAnswerRepo = _employeeAnswerRepo;
        }
        private EmployeeAnswer ConvertToEmployeeAnswer(EmployeeAnswerDto dto)
        {
            return new EmployeeAnswer
            {
                Id = dto.Id,
                IdAnswer = dto.IdAnswer,
                IdResult = dto.IdResult
            };
        }
        private EmployeeAnswerDto ConvertToEmployeeAnswerDto(EmployeeAnswer answer)
        {
            return new EmployeeAnswerDto
            {
                Id = answer.Id,
                IdAnswer = answer.IdAnswer,
                IdResult = answer.IdResult
            };
        }
        public void DeleteEmployeeAnswerById(int id)
        {
            employeeAnswerRepo.DeleteEmployeeAnswerById(id);
        }

        public void DeleteEmployeeAnswersByResult(string idResult)
        {
            List<EmployeeAnswerDto> list = GetAllEmployeeAnswerDtos();
            foreach (EmployeeAnswerDto dto in list)
            {
                if (dto.IdResult.Equals(idResult))
                {
                    DeleteEmployeeAnswerById(dto.Id);
                }
            }
        }

        public List<EmployeeAnswer> GetAllEmployeeAnswers()
        {
            return employeeAnswerRepo.GetAllEmployeeAnswers();
        }

        public List<EmployeeAnswerDto> GetAllEmployeeAnswerDtos()
        {
            List<EmployeeAnswerDto> list = new List<EmployeeAnswerDto>();
            GetAllEmployeeAnswers().ForEach(x => list.Add(ConvertToEmployeeAnswerDto(x)));
            return list;
        }

        public EmployeeAnswer GetEmployeeAnswerById(int id)
        {
            return employeeAnswerRepo.GetEmployeeAnswerById(id);
        }

        public EmployeeAnswerDto GetEmployeeAnswerDtoById(int id)
        {
            return ConvertToEmployeeAnswerDto(employeeAnswerRepo.GetEmployeeAnswerById(id));
        }

        public void SaveEmployeeAnswer(EmployeeAnswer EmployeeAnswerToSave)
        {
            employeeAnswerRepo.SaveEmployeeAnswer(EmployeeAnswerToSave);
        }
    }
}
