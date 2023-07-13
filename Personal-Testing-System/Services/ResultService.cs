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
                ScoreFrom = Result.ScoreFrom,
                ScoreTo = Result.ScoreTo,
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
                ScoreFrom = ResultDto.ScoreFrom,
                ScoreTo = ResultDto.ScoreTo,
                StartDate = DateOnly.Parse(ResultDto.StartDate),
                StartTime = TimeOnly.Parse(ResultDto.StartTime),
                Duration = ResultDto.Duration,
                EndTime = TimeOnly.Parse(ResultDto.EndTime),
                Description = ResultDto.Description
            };
        }
        public void DeleteResultById(string id)
        {
            ResultRepo.DeleteResultById(id);
        }

        public List<Result> GetAllResults()
        {
            return ResultRepo.GetAllResults();
        }

        public List<ResultDto> GetAllResultDtos()
        {
            List<ResultDto> Results = new List<ResultDto>();
            ResultRepo.GetAllResults().ForEach(x => Results.Add(ConvertToResultDto(x)));
            return Results;
        }

        public Result GetResultById(string id)
        {
            return ResultRepo.GetResultById(id);
        }

        public ResultDto GetResultDtoById(string id)
        {
            return ConvertToResultDto(ResultRepo.GetResultById(id));
        }

        public void SaveResult(Result ResultToSave)
        {
            ResultRepo.SaveResult(ResultToSave);
        }
    }
}
