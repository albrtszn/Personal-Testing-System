using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class SecondPartService
    {
        private ISecondPartRepo secondPartRepo;
        public SecondPartService(ISecondPartRepo _secondPartRepo)
        {
            this.secondPartRepo = _secondPartRepo;
        }

        private SecondPartDto ConvertToSecondPartDto(SecondPart secondPart)
        {
            return new SecondPartDto
            {
                IdSecondPart = secondPart.Id,
                IdFirstPart = secondPart.IdFirstPart,
                Text = secondPart.Text
            };
        }

        public void DeleteSecondPartById(int id)
        {
            secondPartRepo.DeleteSecondPartById(id);
        }

        public List<SecondPart> GetAllSecondParts()
        {
            return secondPartRepo.GetAllSecondParts();
        }

        public List<SecondPartDto> GetAllSecondPartDtos()
        {
            List<SecondPartDto> secondPartDtos = new List<SecondPartDto>();
            GetAllSecondParts().ForEach(x => secondPartDtos.Add(ConvertToSecondPartDto(x)));
            return secondPartDtos;
        }

        public SecondPartDto GetSecondPartDtoByFirstPartId(string id)
        {
            return GetAllSecondPartDtos().Find(x => x.IdFirstPart.Equals(id));
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
