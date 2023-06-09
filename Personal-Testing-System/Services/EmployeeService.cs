using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class EmployeeService
    {
        private IEmployeeRepo employeeRepo;
        public EmployeeService(IEmployeeRepo _employeeRepo)
        {
            this.employeeRepo = _employeeRepo;
        }
        public void DeleteEmployeeById(int id)
        {
            employeeRepo.DeleteEmployeeById(id);
        }

        public List<Employee> GetAllEmployees()
        {
            return employeeRepo.GetAllEmployees();
        }

        public Employee GetEmployeeById(int id)
        {
            return employeeRepo.GetEmployeeById(id);
        }

        public void SaveEmployee(Employee EmployeeToSave)
        {
            employeeRepo.SaveEmployee(EmployeeToSave);
        }
    }
}
