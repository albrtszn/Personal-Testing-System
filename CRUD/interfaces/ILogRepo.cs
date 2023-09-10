using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ILogRepo
    {
        Task<List<Log>> GetAllLogs();
        Task<Log> GetLogById(int id);
        Task<bool> SaveLog(Log LogToSave);
        Task<bool> DeleteLogById(int id);
    }
}
