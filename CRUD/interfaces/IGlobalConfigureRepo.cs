using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IGlobalConfigureRepo
    {
        Task<List<GlobalConfigure>> GetAllGlobalConfigures();
        Task<GlobalConfigure> GetGlobalConfigureById(int id);
        Task<bool> SaveGlobalConfigure(GlobalConfigure GlobalConfigureToSave);
        Task<bool> DeleteGlobalConfigureById(int id);
    }
}
