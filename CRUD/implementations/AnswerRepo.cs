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
    public class AnswerRepo : IAnswerRepo
    {
        private readonly EFDbContext context;
        public AnswerRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteAnswerById(int id)
        {
            context.Answers.Remove((await GetAllAnswers()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Answer>> GetAllAnswers()
        {
            return await context.Answers.ToListAsync();
        }

        public async Task<Answer> GetAnswerById(int id)
        {
            return await context.Answers.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveAnswer(Answer AnswerToSave)
        {
            Answer? answer = await GetAnswerById(AnswerToSave.Id);
            //Answer? Answer = await context.Answers.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(AnswerToSave.Id));
            if (answer != null && AnswerToSave.Id != 0)
            {
                /*context.Answers.Entry(AnswerToSave).State = EntityState.Detached;
                context.Set<Answer>().Update(AnswerToSave);*/
                answer.Text = AnswerToSave.Text;
                answer.IdQuestion = AnswerToSave.IdQuestion;
                answer.Correct = AnswerToSave.Correct;
                answer.ImagePath = AnswerToSave.ImagePath;
                answer.Number = AnswerToSave.Number;
                answer.Weight = AnswerToSave.Weight;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Answers.AddAsync(AnswerToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
