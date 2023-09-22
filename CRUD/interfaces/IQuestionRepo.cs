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
        Task<List<Question>> GetAllQuestions();
        Task<Question> GetQuestionById(string id);
        Task<bool> SaveQuestion(Question QuestionToSave);
        Task<bool> DeleteQuestionById(string id);
        Task<List<Question>> GetAllQuestionsByTestId(string testId);
    }
}
