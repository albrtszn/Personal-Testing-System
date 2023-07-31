using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class LogRepo : ILogRepo
    {
        private EFDbContext context;
        public LogRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteLogById(int id)
        {
            context.Logs.Remove(GetAllLogs().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Log> GetAllLogs()
        {
            return context.Logs.ToList();
        }

        public Log GetLogById(int id)
        {
            return GetAllLogs().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveLog(Log LogToSave)
        {
            context.Logs.Add(LogToSave);
            context.SaveChanges();
        }
    }
}
