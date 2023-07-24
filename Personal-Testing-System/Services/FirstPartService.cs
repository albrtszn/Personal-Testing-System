using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class FirstPartService
    {
        private IFirstPartRepo firstPartRepo;
        public FirstPartService(IFirstPartRepo _firstPartRepo)
        {
            this.firstPartRepo = _firstPartRepo;
        }

        private FirstPartDto ConvertToFirstPartDto(FirstPart firstPart)
        {
            return new FirstPartDto
            {
                Id = firstPart.Id,
                Text = firstPart.Text,
                IdQuestion = firstPart.IdQuestion
            };
        }

        public void DeleteFirstPartById(string id)
        {
            firstPartRepo.DeleteFirstPartById(id);
        }

        public List<FirstPart> GetAllFirstParts()
        {
            return firstPartRepo.GetAllFirstParts();
        }

        public List<FirstPartDto> GetFirstPartDtos()
        {
            List<FirstPartDto> firstParts = new List<FirstPartDto>();
            GetAllFirstParts().ForEach(x => firstParts.Add(ConvertToFirstPartDto(x)));
            return firstParts;
        }

        public List<FirstPartDto> GetAllFirstPartDtosByQuestionId(string id)
        {
            return GetFirstPartDtos().Where(x => x.IdQuestion == id).ToList();
        }

        public List<FirstSecondPartDto> GetFirstSecondPartDto(string id)
        {
            List<FirstSecondPartDto> firstSecondPartDtoList = new List<FirstSecondPartDto>();
            GetAllFirstParts().Where(x => x.IdQuestion.Equals(id)).ToList()
                .ForEach(x => firstSecondPartDtoList.Add(new FirstSecondPartDto
                {

                }));
            return firstSecondPartDtoList;
        }

        public FirstPart GetFirstPartById(string id)
        {
            return firstPartRepo.GetFirstPartById(id);
        }

        public void SaveFirstPart(FirstPart FirstPartToSave)
        {
            firstPartRepo.SaveFirstPArt(FirstPartToSave);
        }
    }
}
