using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ICompetenceScoreRepo
    {
        Task<List<CompetenceScore>> GetAllCompetenceScores();
        Task<CompetenceScore> GetCompetenceScoreById(int id);
        Task<bool> SaveCompetenceScore(CompetenceScore CompetenceScoreToSave);
        Task<bool> DeleteCompetenceScoreById(int id);
    }
}
