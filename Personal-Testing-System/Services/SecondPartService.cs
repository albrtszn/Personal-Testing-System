using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class SecondPartService
    {
        private ISecondPartRepo SecondPartRepo;
        public SecondPartService(ISecondPartRepo _secondPartRepo)
        {
            this.SecondPartRepo = _secondPartRepo;
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

        public async Task<bool> DeleteSecondPartById(int id)
        {
            return await SecondPartRepo.DeleteSecondPartById(id);
        }

        public async Task<List<SecondPart>> GetAllSecondParts()
        {
            return await SecondPartRepo.GetAllSecondParts();
        }

        public async Task<List<SecondPartDto>> GetAllSecondPartDtos()
        {
            List<SecondPartDto> SecondParts = new List<SecondPartDto>();
            List<SecondPart> list = await SecondPartRepo.GetAllSecondParts();
            list.ForEach(x => SecondParts.Add(ConvertToSecondPartDto(x)));
            return SecondParts;
        }

        public async Task<SecondPartDto> GetSecondPartDtoByFirstPartId(string id)
        {
            return (await GetAllSecondPartDtos()).Find(x => x.IdFirstPart.Equals(id));
        }

        public async Task<SecondPart> GetSecondPartById(int id)
        {
            return await SecondPartRepo.GetSecondPartById(id);
        }

        public async Task<bool> SaveSecondPart(SecondPart SecondPartToSave)
        {
            return await SecondPartRepo.SaveSecondPart(SecondPartToSave);
        }
    }
}
