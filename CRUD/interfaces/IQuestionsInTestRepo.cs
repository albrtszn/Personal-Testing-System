using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IQuestionsInTestRepo
    {
        List<QuestionsInTest> GetAllQuestionsInTests();
        QuestionsInTest GetQuestionInTestById(int id);
        void SaveQustionInTest(QuestionsInTest QuestionsInTestToSave);
        void DeleteQuestionInTestById(int id);
    }
}
