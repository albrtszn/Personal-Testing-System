using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class FirstPartService
    {
        private IFirstPartRepo FirstPartRepo;
        public FirstPartService(IFirstPartRepo _firstPartRepo)
        {
            this.FirstPartRepo = _firstPartRepo;
        }

        private FirstPartDto ConvertToFirstPartDto(FirstPart firstPart)
        {
            return new FirstPartDto
            {
                IdFirstPart = firstPart.Id,
                Text = firstPart.Text,
                IdQuestion = firstPart.IdQuestion
            };
        }

        public async Task<bool> DeleteFirstPartById(string id)
        {
            return await FirstPartRepo.DeleteFirstPartById(id);
        }

        public async Task<bool> DeleteFirstPartsByQuestion(string idQuestion)
        {
            List<FirstPart> list = (await GetAllFirstParts()).Where(x => x.IdQuestion.Equals(idQuestion)).ToList();
            foreach (FirstPart firstPart in list)
            {
                await DeleteFirstPartById(firstPart.Id);
            }
            return true;
        }

        public async Task<List<FirstPart>> GetAllFirstParts()
        {
            return await FirstPartRepo.GetAllFirstParts();
        }

        public async Task<List<FirstPartDto>> GetFirstPartDtos()
        {
            List<FirstPartDto> FirstParts = new List<FirstPartDto>();
            List<FirstPart> list = await FirstPartRepo.GetAllFirstParts();
            list.ForEach(x => FirstParts.Add(ConvertToFirstPartDto(x)));
            return FirstParts;
        }

        public async Task<List<FirstPartDto>> GetAllFirstPartDtosByQuestionId(string id)
        {
            return (await GetFirstPartDtos()).Where(x => x.IdQuestion.Equals(id)).ToList();
        }

        public async Task<FirstPart> GetFirstPartById(string id)
        {
            return await FirstPartRepo.GetFirstPartById(id);
        }

        public async Task<bool> SaveFirstPart(FirstPart FirstPartToSave)
        {
            return await FirstPartRepo.SaveFirstPart(FirstPartToSave);
        }
    }
}
