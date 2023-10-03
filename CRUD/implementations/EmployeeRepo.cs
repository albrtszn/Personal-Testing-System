using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<bool> DeleteEmployeeById(string id)
        {
            context.Employees.Remove((await GetAllEmployees()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(string id)
        {
            return await context.Employees.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveEmployee(Employee EmployeeToSave)
        {
            Employee? employee = await GetEmployeeById(EmployeeToSave.Id);
            //Employee? Employee = await context.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(EmployeeToSave.Id));
            if (employee != null && !EmployeeToSave.Id.IsNullOrEmpty())
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
                employee.Phone = EmployeeToSave.Phone;
                employee.RegistrationDate = EmployeeToSave.RegistrationDate;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Employees.AddAsync(EmployeeToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
