using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IAdminRepo
    {
        List<Admin> GetAllAdmins();
        Admin GetAdminById(int id);
        void SaveAdmin(Admin AdminToSave);
        void DeleteAdminById(int id);
    }
}
