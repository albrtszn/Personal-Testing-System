using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
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
        public void DeleteQuestionById(string id)
        {
            context.Questions.Remove(GetAllQuestions().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Question> GetAllQuestions()
        {
            return context.Questions.ToList();
        }

        public Question GetQuestionById(string id)
        {
            return GetAllQuestions().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveQuestion(Question QuestionToSave)
        {
            Question? quest = GetQuestionById(QuestionToSave.Id);
            if (quest != null && !QuestionToSave.Id.IsNullOrEmpty())
            {
                //context.Questions.Update(QuestionToSave);
                quest.Text = QuestionToSave.Text;
                quest.IdQuestionType = QuestionToSave.IdQuestionType;
                quest.IdTest = QuestionToSave.IdTest;
                quest.ImagePath = QuestionToSave.ImagePath;
            }
            else
            {
                context.Questions.Add(QuestionToSave);
            }
            context.SaveChanges();
        }
    }
}
