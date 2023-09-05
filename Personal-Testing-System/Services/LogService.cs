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

        public void DeleteLogById(int id)
        {
            LogRepo.DeleteLogById(id);
        }

        public List<Log> GetAllLogs()
        {
            return LogRepo.GetAllLogs();
        }

        public List<LogDto> GetAllLogDtos()
        {
            List<LogDto> list = new List<LogDto>();
            LogRepo.GetAllLogs().ForEach(x => list.Add(ConvertToLogDto(x)));
            return list;
        }

        public Log GetLogById(int id)
        {
            return LogRepo.GetLogById(id);
        }

        public void SaveLog(Log LogToSave)
        {
            LogRepo.SaveLog(LogToSave);
        }
        public void SaveLogDto(LogDto LogDtoToSave)
        {
            LogRepo.SaveLog(ConvertToLog(LogDtoToSave));
        }
    }
}
