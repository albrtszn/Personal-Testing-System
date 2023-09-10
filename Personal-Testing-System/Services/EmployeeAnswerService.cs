using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class EmployeeAnswerService
    {
        private IEmployeeAnswerRepo EmployeeAnswerRepo;
        public EmployeeAnswerService(IEmployeeAnswerRepo _employeeAnswerRepo)
        {
            this.EmployeeAnswerRepo = _employeeAnswerRepo;
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
        public async Task<bool> DeleteEmployeeAnswerById(int id)
        {
            return await EmployeeAnswerRepo.DeleteEmployeeAnswerById(id);
        }

        public async Task<bool> DeleteEmployeeAnswersByResult(string idResult)
        {
            List<EmployeeAnswerDto> list = await GetAllEmployeeAnswerDtos();
            foreach (EmployeeAnswerDto dto in list)
            {
                if (dto.IdResult.Equals(idResult))
                {
                    await DeleteEmployeeAnswerById(dto.Id);
                }
            }
            return true;
        }

        public async Task<List<EmployeeAnswer>> GetAllEmployeeAnswers()
        {
            return await EmployeeAnswerRepo.GetAllEmployeeAnswers();
        }

        public async Task<List<EmployeeAnswerDto>> GetAllEmployeeAnswerDtos()
        {
            List<EmployeeAnswerDto> EmployeeAnswers = new List<EmployeeAnswerDto>();
            List<EmployeeAnswer> list = await EmployeeAnswerRepo.GetAllEmployeeAnswers();
            list.ForEach(x => EmployeeAnswers.Add(ConvertToEmployeeAnswerDto(x)));
            return EmployeeAnswers;
        }

        public async Task<EmployeeAnswer> GetEmployeeAnswerById(int id)
        {
            return await EmployeeAnswerRepo.GetEmployeeAnswerById(id);
        }

        public async Task<EmployeeAnswerDto> GetEmployeeAnswerDtoById(int id)
        {
            return ConvertToEmployeeAnswerDto(await EmployeeAnswerRepo.GetEmployeeAnswerById(id));
        }

        public async Task<bool> SaveEmployeeAnswer(EmployeeAnswer EmployeeAnswerToSave)
        {
            return await EmployeeAnswerRepo.SaveEmployeeAnswer(EmployeeAnswerToSave);
        }
    }
}
