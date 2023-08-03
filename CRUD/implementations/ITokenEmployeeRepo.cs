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
        List<TokenEmployee> GetAllTokenEmployees();
        TokenEmployee GetTokenEmployeeById(int id);
        void SaveTokenEmployee(TokenEmployee TokenEmployeeToSave);
        void DeleteTokenEmployeeById(int id);
    }
}
