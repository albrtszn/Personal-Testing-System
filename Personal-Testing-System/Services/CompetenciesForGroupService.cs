using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class CompetenciesForGroupService
    {
        private ICompetenciesForGroupRepo CompetenciesForGroupRepo;
        public CompetenciesForGroupService(ICompetenciesForGroupRepo _сompetenceRepo)
        {
            this.CompetenciesForGroupRepo = _сompetenceRepo;
        }

        private CompetenciesForGroup ConvertToCompetenciesForGroup(CompetenciesForGroupDto subDto)
        {
            return new CompetenciesForGroup
            {
                Id = subDto.Id.Value,
                IdTest = subDto.IdTest,
                IdGroupPositions = subDto.IdGroupPositions
            };
        }

        private CompetenciesForGroupDto ConvertToCompetenciesForGroupDto(CompetenciesForGroup sub)
        {
            return new CompetenciesForGroupDto
            {
                Id = sub.Id,
                IdTest = sub.IdTest,
                IdGroupPositions = sub.IdGroupPositions
            };
        }

        public async Task<bool> DeleteCompetenciesForGroupById(int id)
        {
            return await CompetenciesForGroupRepo.DeleteCompetenciesForGroupById(id);
        }

        public async Task<List<CompetenciesForGroup>> GetAllCompetenciesForGroups()
        {
            return await CompetenciesForGroupRepo.GetAllCompetenciesForGroups();
        }

        public async Task<List<CompetenciesForGroupDto>> GetAllCompetenciesForGroupDtos()
        {
            List<CompetenciesForGroupDto> CompetenciesForGroups = new List<CompetenciesForGroupDto>();
            List<CompetenciesForGroup> list = await CompetenciesForGroupRepo.GetAllCompetenciesForGroups();
            list.ForEach(x => CompetenciesForGroups.Add(ConvertToCompetenciesForGroupDto(x)));
            return CompetenciesForGroups;
        }

        public async Task<CompetenciesForGroup> GetCompetenciesForGroupByEmployeeTestId(string idTest, int groupPositionId)
        {
            CompetenciesForGroup? testForGroup = (await CompetenciesForGroupRepo.GetAllCompetenciesForGroups())
                .Find(x => x!= null && x.IdTest !=null && x.IdGroupPositions != null && x.IdTest.Equals(idTest) && x.IdGroupPositions.Equals(groupPositionId));
            return testForGroup;
        }

        public async Task<CompetenciesForGroup> GetCompetenciesForGroupById(int id)
        {
            return await CompetenciesForGroupRepo.GetCompetenciesForGroupById(id);
        }

        public async Task<CompetenciesForGroupDto> GetCompetenciesForGroupDtoById(int id)
        {
            return ConvertToCompetenciesForGroupDto(await CompetenciesForGroupRepo.GetCompetenciesForGroupById(id));
        }

        public async Task<bool> SaveCompetenciesForGroup(CompetenciesForGroup CompetenciesForGroupToSave)
        {
            return await CompetenciesForGroupRepo.SaveCompetenciesForGroup(CompetenciesForGroupToSave);
        }

        public async Task<bool> SaveCompetenciesForGroup(CompetenciesForGroupDto CompetenciesForGroupDtoToSave)
        {
            return await CompetenciesForGroupRepo.SaveCompetenciesForGroup(ConvertToCompetenciesForGroup(CompetenciesForGroupDtoToSave));
        }

        public async Task<bool> SaveCompetenciesForGroup(AddCompetenciesForGroupModel CompetenciesForGroupDtoToSave)
        {
            await CompetenciesForGroupRepo.SaveCompetenciesForGroup(new CompetenciesForGroup { 
                IdTest = CompetenciesForGroupDtoToSave.IdTest,
                IdGroupPositions = CompetenciesForGroupDtoToSave.IdGroupPositions
            });
            return true;
        }
    }
}
