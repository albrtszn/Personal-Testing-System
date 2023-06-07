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
        List<SecondPart> GetAllSecondParts();
        SecondPart GetSecondPartById(int id);
        void SaveSecondPart(SecondPart SecondPartToSave);
        void DeleteSecondPartById(int id);
    }
}
