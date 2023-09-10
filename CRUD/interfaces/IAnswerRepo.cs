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
        Task<List<Answer>> GetAllAnswers();
        Task<Answer> GetAnswerById(int id);
        Task<bool> SaveAnswer(Answer AnswerToSave);
        Task<bool> DeleteAnswerById(int id);
    }
}
