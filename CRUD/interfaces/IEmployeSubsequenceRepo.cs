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
        Task<List<EmployeeSubsequence>> GetAllEmployeeSubsequences();
        Task<EmployeeSubsequence> GetEmployeeSubsequenceById(int id);
        Task<bool> SaveEmployeeSubsequence(EmployeeSubsequence EmployeeSubsequenceToSave);
        Task<bool> DeleteEmployeeSubsequenceById(int id);
    }
}
