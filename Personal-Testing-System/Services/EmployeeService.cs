using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class EmployeeService
    {
        private IEmployeeRepo employeeRepo;
        public EmployeeService(IEmployeeRepo _employeeRepo)
        {
            this.employeeRepo = _employeeRepo;
        }
        private EmployeeDto ConvertToEmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                SecondName = employee.SecondName,
                LastName = employee.LastName,
                Login = employee.Login,
                Password = employee.Password,
                DateOfBirth = employee.DateOfBirth.ToString(),
                IdSubdivision = employee.IdSubdivision
            };
        }
        private Employee ConvertToEmployee(EmployeeDto employeeDto)
        {
            return new Employee
            {
                Id = employeeDto.Id,
                FirstName = employeeDto.FirstName,
                SecondName = employeeDto.SecondName,
                LastName = employeeDto.LastName,
                Login = employeeDto.Login,
                Password = employeeDto.Password,
                DateOfBirth = DateOnly.Parse(employeeDto.DateOfBirth),
                IdSubdivision = employeeDto.IdSubdivision
            };
        }
        public void DeleteEmployeeById(string id)
        {
            employeeRepo.DeleteEmployeeById(id);
        }

        public List<Employee> GetAllEmployees()
        {
            return employeeRepo.GetAllEmployees();
        }

        public List<EmployeeDto> GetAllEmployeeDtos()
        {
            List<EmployeeDto> employeeDtos = new List<EmployeeDto>();
            GetAllEmployees().ForEach(x => employeeDtos.Add(ConvertToEmployeeDto(x)));
            return employeeDtos;
        }

        public Employee GetEmployeeById(string id)
        {
            return employeeRepo.GetEmployeeById(id);
        }

        public EmployeeDto GetEmployeeDtoById(string id)
        {
            return ConvertToEmployeeDto(employeeRepo.GetEmployeeById(id));
        }

        public void SaveEmployee(Employee EmployeeToSave)
        {
            employeeRepo.SaveEmployee(EmployeeToSave);
        }

        public void SaveEmployee(EmployeeDto EmployeeToSave)
        {
            employeeRepo.SaveEmployee(ConvertToEmployee(EmployeeToSave));
        }
    }
}
