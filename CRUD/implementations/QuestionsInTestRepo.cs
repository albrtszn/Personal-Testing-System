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
    public class QuestionsInTestRepo : IQuestionsInTestRepo
    {
        private EFDbContext context;
        public QuestionsInTestRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteQuestionInTestById(int id)
        {
            //context.QuestionsInTest.Remove(GetAllQuestionsInTests().FirstOrDefault(x => x.IdQuestion.Equals(id)));
            context.SaveChanges();
        }

        public List<QuestionsInTest> GetAllQuestionsInTests()
        {
            return new List<QuestionsInTest>();//context.QuestionsInTest.ToList();
        }

        public QuestionsInTest GetQuestionInTestById(int id)
        {
            return GetAllQuestionsInTests().FirstOrDefault(x => x.IdQuestion.Equals(id));
        }

        public void SaveQustionInTest(QuestionsInTest QuestionsInTestToSave)
        {
            //context.QuestionsInTest.Add(QuestionsInTestToSave);
            context.SaveChanges();
        }
    }
}
