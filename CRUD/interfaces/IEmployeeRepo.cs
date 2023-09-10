using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IEmployeeRepo
    {
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(string id);
        Task<bool> SaveEmployee(Employee EmployeeToSave);
        Task<bool> DeleteEmployeeById(string id);
    }
}
