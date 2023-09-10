using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class SubsequenceService
    {
        private ISubsequenceRepo SubsequenceRepo;
        public SubsequenceService(ISubsequenceRepo _subsequenceRepo)
        {
            this.SubsequenceRepo = _subsequenceRepo;
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

        public async Task<bool> DeleteSubsequenceById(int id)
        {
            return await SubsequenceRepo.DeleteSubsequenceById(id);
        }

        public async Task<bool> DeleteSubsequencesByQuestion(string idQuestion)
        {
            var list = (await GetAllSubsequences()).Where(x => x.IdQuestion.Equals(idQuestion)).ToList();
            if (list.Count > 0)
            {
                foreach (var subsequence in list)
                {
                    await DeleteSubsequenceById(subsequence.Id);
                }
            }
            return true;
        }

        public async Task<List<Subsequence>> GetAllSubsequences()
        {
            return await SubsequenceRepo.GetAllSubsequences();
        }

        public async Task<List<SubsequenceDto>> GetAllSubsequenceDtos()
        {
            List<SubsequenceDto> Subsequences = new List<SubsequenceDto>();
            List<Subsequence> list = await SubsequenceRepo.GetAllSubsequences();
            list.ForEach(x => Subsequences.Add(ConvertToSubsequenceDto(x)));
            return Subsequences;
        }

        public async Task<List<SubsequenceDto>> GetSubsequenceDtosByQuestionId(string id)
        {
            return (await GetAllSubsequenceDtos()).Where(x => x.IdQuestion.Equals(id)).ToList();
        }

        public async Task<Subsequence> GetSubsequenceById(int id)
        {
            return await SubsequenceRepo.GetSubsequenceById(id);
        }

        public async Task<bool> SaveSubsequence(Subsequence SubsequenceToSave)
        {
            return await SubsequenceRepo.SaveSubsequence(SubsequenceToSave);
        }
    }
}
