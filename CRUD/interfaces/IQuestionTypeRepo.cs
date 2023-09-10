using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IQuestionTypeRepo
    {
        Task<List<QuestionType>> GetAllQuestionTypes();
        Task<QuestionType> GetQuestionTypeById(int id);
        Task<bool> SaveQuestionType(QuestionType QuestionTypeToSave);
        Task<bool> DeleteQuestionTypeById(int id);
    }
}
