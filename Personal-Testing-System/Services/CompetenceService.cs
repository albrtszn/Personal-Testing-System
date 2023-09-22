using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class CompetenceService
    {
        private ICompetenceRepo CompetenceRepo;
        public CompetenceService(ICompetenceRepo _сompetenceRepo)
        {
            this.CompetenceRepo = _сompetenceRepo;
        }

        private Competence ConvertToCompetence(CompetenceDto subDto)
        {
            return new Competence
            {
                Id = subDto.Id.Value,
                Name = subDto.Name,
            };
        }

        private CompetenceDto ConvertToCompetenceDto(Competence sub)
        {
            return new CompetenceDto
            {
                Id = sub.Id,
                Name = sub.Name
            };
        }

        public async Task<bool> DeleteCompetenceById(int id)
        {
            return await CompetenceRepo.DeleteCompetenceById(id);
        }

        public async Task<List<Competence>> GetAllCompetences()
        {
            return await CompetenceRepo.GetAllCompetences();
        }

        public async Task<List<CompetenceDto>> GetAllCompetenceDtos()
        {
            List<CompetenceDto> Competences = new List<CompetenceDto>();
            List<Competence> list = await CompetenceRepo.GetAllCompetences();
            list.ForEach(x => Competences.Add(ConvertToCompetenceDto(x)));
            return Competences;
        }

        public async Task<Competence> GetCompetenceById(int id)
        {
            return await CompetenceRepo.GetCompetenceById(id);
        }

        public async Task<CompetenceDto> GetCompetenceDtoById(int id)
        {
            return ConvertToCompetenceDto(await CompetenceRepo.GetCompetenceById(id));
        }

        public async Task<bool> SaveCompetence(Competence CompetenceToSave)
        {
            return await CompetenceRepo.SaveCompetence(CompetenceToSave);
        }

        public async Task<bool> SaveCompetence(CompetenceDto CompetenceDtoToSave)
        {
            return await CompetenceRepo.SaveCompetence(ConvertToCompetence(CompetenceDtoToSave));
        }

        public async Task<bool> SaveCompetence(AddCompetenceModel CompetenceDtoToSave)
        {
            await CompetenceRepo.SaveCompetence(new Competence { Name = CompetenceDtoToSave.Name });
            return true;
        }
    }
}
