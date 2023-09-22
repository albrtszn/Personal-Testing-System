using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ICompetenciesForGroupRepo
    {
        Task<List<CompetenciesForGroup>> GetAllCompetenciesForGroups();
        Task<CompetenciesForGroup> GetCompetenciesForGroupById(int id);
        Task<bool> SaveCompetenciesForGroup(CompetenciesForGroup CompetenciesForGroupToSave);
        Task<bool> DeleteCompetenciesForGroupById(int id);
    }
}
