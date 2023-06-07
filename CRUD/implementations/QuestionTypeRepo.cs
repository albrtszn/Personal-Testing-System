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
    public class QuestionTypeRepo : IQuestionTypeRepo
    {
        private EFDbContext context;
        public QuestionTypeRepo(EFDbContext _context)
        {
            this.context = _context;
        }

        public void DeleteQuestionTypeById(int id)
        {
            context.QuestionTypes.Remove(GetAllQuestionTypes().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<QuestionType> GetAllQuestionTypes()
        {
            return context.QuestionTypes.ToList();
        }

        public QuestionType GetQuestionTypeById(int id)
        {
            return GetAllQuestionTypes().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveQuestionType(QuestionType QuestionTypeToSave)
        {
            context.QuestionTypes.Add(QuestionTypeToSave);
            context.SaveChanges();
        }
    }
}
