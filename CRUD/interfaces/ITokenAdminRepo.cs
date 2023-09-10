using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ITokenAdminRepo
    {
        Task<List<TokenAdmin>> GetAllTokenAdmins();
        Task<TokenAdmin> GetTokenAdminById(int id);
        Task<bool> SaveTokenAdmin(TokenAdmin AdminToSave);
        Task<bool> DeleteTokenAdminById(int id);
    }
}
