using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ISubdivisionRepo
    {
        Task<List<Subdivision>> GetAllSubdivisions();
        Task<Subdivision> GetSubdivisionById(int id);
        Task<bool> SaveSubdivision(Subdivision SubdivisionToSave);
        Task<bool> DeleteSubdivisionById(int id);
    }
}
