using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class CompetenceScoreService
    {
        private ICompetenceScoreRepo CompetenceScoreRepo;
        public CompetenceScoreService(ICompetenceScoreRepo _CompetenceScoreRepo)
        {
            this.CompetenceScoreRepo = _CompetenceScoreRepo;
        }
        private CompetenceScoreDto ConvertToCompetenceScoreDto(CompetenceScore CompetenceScore)
        {
            return new CompetenceScoreDto
            {
                Id = CompetenceScore.Id,
                IdCompetence = CompetenceScore.IdCompetence,
                IdGroup = CompetenceScore.IdGroup,
                Name = CompetenceScore.Name,
                NumberPoints = CompetenceScore.NumnerPoints,
                Description = CompetenceScore.Description
            };
        }
        private CompetenceScore ConvertToCompetenceScore(CompetenceScoreDto CompetenceScoreDto)
        {
            return new CompetenceScore
            {
                Id = CompetenceScoreDto.Id.Value,
                IdCompetence = CompetenceScoreDto.IdCompetence.Value,
                IdGroup = CompetenceScoreDto.IdGroup.Value,
                Name = CompetenceScoreDto.Name,
                NumnerPoints = CompetenceScoreDto.NumberPoints.Value,
                Description = CompetenceScoreDto.Description
            };
        }
        public async Task<bool> DeleteCompetenceScoreById(int id)
        {
            return await CompetenceScoreRepo.DeleteCompetenceScoreById(id);
        }

        public async Task<bool> DeleteCompetenceScoresByIdGroup(string idGroup) { 
            List<CompetenceScore> list = (await GetAllCompetenceScores()).Where(x => x.IdGroup != null && x.IdGroup.Equals(idGroup)).ToList();
            foreach (CompetenceScore CompetenceScore in list)
            {
                await DeleteCompetenceScoreById(CompetenceScore.Id);
            }
            return true;
        }

        public async Task<List<CompetenceScore>> GetAllCompetenceScores()
        {
            return await CompetenceScoreRepo.GetAllCompetenceScores();
        }

        public async Task<List<CompetenceScore>> GetCompetenceScoresByGroupId(string id)
        {
            return (await GetAllCompetenceScores()).Where(x => x.IdGroup != null && x.IdGroup.Equals(id)).ToList();
        }

        public async Task<CompetenceScoreDto?> GetCompetenceScoreDtoByCompetenceAndGroupId(int idCompetence, int idGroup)
        {
            var scores = (await GetAllCompetenceScoreDtos()).FirstOrDefault(x => x.IdCompetence != null && x.IdCompetence.Equals(idCompetence)
                                                                              && x.IdGroup != null && x.IdGroup.Equals(idGroup));
            return scores;
        }
        public async Task<List<CompetenceScoreDto>> GetAllCompetenceScoreDtos()
        {
            List<CompetenceScoreDto> CompetenceScores = new List<CompetenceScoreDto>();
            List<CompetenceScore> list = await CompetenceScoreRepo.GetAllCompetenceScores();
            list.ForEach(x => CompetenceScores.Add(ConvertToCompetenceScoreDto(x)));
            return CompetenceScores;
        }

        public async Task<CompetenceScoreDto?> GetCompetenceScoreDtoById(int id)
        {
            return (await GetAllCompetenceScoreDtos()).FirstOrDefault(x => x.Id != null && x.Id.Equals(id));
        }

        public async Task<List<CompetenceScoreDto>> GetCompetenceScoreDtosByGroupId(int id)
        {
            return (await GetAllCompetenceScoreDtos()).Where(x => x.IdGroup != null && x.IdGroup.Equals(id)).ToList();
        }

        public async Task<List<CompetenceScoreDto>> GetCompetenceScoreDtosByCompetenceId(int id)
        {
            return (await GetAllCompetenceScoreDtos()).Where(x => x.IdCompetence != null && x.IdCompetence.Equals(id)).ToList();
        }

        public async Task<CompetenceScore> GetCompetenceScoreById(int id)
        {
            return await CompetenceScoreRepo.GetCompetenceScoreById(id);
        }

        public async Task<bool> SaveCompetenceScore(CompetenceScore CompetenceScoreToSave)
        {
            return await CompetenceScoreRepo.SaveCompetenceScore(CompetenceScoreToSave);
        }

        public async Task<bool> SaveCompetenceScore(CompetenceScoreDto CompetenceScoreDtoToSave)
        {
            return await CompetenceScoreRepo.SaveCompetenceScore(ConvertToCompetenceScore(CompetenceScoreDtoToSave));
        }

        public async Task<bool> SaveCompetenceScore(AddCompetenceScoreModel CompetenceScoreToSave)
        {
            return await CompetenceScoreRepo.SaveCompetenceScore(new CompetenceScore
            {
                IdCompetence = CompetenceScoreToSave.IdCompetence.Value,
                IdGroup = CompetenceScoreToSave.IdGroup.Value,
                Name = CompetenceScoreToSave.Name,
                NumnerPoints = CompetenceScoreToSave.NumberPoints.Value,
                Description = CompetenceScoreToSave.Description
            });
        }
    }
}
