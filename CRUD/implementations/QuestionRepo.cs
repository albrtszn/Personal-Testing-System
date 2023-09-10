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
    public class QuestionRepo : IQuestionRepo
    {
        private EFDbContext context;
        public QuestionRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteQuestionById(string id)
        {
            context.Questions.Remove((await GetAllQuestions()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Question>> GetAllQuestions()
        {
            return await context.Questions.ToListAsync();
        }

        public async Task<Question> GetQuestionById(string id)
        {
            return await context.Questions.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveQuestion(Question QuestionToSave)
        {
            Question? question = await GetQuestionById(QuestionToSave.Id);
            //Question? Question = await context.Questions.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(QuestionToSave.Id));
            if (question != null && !QuestionToSave.Id.IsNullOrEmpty())
            {
                /*context.Questions.Entry(QuestionToSave).State = EntityState.Detached;
                context.Set<Question>().Update(QuestionToSave);*/
                question.Text = QuestionToSave.Text;
                question.IdQuestionType = QuestionToSave.IdQuestionType;
                question.IdTest = QuestionToSave.IdTest;
                question.ImagePath = QuestionToSave.ImagePath;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Questions.AddAsync(QuestionToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
