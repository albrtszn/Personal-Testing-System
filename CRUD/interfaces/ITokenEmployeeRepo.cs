using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ITokenEmployeeRepo
    {
        Task<List<TokenEmployee>> GetAllTokenEmployees();
        Task<TokenEmployee> GetTokenEmployeeById(int id);
        Task<bool> SaveTokenEmployee(TokenEmployee TokenEmployeeToSave);
        Task<bool> DeleteTokenEmployeeById(int id);
    }
}
