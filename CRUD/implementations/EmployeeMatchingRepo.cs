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
    public class EmployeeMatchingRepo : IEmployeeMatchingRepo
    {
        private EFDbContext context;
        public EmployeeMatchingRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteEmployeeMatchingById(int id)
        {
            context.EmployeeMatchings.Remove((await GetAllEmployeeMatchings()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EmployeeMatching>> GetAllEmployeeMatchings()
        {
            return await context.EmployeeMatchings.ToListAsync();
        }

        public async Task<EmployeeMatching> GetEmployeeMatchingById(int id)
        {
            return await context.EmployeeMatchings.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveEmployeeMatching(EmployeeMatching EmployeeMatchingToSave)
        {
            await context.EmployeeMatchings.AddAsync(EmployeeMatchingToSave);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
