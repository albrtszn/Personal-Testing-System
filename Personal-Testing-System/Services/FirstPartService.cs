using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class FirstPartService
    {
        private IFirstPartRepo firstPartRepo;
        public FirstPartService(IFirstPartRepo _firstPartRepo)
        {
            this.firstPartRepo = _firstPartRepo;
        }
        public void DeleteFirstPartById(int id)
        {
            firstPartRepo.DeleteFirstPartById(id);
        }

        public List<FirstPart> GetAllFirstParts()
        {
            return firstPartRepo.GetAllFirstParts();
        }

        public FirstPart GetFirstPartById(int id)
        {
            return firstPartRepo.GetFirstPartById(id);
        }

        public void SaveFirstPart(FirstPart FirstPartToSave)
        {
            firstPartRepo.SaveFirstPArt(FirstPartToSave);
        }
    }
}
