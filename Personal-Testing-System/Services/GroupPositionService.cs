using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;
using System.Linq;

namespace Personal_Testing_System.Services
{
    public class GroupPositionService
    {
        private IGroupPositionRepo GroupPositionRepo;
        public GroupPositionService(IGroupPositionRepo _сompetenceRepo)
        {
            this.GroupPositionRepo = _сompetenceRepo;
        }

        private GroupPosition ConvertToGroupPosition(GroupPositionDto subDto)
        {
            return new GroupPosition
            {
                Id = subDto.Id.Value,
                Name = subDto.Name,
                IdProfile = subDto.IdProfile
            };
        }

        private GroupPositionDto ConvertToGroupPositionDto(GroupPosition sub)
        {
            return new GroupPositionDto
            {
                Id = sub.Id,
                Name = sub.Name,
                IdProfile = sub.IdProfile
            };
        }

        public async Task<bool> DeleteGroupPositionById(int id)
        {
            return await GroupPositionRepo.DeleteGroupPositionById(id);
        }

        public async Task<List<GroupPosition>> GetAllGroupPositions()
        {
            return await GroupPositionRepo.GetAllGroupPositions();
        }

        public async Task<List<GroupPositionDto>> GetAllGroupPositionDtos()
        {
            List<GroupPositionDto> GroupPositions = new List<GroupPositionDto>();
            List<GroupPosition> list = await GroupPositionRepo.GetAllGroupPositions();
            list.ForEach(x => GroupPositions.Add(ConvertToGroupPositionDto(x)));
            return GroupPositions;
        }

        public async Task<GroupPosition> GetGroupPositionById(int id)
        {
            return await GroupPositionRepo.GetGroupPositionById(id);
        }

        public async Task<GroupPositionDto> GetGroupPositionDtoById(int id)
        {
            return ConvertToGroupPositionDto(await GroupPositionRepo.GetGroupPositionById(id));
        }

        public async Task<bool> SaveGroupPosition(GroupPosition GroupPositionToSave)
        {
            return await GroupPositionRepo.SaveGroupPosition(GroupPositionToSave);
        }

        public async Task<bool> SaveGroupPosition(GroupPositionDto GroupPositionDtoToSave)
        {
            return await GroupPositionRepo.SaveGroupPosition(ConvertToGroupPosition(GroupPositionDtoToSave));
        }

        public async Task<bool> SaveGroupPosition(AddGroupPositionModel GroupPositionDtoToSave)
        {
            await GroupPositionRepo.SaveGroupPosition(new GroupPosition 
            { 
                Name = GroupPositionDtoToSave.Name,
                IdProfile = GroupPositionDtoToSave.IdProfile
            }
            );
            return true;
        }
    }
}
