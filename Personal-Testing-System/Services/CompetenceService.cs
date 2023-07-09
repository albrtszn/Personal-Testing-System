using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class CompetenceService
    {
        private ICompetenceRepo competenceRepo;
        public CompetenceService(ICompetenceRepo _сompetenceRepo)
        {
            this.competenceRepo = _сompetenceRepo;
        }
        public void DeleteCompetenceById(int id)
        {
            competenceRepo.DeleteCompetenceById(id);
        }

        public List<Competence> GetAllCompetences()
        {
            return competenceRepo.GetAllCompetences();
        }

        public Competence GetCompetenceById(int id)
        {
            return competenceRepo.GetCompetenceById(id);
        }

        public void SaveCompetence(Competence CompetenceToSave)
        {
            competenceRepo.SaveCompetence(CompetenceToSave);
        }
    }
}
