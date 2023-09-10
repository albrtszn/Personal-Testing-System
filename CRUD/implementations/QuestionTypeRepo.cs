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
    public class QuestionTypeRepo : IQuestionTypeRepo
    {
        private EFDbContext context;
        public QuestionTypeRepo(EFDbContext _context)
        {
            this.context = _context;
        }

        public async Task<bool> DeleteQuestionTypeById(int id)
        {
            context.QuestionTypes.Remove((await GetAllQuestionTypes()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionType>> GetAllQuestionTypes()
        {
            return await context.QuestionTypes.ToListAsync();
        }

        public async Task<QuestionType> GetQuestionTypeById(int id)
        {
            return await context.QuestionTypes.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveQuestionType(QuestionType QuestionTypeToSave)
        {
            await context.QuestionTypes.AddAsync(QuestionTypeToSave);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
