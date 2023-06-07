using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IAnswerRepo
    {
        List<Answer> GetAllAnswers();
        Answer GetAnswerById(int id);
        void SaveAnswer(Answer AnswerToSave);
        void DeleteAnswerById(int id);
    }
}
