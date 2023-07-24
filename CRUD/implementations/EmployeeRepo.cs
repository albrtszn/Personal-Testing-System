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
        public void DeleteEmployeeById(string id)
        {
            context.Employees.Remove(GetAllEmployees().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Employee> GetAllEmployees()
        {
            return context.Employees.ToList();
        }

        public Employee GetEmployeeById(string id)
        {
            return GetAllEmployees().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveEmployee(Employee EmployeeToSave)
        {
            Employee? employee = GetEmployeeById(EmployeeToSave.Id);
            if (employee != null)
            {
                //context.Employees.Update(EmployeeToSave);
                employee.Id = EmployeeToSave.Id;
                employee.FirstName = EmployeeToSave.FirstName;
                employee.SecondName = EmployeeToSave.SecondName;
                employee.LastName = EmployeeToSave.LastName;
                employee.Login = EmployeeToSave.Login;
                employee.Password = EmployeeToSave.Password;
                employee.DateOfBirth = EmployeeToSave.DateOfBirth;
                employee.IdSubdivision = EmployeeToSave.IdSubdivision;
            }
            else
            {
                context.Employees.Add(EmployeeToSave);
            }
            context.SaveChanges();
        }
    }
}
