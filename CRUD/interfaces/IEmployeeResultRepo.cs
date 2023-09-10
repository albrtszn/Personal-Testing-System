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
        Task<List<EmployeeResult>> GetAllEmployeeResults();
        Task<EmployeeResult> GetEmployeeResultById(int id);
        Task<bool> SaveEmployeeResult(EmployeeResult EmployeeResultToSave);
        Task<bool> DeleteEmployeeResultById(int id);
    }
}
