using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IQuestionSubcompetenceRepo
    {
        Task<List<QuestionSubcompetence>> GetAllQuestionSubcompetences();
        Task<QuestionSubcompetence?> GetQuestionSubcompetenceById(int id);
        Task<bool> SaveQuestionSubcompetence(QuestionSubcompetence QuestionSubcompetenceToSave);
        Task<bool> DeleteQuestionSubcompetenceById(int id);
    }
}
