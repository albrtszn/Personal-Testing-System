using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class EmployeeResultSubcompetenceService
    {
        private IEmployeeResultSubcompetenceRepo EmployeeResultSubcompetenceRepo;
        public EmployeeResultSubcompetenceService(IEmployeeResultSubcompetenceRepo _EmployeeResultSubcompetenceRepo)
        {
            this.EmployeeResultSubcompetenceRepo = _EmployeeResultSubcompetenceRepo;
        }
        private EmployeeResultSubcompetenceDto ConvertToEmployeeResultSubcompetenceDto(ElployeeResultSubcompetence EmployeeResultSubcompetence)
        {
            return new EmployeeResultSubcompetenceDto
            {
                Id = EmployeeResultSubcompetence.Id,
                IdResult = EmployeeResultSubcompetence.IdResult,
                IdSubcompetence = EmployeeResultSubcompetence.IdSubcompetence,
                Result = EmployeeResultSubcompetence.Result
            };
        }
        private ElployeeResultSubcompetence ConvertToEmployeeResultSubcompetence(EmployeeResultSubcompetenceDto EmployeeResultSubcompetenceDto)
        {
            return new ElployeeResultSubcompetence
            {
                Id = EmployeeResultSubcompetenceDto.Id,
                IdResult = EmployeeResultSubcompetenceDto.IdResult,
                IdSubcompetence = EmployeeResultSubcompetenceDto.IdSubcompetence,
                Result = EmployeeResultSubcompetenceDto.Result
            };
        }
        public async Task<bool> DeleteEmployeeResultSubcompetenceById(int id)
        {
            return await EmployeeResultSubcompetenceRepo.DeleteEmployeeResultSubcompetenceById(id);
        }

        /*public async Task<bool> DeleteEmployeeResultSubcompetencesByQuestion(string idQuestion)
        {
            List<EmployeeResultSubcompetence> list = (await GetAllEmployeeResultSubcompetences()).Where(x => x.IdQuestion.Equals(idQuestion)).ToList();
            foreach (EmployeeResultSubcompetence EmployeeResultSubcompetence in list)
            {
                await DeleteEmployeeResultSubcompetenceById(EmployeeResultSubcompetence.Id);
            }
            return true;
        }*/

        public async Task<List<ElployeeResultSubcompetence>> GetAllEmployeeResultSubcompetences()
        {
            return await EmployeeResultSubcompetenceRepo.GetAllEmployeeResultSubcompetences();
        }

        public async Task<List<ElployeeResultSubcompetence>> GetEmployeeResultSubcompetencesByresultId(int id)
        {
            return (await GetAllEmployeeResultSubcompetences()).Where(x => x!= null && x.IdResult != null && x.IdResult.Equals(id)).ToList();
        }

        public async Task<List<EmployeeResultSubcompetenceDto>> GetAllEmployeeResultSubcompetenceDtos()
        {
            List<EmployeeResultSubcompetenceDto> EmployeeResultSubcompetences = new List<EmployeeResultSubcompetenceDto>();
            List<ElployeeResultSubcompetence> list = await EmployeeResultSubcompetenceRepo.GetAllEmployeeResultSubcompetences();
            list.ForEach(x => EmployeeResultSubcompetences.Add(ConvertToEmployeeResultSubcompetenceDto(x)));
            return EmployeeResultSubcompetences;
        }

        /*public async Task<List<EmployeeResultSubcompetenceDto>> GetEmployeeResultSubcompetenceDtosByQuestionId(string id)
        {
            return (await GetAllEmployeeResultSubcompetenceDtos()).Where(x => x.IdQuestion.Equals(id)).ToList();
        }*/

        public async Task<ElployeeResultSubcompetence> GetEmployeeResultSubcompetenceById(int id)
        {
            return await EmployeeResultSubcompetenceRepo.GetEmployeeResultSubcompetenceById(id);
        }

        public async Task<bool> SaveEmployeeResultSubcompetence(ElployeeResultSubcompetence EmployeeResultSubcompetenceToSave)
        {
            return await EmployeeResultSubcompetenceRepo.SaveEmployeeResultSubcompetence(EmployeeResultSubcompetenceToSave);
        }
    }
}
