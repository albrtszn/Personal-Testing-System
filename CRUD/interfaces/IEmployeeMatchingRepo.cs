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
        Task<List<EmployeeMatching>> GetAllEmployeeMatchings();
        Task<EmployeeMatching> GetEmployeeMatchingById(int id);
        Task<bool> SaveEmployeeMatching(EmployeeMatching EmployeeMatchingToSave);
        Task<bool> DeleteEmployeeMatchingById(int id);
    }
}
