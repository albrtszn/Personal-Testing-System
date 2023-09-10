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
        Task<List<Admin>> GetAllAdmins();
        Task<Admin> GetAdminById(string id);
        Task<bool> SaveAdmin(Admin AdminToSave);
        Task<bool> DeleteAdminById(string id);
    }
}
