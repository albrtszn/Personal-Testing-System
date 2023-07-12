using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IEmployeeResultRepo
    {
        List<EmployeeResult> GetAllEmployeeResults();
        EmployeeResult GetEmployeeResultById(string id);
        void SaveEmployeeResult(EmployeeResult EmployeeResultToSave);
        void DeleteEmployeeResultById(int id);
    }
}
