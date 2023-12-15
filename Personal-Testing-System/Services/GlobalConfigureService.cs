using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class GlobalConfigureService
    {
        private IGlobalConfigureRepo GlobalConfigureRepo;
        public GlobalConfigureService(IGlobalConfigureRepo _GlobalConfigureRepo)
        {
            this.GlobalConfigureRepo = _GlobalConfigureRepo;
        }
        private GlobalConfigureDto ConvertToGlobalConfigureDto(GlobalConfigure GlobalConfigure)
        {
            return new GlobalConfigureDto
            {
                Id = GlobalConfigure.Id,
                TestingTimeLimit= GlobalConfigure.TestingTimeLimit,
                SkippingQuestion = GlobalConfigure.SkippingQuestion,
                EarlyCompletionTesting = GlobalConfigure.EarlyCompletionTesting,
                AdditionalBool = GlobalConfigure.AdditionalBool,
                AdditionalInt = GlobalConfigure.AdditionalInt,
                AdditionalString = GlobalConfigure.AdditionalString
            };
        }
        private GlobalConfigure ConvertToGlobalConfigure(GlobalConfigureDto GlobalConfigureDto)
        {
            return new GlobalConfigure
            {
                Id = GlobalConfigureDto.Id.Value,
                TestingTimeLimit = GlobalConfigureDto.TestingTimeLimit.Value,
                SkippingQuestion = GlobalConfigureDto.SkippingQuestion.Value,
                EarlyCompletionTesting = GlobalConfigureDto.EarlyCompletionTesting.Value,
                AdditionalBool = GlobalConfigureDto.AdditionalBool,
                AdditionalInt = GlobalConfigureDto.AdditionalInt,
                AdditionalString = GlobalConfigureDto.AdditionalString
            };
        }

        private GlobalConfigure ConvertToGlobalConfigure(AddGlobalConfigureModel GlobalConfigureDto)
        {
            return new GlobalConfigure
            {
                TestingTimeLimit = GlobalConfigureDto.TestingTimeLimit.Value,
                SkippingQuestion = GlobalConfigureDto.SkippingQuestion.Value,
                EarlyCompletionTesting = GlobalConfigureDto.EarlyCompletionTesting.Value,
                AdditionalBool = GlobalConfigureDto.AdditionalBool,
                AdditionalInt = GlobalConfigureDto.AdditionalInt,
                AdditionalString = GlobalConfigureDto.AdditionalString
            };
        }
        public async Task<bool> DeleteGlobalConfigureById(int id)
        {
            return await GlobalConfigureRepo.DeleteGlobalConfigureById(id);
        }

        public async Task<List<GlobalConfigure>> GetAllGlobalConfigures()
        {
            return await GlobalConfigureRepo.GetAllGlobalConfigures();
        }

        public async Task<List<GlobalConfigureDto>> GetAllGlobalConfigureDtos()
        {
            List<GlobalConfigureDto> GlobalConfigures = new List<GlobalConfigureDto>();
            List<GlobalConfigure> list = await GlobalConfigureRepo.GetAllGlobalConfigures();
            list.ForEach(x => GlobalConfigures.Add(ConvertToGlobalConfigureDto(x)));
            return GlobalConfigures;
        }

        public async Task<GlobalConfigure> GetGlobalConfigureById(int id)
        {
            return await GlobalConfigureRepo.GetGlobalConfigureById(id);
        }

        public async Task<bool> SaveGlobalConfigure(GlobalConfigure GlobalConfigureToSave)
        {
            return await GlobalConfigureRepo.SaveGlobalConfigure(GlobalConfigureToSave);
        }         
        public async Task<bool> SaveGlobalConfigure(GlobalConfigureDto GlobalConfigureToSave)
        {
            return await GlobalConfigureRepo.SaveGlobalConfigure(ConvertToGlobalConfigure(GlobalConfigureToSave));
        } 
        public async Task<bool> SaveGlobalConfigure(AddGlobalConfigureModel GlobalConfigureToSave)
        {
            return await GlobalConfigureRepo.SaveGlobalConfigure(ConvertToGlobalConfigure(GlobalConfigureToSave));
        }
    }
}
