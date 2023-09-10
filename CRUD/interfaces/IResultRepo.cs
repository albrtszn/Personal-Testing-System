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
        Task<List<Result>> GetAllResults();
        Task<Result> GetResultById(string id);
        Task<bool> SaveResult(Result ResultToSave);
        Task<bool> DeleteResultById(string id);
    }
}
