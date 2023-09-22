using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IProfileRepo
    {
        Task<List<Profile>> GetAllProfiles();
        Task<Profile> GetProfileById(int id);
        Task<bool> SaveProfile(Profile ProfileToSave);
        Task<bool> DeleteProfileById(int id);
    }
}
