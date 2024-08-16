using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class CompetenceCoeffService
    {
        private ICompetenceCoeffRepo CompetenceCoeffRepo;
        public CompetenceCoeffService(ICompetenceCoeffRepo _CompetenceCoeffRepo)
        {
            this.CompetenceCoeffRepo = _CompetenceCoeffRepo;
        }
        private CompetenceCoeffDto ConvertToCompetenceCoeffDto(СompetenceСoeff CompetenceCoeff)
        {
            return new CompetenceCoeffDto
            {
                Id = CompetenceCoeff.Id,
                IdCompetence = CompetenceCoeff.IdCompetence,
                IdGroup = CompetenceCoeff.IdGroup,
                Coefficient = CompetenceCoeff.Coefficient
            };
        }
        private СompetenceСoeff ConvertToCompetenceCoeff(CompetenceCoeffDto CompetenceCoeffDto)
        {
            return new СompetenceСoeff
            {
                Id = CompetenceCoeffDto.Id.Value,
                IdCompetence = CompetenceCoeffDto.IdCompetence,
                IdGroup = CompetenceCoeffDto.IdGroup,
                Coefficient = CompetenceCoeffDto.Coefficient
            };
        }
        public async Task<bool> DeleteCompetenceCoeffById(int id)
        {
            return await CompetenceCoeffRepo.DeleteCompetenceCoeffById(id);
        }

        /*public async Task<bool> DeleteCompetenceCoeffsByQuestion(string idQuestion)
        {
            List<СompetenceСoeff> list = (await GetAllCompetenceCoeffs()).Where(x => x.IdQuestion.Equals(idQuestion)).ToList();
            foreach (СompetenceСoeff CompetenceCoeff in list)
            {
                await DeleteCompetenceCoeffById(CompetenceCoeff.Id);
            }
            return true;
        }*/

        public async Task<List<СompetenceСoeff>> GetAllCompetenceCoeffs()
        {
            return await CompetenceCoeffRepo.GetAllCompetenceCoeffs();
        }

        public async Task<List<СompetenceСoeff>> GetCompetenceCoeffsByGroupId(int id)
        {
            return (await GetAllCompetenceCoeffs()).Where(x => x.IdGroup != null && x.IdGroup.Equals(id)).ToList();
        }

        public async Task<List<СompetenceСoeff>> GetCompetenceCoeffsByCompetenceId(int id)
        {
            return (await GetAllCompetenceCoeffs()).Where(x => x.IdCompetence != null && x.IdCompetence.Equals(id)).ToList();
        }

        public async Task<List<CompetenceCoeffDto>> GetCompetenceCoeffDtosByGroupId(int id)
        {
            var coeffs = (await GetAllCompetenceCoeffs()).Where(x => x.IdGroup != null && x.IdGroup.Equals(id)).ToList();
            List<CompetenceCoeffDto> dtos = new List<CompetenceCoeffDto>();
            foreach(var coeff in coeffs)
            {
                dtos.Add(ConvertToCompetenceCoeffDto(coeff));
            }
            return dtos;
        }

        public async Task<CompetenceCoeffDto?> GetCompetenceCoeffDtoByCompetenceAndGroupId(int idCompetence, int idGroup)
        {
            var coeff = (await GetAllCompetenceCoeffDtos()).FirstOrDefault(x => x.IdCompetence != null && x.IdCompetence.Equals(idCompetence) 
                                                                              && x.IdGroup != null && x.IdGroup.Equals(idGroup));
            return coeff;
        }

        public async Task<List<CompetenceCoeffDto>> GetAllCompetenceCoeffDtos()
        {
            List<CompetenceCoeffDto> CompetenceCoeffs = new List<CompetenceCoeffDto>();
            List<СompetenceСoeff> list = await CompetenceCoeffRepo.GetAllCompetenceCoeffs();
            list.ForEach(x => CompetenceCoeffs.Add(ConvertToCompetenceCoeffDto(x)));
            return CompetenceCoeffs;
        }

        public async Task<СompetenceСoeff> GetCompetenceCoeffById(int id)
        {
            return await CompetenceCoeffRepo.GetCompetenceCoeffById(id);
        }

        public async Task<CompetenceCoeffDto?> GetCompetenceCoeffDtoById(int id)
        {
            return ConvertToCompetenceCoeffDto( await CompetenceCoeffRepo.GetCompetenceCoeffById(id));
        }

        public async Task<bool> SaveCompetenceCoeff(СompetenceСoeff CompetenceCoeffToSave)
        {
            return await CompetenceCoeffRepo.SaveCompetenceCoeff(CompetenceCoeffToSave);
        }

        public async Task<bool> SaveCompetenceCoeff(CompetenceCoeffDto CompetenceCoeffToSave)
        {
            return await CompetenceCoeffRepo.SaveCompetenceCoeff(ConvertToCompetenceCoeff(CompetenceCoeffToSave));
        }

        public async Task<bool> SaveCompetenceCoeff(AddCompetenceCoeffModel CompetenceCoeffToSave)
        {
            return await CompetenceCoeffRepo.SaveCompetenceCoeff(new СompetenceСoeff
            {
                IdCompetence = CompetenceCoeffToSave.IdCompetence,
                IdGroup = CompetenceCoeffToSave.IdGroup,
                Coefficient = CompetenceCoeffToSave.Coefficient
            });
        }
    }
}
