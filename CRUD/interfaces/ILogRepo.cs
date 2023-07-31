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
        List<Log> GetAllLogs();
        Log GetLogById(int id);
        void SaveLog(Log LogToSave);
        void DeleteLogById(int id);
    }
}
