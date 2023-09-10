using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ICompetenceRepo
    {
        Task<List<Competence>> GetAllCompetences();
        Task<Competence> GetCompetenceById(int id);
        Task<bool> SaveCompetence(Competence CompetenceToSave);
        Task<bool> DeleteCompetenceById(int id);
    }
}
