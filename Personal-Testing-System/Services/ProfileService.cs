using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class ProfileService
    {
        private IProfileRepo ProfileRepo;
        public ProfileService(IProfileRepo _сompetenceRepo)
        {
            this.ProfileRepo = _сompetenceRepo;
        }

        private Profile ConvertToProfile(ProfileDto subDto)
        {
            return new Profile
            {
                Id = subDto.Id.Value,
                Name = subDto.Name
            };
        }

        private ProfileDto ConvertToProfileDto(Profile sub)
        {
            return new ProfileDto
            {
                Id = sub.Id,
                Name = sub.Name
            };
        }

        public async Task<bool> DeleteProfileById(int id)
        {
            return await ProfileRepo.DeleteProfileById(id);
        }

        public async Task<List<Profile>> GetAllProfiles()
        {
            return await ProfileRepo.GetAllProfiles();
        }

        public async Task<List<ProfileDto>> GetAllProfileDtos()
        {
            List<ProfileDto> Profiles = new List<ProfileDto>();
            List<Profile> list = await ProfileRepo.GetAllProfiles();
            list.ForEach(x => Profiles.Add(ConvertToProfileDto(x)));
            return Profiles;
        }

        public async Task<Profile> GetProfileById(int id)
        {
            return await ProfileRepo.GetProfileById(id);
        }

        public async Task<ProfileDto> GetProfileDtoById(int id)
        {
            return ConvertToProfileDto(await ProfileRepo.GetProfileById(id));
        }

        public async Task<bool> SaveProfile(Profile ProfileToSave)
        {
            return await ProfileRepo.SaveProfile(ProfileToSave);
        }

        public async Task<bool> SaveProfile(ProfileDto ProfileDtoToSave)
        {
            return await ProfileRepo.SaveProfile(ConvertToProfile(ProfileDtoToSave));
        }

        public async Task<bool> SaveProfile(AddProfileModel ProfileDtoToSave)
        {
            await ProfileRepo.SaveProfile(new Profile { Name = ProfileDtoToSave.Name });
            return true;
        }
    }
}
