using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class CompetenceService
    {
        private ICompetenceRepo competenceRepo;
        public CompetenceService(ICompetenceRepo _сompetenceRepo)
        {
            this.competenceRepo = _сompetenceRepo;
        }

        private Competence ConvertToCompetence(CompetenceDto subDto)
        {
            return new Competence
            {
                Id = subDto.Id.Value,
                Name = subDto.Name
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

        public void DeleteCompetenceById(int id)
        {
            competenceRepo.DeleteCompetenceById(id);
        }

        public List<Competence> GetAllCompetences()
        {
            return competenceRepo.GetAllCompetences();
        }

        public List<CompetenceDto> GetAllCompetenceDtos()
        {
            List<CompetenceDto> Competences = new List<CompetenceDto>();
            competenceRepo.GetAllCompetences().ForEach(x => Competences.Add(ConvertToCompetenceDto(x)));
            return Competences;
        }

        public Competence GetCompetenceById(int id)
        {
            return competenceRepo.GetCompetenceById(id);
        }

        public CompetenceDto GetCompetenceDtoById(int id)
        {
            return ConvertToCompetenceDto(competenceRepo.GetCompetenceById(id));
        }

        public void SaveCompetence(Competence CompetenceToSave)
        {
            competenceRepo.SaveCompetence(CompetenceToSave);
        }

        public void SaveCompetence(CompetenceDto CompetenceDtoToSave)
        {
            competenceRepo.SaveCompetence(ConvertToCompetence(CompetenceDtoToSave));
        }

        public void SaveCompetence(AddCompetenceModel CompetenceDtoToSave)
        {
            competenceRepo.SaveCompetence(new Competence { Name = CompetenceDtoToSave.Name });
        }
    }
}
