using DataBase.Repository.Models;
using DataBase.Repository;
using CRUD.interfaces;

namespace Personal_Testing_System.Services
{
    public class SubdivisionService
    {
        private ISubdivisionRepo subdivisionRepo;
        public SubdivisionService(ISubdivisionRepo _subdivisionRepo)
        {
            this.subdivisionRepo = _subdivisionRepo;
        }
        public void DeleteSubdivisionById(int id)
        {
            subdivisionRepo.DeleteSubdivisionById(id);
        }

        public List<Subdivision> GetAllSubdivisions()
        {
            return subdivisionRepo.GetAllSubdivisions();
        }

        public Subdivision GetSubdivisionById(int id)
        {
            return subdivisionRepo.GetSubdivisionById(id);
        }

        public void SaveSubdivision(Subdivision SubdivisionToSave)
        {
            subdivisionRepo.SaveSubdivision(SubdivisionToSave);
        }
    }
}
