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
    public class EmployeeResultRepo : IEmployeeResultRepo
    {
        private EFDbContext context;
        public EmployeeResultRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteEmployeeResultById(int id)
        {
            context.EmployeeResults.Remove((await GetAllEmployeeResults()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EmployeeResult>> GetAllEmployeeResults()
        {
            return await context.EmployeeResults.ToListAsync();
        }

        public async Task<EmployeeResult> GetEmployeeResultById(int id)
        {
            return await context.EmployeeResults.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveEmployeeResult(EmployeeResult EmployeeResultToSave)
        {
            await context.EmployeeResults.AddAsync(EmployeeResultToSave);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
