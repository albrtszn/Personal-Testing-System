using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class SubcompetenceScoreService
    {
        private ISubcompetenceScoreRepo SubcompetenceScoreRepo;
        public SubcompetenceScoreService(ISubcompetenceScoreRepo _SubcompetenceScoreRepo)
        {
            this.SubcompetenceScoreRepo = _SubcompetenceScoreRepo;
        }
        private SubcompetenceScoreDto ConvertToSubcompetenceScoreDto(SubcompetenceScore SubcompetenceScore)
        {
            return new SubcompetenceScoreDto
            {
                Id = SubcompetenceScore.Id,
                MinValue = SubcompetenceScore.MinValue,
                MaxValue = SubcompetenceScore.MaxValue,
                Description = SubcompetenceScore.Description,
                IdSubcompetence = SubcompetenceScore.IdSubcompetence
            };
        }
        private SubcompetenceScore ConvertToSubcompetenceScore(SubcompetenceScoreDto SubcompetenceScoreDto)
        {
            return new SubcompetenceScore
            {
                Id = SubcompetenceScoreDto.Id.Value,
                MinValue = SubcompetenceScoreDto.MinValue,
                MaxValue = SubcompetenceScoreDto.MaxValue,
                Description = SubcompetenceScoreDto.Description,
                IdSubcompetence = SubcompetenceScoreDto.IdSubcompetence
            };
        }
        public async Task<bool> DeleteSubcompetenceScoreById(int id)
        {
            return await SubcompetenceScoreRepo.DeleteSubcompetenceScoreById(id);
        }

        public async Task<bool> DeleteSubcompetenceScoresBySubCompetence(int idSubCompetencene)
        {
            List<SubcompetenceScore> list = (await GetAllSubcompetenceScores()).Where(x => x.IdSubcompetence.Equals(idSubCompetencene)).ToList();
            foreach (SubcompetenceScore SubcompetenceScore in list)
            {
                await DeleteSubcompetenceScoreById(SubcompetenceScore.Id);
            }
            return true;
        }

        public async Task<List<SubcompetenceScore>> GetAllSubcompetenceScores()
        {
            return await SubcompetenceScoreRepo.GetAllSubcompetenceScores();
        }

        public async Task<List<SubcompetenceScore>> GetSubcompetenceScoresBySubCompetence(int idSubCompetence)
        {
            return (await GetAllSubcompetenceScores()).Where(x => x.IdSubcompetence.Equals(idSubCompetence)).ToList();
        }

        public async Task<List<SubcompetenceScoreDto>> GetAllSubcompetenceScoreDtos()
        {
            List<SubcompetenceScoreDto> SubcompetenceScores = new List<SubcompetenceScoreDto>();
            List<SubcompetenceScore> list = await SubcompetenceScoreRepo.GetAllSubcompetenceScores();
            list.ForEach(x => SubcompetenceScores.Add(ConvertToSubcompetenceScoreDto(x)));
            return SubcompetenceScores;
        }

        public async Task<List<SubcompetenceScoreDto>> GetSubcompetenceScoreDtosBySubCompetence(int idSubCompetence)
        {
            return (await GetAllSubcompetenceScoreDtos()).Where(x => x.IdSubcompetence.Equals(idSubCompetence)).ToList();
        }


        public async Task<SubcompetenceScore> GetSubcompetenceScoreById(int id)
        {
            return await SubcompetenceScoreRepo.GetSubcompetenceScoreById(id);
        }

        public async Task<SubcompetenceScoreDto> GetSubcompetenceScoreDtoById(int id)
        {
            return ConvertToSubcompetenceScoreDto(await SubcompetenceScoreRepo.GetSubcompetenceScoreById(id));
        }

        public async Task<bool> SaveSubcompetenceScore(SubcompetenceScore SubcompetenceScoreToSave)
        {
            return await SubcompetenceScoreRepo.SaveSubcompetenceScore(SubcompetenceScoreToSave);
        }          
        public async Task<bool> SaveSubcompetenceScore(SubcompetenceScoreDto SubcompetenceScoreToSave)
        {
            return await SubcompetenceScoreRepo.SaveSubcompetenceScore(ConvertToSubcompetenceScore(SubcompetenceScoreToSave));
        }       
        public async Task<bool> SaveSubcompetenceScore(AddSubcompetenceScoreModel SubcompetenceScoreToSave)
        {
            return await SubcompetenceScoreRepo.SaveSubcompetenceScore(new SubcompetenceScore()
            {
                MinValue = SubcompetenceScoreToSave.MinValue,
                MaxValue = SubcompetenceScoreToSave.MaxValue,
                Description = SubcompetenceScoreToSave.Description,
                IdSubcompetence = SubcompetenceScoreToSave.IdSubcompetence
            });
        }
    }
}
