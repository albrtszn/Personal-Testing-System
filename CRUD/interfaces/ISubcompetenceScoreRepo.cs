using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ISubcompetenceScoreRepo
    {
        Task<List<SubcompetenceScore>> GetAllSubcompetenceScores();
        Task<SubcompetenceScore> GetSubcompetenceScoreById(int id);
        Task<bool> SaveSubcompetenceScore(SubcompetenceScore SubcompetenceScoreToSave);
        Task<bool> DeleteSubcompetenceScoreById(int id);
    }
}
