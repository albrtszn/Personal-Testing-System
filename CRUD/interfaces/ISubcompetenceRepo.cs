using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ISubcompetenceRepo
    {
        Task<List<Subcompetence>> GetAllSubcompetences();
        Task<Subcompetence> GetSubcompetenceById(int id);
        Task<bool> SaveSubcompetence(Subcompetence SubcompetenceToSave);
        Task<bool> DeleteSubcompetenceById(int id);
    }
}
