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
        List<QuestionType> GetAllQuestionTypes();
        QuestionType GetQuestionTypeById(int id);
        void SaveQuestionType(QuestionType QuestionTypeToSave);
        void DeleteQuestionTypeById(int id);
    }
}
