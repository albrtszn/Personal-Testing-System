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
        List<Competence> GetAllCompetences();
        Competence GetCompetenceById(int id);
        void SaveCompetence(Competence CompetenceToSave);
        void DeleteCompetenceById(int id);
    }
}
