using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class ResultService
    {
        private IResultRepo ResultRepo;
        public ResultService(IResultRepo _ResultRepo)
        {
            this.ResultRepo = _ResultRepo;
        }
        private ResultDto ConvertToResultDto(Result Result)
        {
            return new ResultDto
            {
                Id = Result.Id,
                IdTest = Result.IdTest,
                StartDate = Result.StartDate.ToString(),
                StartTime = Result.StartTime.ToString(),
                Duration = Result.Duration,
                EndTime = Result.EndTime.ToString(),
                Description = Result.Description
            };
        }
        private Result ConvertToResult(ResultDto ResultDto)
        {
            return new Result
            {
                Id = ResultDto.Id,
                IdTest = ResultDto.IdTest,
                StartDate = DateOnly.Parse(ResultDto.StartDate),
                StartTime = TimeOnly.Parse(ResultDto.StartTime),
                Duration = ResultDto.Duration,
                EndTime = TimeOnly.Parse(ResultDto.EndTime),
                Description = ResultDto.Description
            };
        }
        public async Task<List<Result>> GetAllResults()
        {
            return await ResultRepo.GetAllResults();
        }
        public async Task<bool> DeleteResultById(string id)
        {
            return await ResultRepo.DeleteResultById(id);
        }

        public async Task<List<ResultDto>> GetAllResultDtos()
        {
            List<ResultDto> Results = new List<ResultDto>();
            List<Result> list = await ResultRepo.GetAllResults();
            list.ForEach(x => Results.Add(ConvertToResultDto(x)));
            return Results;
        }

        public async Task<Result> GetResultById(string id)
        {
            return await ResultRepo.GetResultById(id);
        }

        public async Task<ResultDto> GetResultDtoById(string id)
        {
            return ConvertToResultDto(await ResultRepo.GetResultById(id));
        }

        public async Task<bool> SaveResult(Result ResultToSave)
        {
            return await ResultRepo.SaveResult(ResultToSave);
        }
    }
}
