using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private EFDbContext context;
        public EmployeeRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteEmployeeById(int id)
        {
            context.Employees.Remove(GetAllEmployees().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Employee> GetAllEmployees()
        {
            return context.Employees.ToList();
        }

        public Employee GetEmployeeById(int id)
        {
            return GetAllEmployees().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveEmployee(Employee EmployeeToSave)
        {
            context.Employees.Add(EmployeeToSave);
            context.SaveChanges();
        }
    }
}
