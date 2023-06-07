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
        List<Subdivision> GetAllSubdivisions();
        Subdivision GetSubdivisionById(int id);
        void SaveSubdivision(Subdivision SubdivisionToSave);
        void DeleteSubdivisionById(int id);
    }
}
