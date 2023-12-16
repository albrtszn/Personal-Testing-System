using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ITestScoreRepo
    {
        Task<List<TestScore>> GetAllTestScores();
        Task<TestScore> GetTestScoreById(int id);
        Task<bool> SaveTestScore(TestScore TestScoreToSave);
        Task<bool> DeleteTestScoreById(int id);
    }
}
