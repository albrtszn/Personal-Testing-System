using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IQuestionRepo
    {
        List<Question> GetAllQuestions();
        Question GetByQuestionId(int id);
        void SaveQuestion(Question QuestionToSave);
        void DeleteQuestionById(int id);
    }
}
