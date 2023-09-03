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
        List<TokenAdmin> GetAllTokenAdmins();
        TokenAdmin GetTokenAdminById(int id);
        void SaveTokenAdmin(TokenAdmin TokenAdminToSave);
        void DeleteTokenAdminById(int id);
    }
}
