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
    public class EmployeeAnswerRepo : IEmployeeAnswerRepo
    {
        private EFDbContext context;
        public EmployeeAnswerRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteEmployeeAnswerById(int id)
        {
            context.EmployeeAnswers.Remove((await GetAllEmployeeAnswers()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EmployeeAnswer>> GetAllEmployeeAnswers()
        {
            return await context.EmployeeAnswers.ToListAsync();
        }

        public async Task<EmployeeAnswer> GetEmployeeAnswerById(int id)
        {
            return await context.EmployeeAnswers.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveEmployeeAnswer(EmployeeAnswer EmployeeAnswerToSave)
        {
            await context.EmployeeAnswers.AddAsync(EmployeeAnswerToSave);
            await context.SaveChangesAsync();
            return false;
        }
    }
}
