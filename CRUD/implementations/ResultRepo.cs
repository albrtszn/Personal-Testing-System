using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class ResultRepo : IResultRepo
    {
        private EFDbContext context;
        public ResultRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteResultById(string id)
        {
            context.Results.Remove((await GetAllResults()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Result>> GetAllResults()
        {
            return await context.Results.ToListAsync();
        }

        public async Task<Result> GetResultById(string id)
        {
            return await context.Results.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveResult(Result ResultToSave)
        {
            await context.Results.AddAsync(ResultToSave);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
