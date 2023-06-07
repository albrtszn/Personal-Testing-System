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
        List<FirstPart> GetAllFirstParts();
        FirstPart GetFirstPartById(int id);
        void SaveFirstPArt(FirstPart FirstPartoSave);
        void DeleteFirstPartById(int id);
    }
}
