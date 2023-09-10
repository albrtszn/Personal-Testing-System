using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> DeleteLogById(int id)
        {
            context.Logs.Remove((await GetAllLogs()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Log>> GetAllLogs()
        {
            return await context.Logs.ToListAsync();
        }

        public async Task<Log> GetLogById(int id)
        {
            return await context.Logs.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveLog(Log LogToSave)
        {
            await context.Logs.AddAsync(LogToSave);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
