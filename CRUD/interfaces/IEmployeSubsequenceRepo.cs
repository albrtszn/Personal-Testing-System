using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IEmployeeSubsequenceRepo
    {
        List<EmployeeSubsequence> GetAllEmployeeSubsequences();
        EmployeeSubsequence GetEmployeeSubsequenceById(int id);
        void SaveEmployeeSubsequence(EmployeeSubsequence EmployeeSubsequenceToSave);
        void DeleteEmployeeSubsequenceById(int id);
    }
}
