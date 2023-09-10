using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ISecondPartRepo
    {
        Task<List<SecondPart>> GetAllSecondParts();
        Task<SecondPart> GetSecondPartById(int id);
        Task<bool> SaveSecondPart(SecondPart SecondPartToSave);
        Task<bool> DeleteSecondPartById(int id);
    }
}
