using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ICompetenceCoeffRepo
    {
        Task<List<СompetenceСoeff>> GetAllCompetenceCoeffs();
        Task<СompetenceСoeff> GetCompetenceCoeffById(int id);
        Task<bool> SaveCompetenceCoeff(СompetenceСoeff CompetenceCoeffToSave);
        Task<bool> DeleteCompetenceCoeffById(int id);
    }
}
