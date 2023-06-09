using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class SecondPartService
    {
        private ISecondPartRepo secondPartRepo;
        public SecondPartService(ISecondPartRepo _secondPartRepo)
        {
            this.secondPartRepo = _secondPartRepo;
        }
        public void DeleteSecondPartById(int id)
        {
            secondPartRepo.DeleteSecondPartById(id);
        }

        public List<SecondPart> GetAllSecondParts()
        {
            return secondPartRepo.GetAllSecondParts();
        }

        public SecondPart GetSecondPartById(int id)
        {
            return secondPartRepo.GetSecondPartById(id);
        }

        public void SaveSecondPart(SecondPart SecondPartToSave)
        {
            secondPartRepo.SaveSecondPart(SecondPartToSave);
        }
    }
}
