using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IResultRepo
    {
        List<Result> GetAllResults();
        Result GetResultById(int id);
        void SaveResult(Result ResultToSave);
        void DeleteResultById(int id);
    }
}
