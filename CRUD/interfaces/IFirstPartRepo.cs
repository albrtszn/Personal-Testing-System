using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IFirstPartRepo
    {
        Task<List<FirstPart>> GetAllFirstParts();
        Task<FirstPart> GetFirstPartById(string id);
        Task<bool> SaveFirstPart(FirstPart FirstPartToSave);
        Task<bool> DeleteFirstPartById(string id);
    }
}
