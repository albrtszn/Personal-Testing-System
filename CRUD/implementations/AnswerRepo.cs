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
    public class AnswerRepo : IAnswerRepo
    {
        private EFDbContext context;
        public AnswerRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteAnswerById(int id)
        {
            context.Answers.Remove(GetAllAnswers().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Answer> GetAllAnswers()
        {
            return context.Answers.ToList();
        }

        public Answer GetAnswerById(int id)
        {
            return GetAllAnswers().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveAnswer(Answer AnswerToSave)
        {
            context.Answers.Add(AnswerToSave);
            context.SaveChanges();
        }
    }
}
