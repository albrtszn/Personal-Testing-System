using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class SubsequenceService
    {
        private ISubsequenceRepo subsequenceRepo;
        public SubsequenceService(ISubsequenceRepo _subsequenceRepo)
        {
            this.subsequenceRepo = _subsequenceRepo;
        }

        private SubsequenceDto ConvertToSubsequenceDto(Subsequence subsequence)
        {
            return new SubsequenceDto
            {
                IdSubsequence = subsequence.Id,
                IdQuestion = subsequence.IdQuestion,
                Text = subsequence.Text,
                Number = subsequence.Number
            };
        }

        private Subsequence ConvertToSubsequence(SubsequenceDto subsequenceDto)
        {
            return new Subsequence
            {
                Id = subsequenceDto.IdSubsequence,
                IdQuestion = subsequenceDto.IdQuestion,
                Text = subsequenceDto.Text,
                Number = subsequenceDto.Number
            };
        }

        public void DeleteSubsequenceById(int id)
        {
            subsequenceRepo.DeleteSubsequenceById(id);
        }

        public List<Subsequence> GetAllSubsequences()
        {
            return subsequenceRepo.GetAllSubSequences();
        }

        public List<SubsequenceDto> GetAllSubsequenceDtos()
        {
            List<SubsequenceDto> subsequences =  new List<SubsequenceDto>();
            GetAllSubsequences().ForEach(x=> subsequences.Add(ConvertToSubsequenceDto(x)));
            return subsequences;
        }

        public List<SubsequenceDto> GetSubsequenceDtosByQuestionId(string id)
        {
            return GetAllSubsequenceDtos().Where(x => x.IdQuestion.Equals(id)).ToList();
        }

        public Subsequence GetSubsequenceById(int id)
        {
            return subsequenceRepo.GetSubsequenceById(id);
        }

        public void SaveSubsequence(Subsequence SubsequenceToSave)
        {
            subsequenceRepo.SaveSubsequence(SubsequenceToSave);
        }
    }
}
