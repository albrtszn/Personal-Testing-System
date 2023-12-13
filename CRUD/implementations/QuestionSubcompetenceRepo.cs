using CRUD.interfaces;
using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CRUD.implementations
{
    public class QuestionSubcompetenceRepo : IQuestionSubcompetenceRepo
    {
        private readonly EFDbContext context;
        public QuestionSubcompetenceRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteQuestionSubcompetenceById(int id)
        {
            context.QuestionSubcompetences.Remove((await GetAllQuestionSubcompetences()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionSubcompetence>> GetAllQuestionSubcompetences()
        {
            return await context.QuestionSubcompetences.ToListAsync();
        }

        public async Task<QuestionSubcompetence> GetQuestionSubcompetenceById(int id)
        {
            return await context.QuestionSubcompetences.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }


        public async Task<bool> SaveQuestionSubcompetence(QuestionSubcompetence QuestionSubcompetenceToSave)
        {
            QuestionSubcompetence? QuestionSubcompetence = await GetQuestionSubcompetenceById(QuestionSubcompetenceToSave.Id);
            //QuestionSubcompetence? QuestionSubcompetence = await context.QuestionSubcompetences.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(QuestionSubcompetenceToSave.Id));
            if (QuestionSubcompetence != null && QuestionSubcompetenceToSave.Id != 0)
            {
                /*context.QuestionSubcompetences.Entry(QuestionSubcompetenceToSave).State = EntityState.Detached;
                context.Set<QuestionSubcompetence>().Update(QuestionSubcompetenceToSave);*/
                QuestionSubcompetence.IdQuestion = QuestionSubcompetenceToSave.IdQuestion;
                QuestionSubcompetence.IdSubcompetence = QuestionSubcompetenceToSave.IdSubcompetence;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.QuestionSubcompetences.AddAsync(QuestionSubcompetenceToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
