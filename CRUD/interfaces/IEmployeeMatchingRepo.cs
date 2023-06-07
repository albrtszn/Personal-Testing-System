using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IEmployeeMatchingRepo
    {
        List<EmployeeMatching> GetAllEmployeeMatchings();
        EmployeeMatching GetEmployeeMatchingById(int id);
        void SaveEmployeeMatching(EmployeeMatching EmployeeMatchingToSave);
        void DeleteEmployeeMatchingById(int id);
    }
}
