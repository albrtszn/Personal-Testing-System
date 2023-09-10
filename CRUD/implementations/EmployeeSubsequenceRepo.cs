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
    public class EmployeeSubsequenceRepo : IEmployeeSubsequenceRepo
    {
        private EFDbContext context;
        public EmployeeSubsequenceRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteEmployeeSubsequenceById(int id)
        {
            context.EmployeeSubsequences.Remove((await GetAllEmployeeSubsequences()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EmployeeSubsequence>> GetAllEmployeeSubsequences()
        {
            return await context.EmployeeSubsequences.ToListAsync();
        }

        public async Task<EmployeeSubsequence> GetEmployeeSubsequenceById(int id)
        {
            return await context.EmployeeSubsequences.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveEmployeeSubsequence(EmployeeSubsequence EmployeeSubsequenceToSave)
        {
            await context.EmployeeSubsequences.AddAsync(EmployeeSubsequenceToSave);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
