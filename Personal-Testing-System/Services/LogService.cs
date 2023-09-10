using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class LogService
    {
        private ILogRepo LogRepo;
        public LogService(ILogRepo _LogRepo)
        {
            this.LogRepo = _LogRepo;
        }

        private LogDto ConvertToLogDto(Log log)
        {
            return new LogDto
            {
                Id = log.Id,
                UrlPath = log.UrlPath,
                UserId = log.UserId,
                UserIp = log.UserIp,
                DataTime = log.DataTime.ToString(),
                Params = log.Params
            };
        }

        private Log ConvertToLog(LogDto logDto)
        {
            return new Log
            {
                Id = logDto.Id,
                UrlPath = logDto.UrlPath,
                UserId = logDto.UserId,
                UserIp = logDto.UserIp,
                DataTime = DateTime.Parse(logDto.DataTime),
                Params = logDto.Params
            };
        }

        public async Task<bool> DeleteLogById(int id)
        {
            return await LogRepo.DeleteLogById(id);
        }

        public async Task<List<Log>> GetAllLogs()
        {
            return await LogRepo.GetAllLogs();
        }

        public async Task<List<LogDto>> GetAllLogDtos()
        {
            List<LogDto> Logs = new List<LogDto>();
            List<Log> list = await LogRepo.GetAllLogs();
            list.ForEach(x => Logs.Add(ConvertToLogDto(x)));
            return Logs;
        }

        public async Task<Log> GetLogById(int id)
        {
            return await LogRepo.GetLogById(id);
        }

        public async Task<bool> SaveLog(Log LogToSave)
        {
            await LogRepo.SaveLog(LogToSave);
            return true;
        }
        public async Task<bool> SaveLogDto(LogDto LogDtoToSave)
        {
            await LogRepo.SaveLog(ConvertToLog(LogDtoToSave));
            return true;
        }
    }
}
