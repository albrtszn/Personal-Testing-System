using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
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
        public void DeleteQuestionById(int id)
        {
            context.Questions.Remove(GetAllQuestions().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Question> GetAllQuestions()
        {
            return context.Questions.ToList();
        }

        public Question GetByQuestionId(int id)
        {
            return GetAllQuestions().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveQuestion(Question QuestionToSave)
        {
            context.Questions.Add(QuestionToSave);
            context.SaveChanges();
        }
    }
}
