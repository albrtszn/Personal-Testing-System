using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class EmployeeService
    {
        private IEmployeeRepo employeeRepo;
        private SubdivisionService subdivisionRepo;
        public EmployeeService(IEmployeeRepo _employeeRepo, SubdivisionService _subdivisionRepo)
        {
            this.employeeRepo = _employeeRepo;
            this.subdivisionRepo = _subdivisionRepo;
        }
        //DTO
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
        //Model
        private EmployeeModel ConvertToEmployeeModel(Employee employee)
        {
            SubdivisionDto subdivision = subdivisionRepo.GetSubdivisionDtoById(employee.IdSubdivision.Value);
            return new EmployeeModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                SecondName = employee.SecondName,
                LastName = employee.LastName,
                Login = employee.Login,
                Password = employee.Password,
                DateOfBirth = employee.DateOfBirth.ToString(),
                Subdivision = subdivision
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

        public List<EmployeeModel> GetAllEmployeeModels()
        {
            List<EmployeeModel> employeeModels = new List<EmployeeModel>();
            GetAllEmployees().ForEach(x => employeeModels.Add(ConvertToEmployeeModel(x)));
            return employeeModels;
        }

        public Employee GetEmployeeById(string id)
        {
            return employeeRepo.GetEmployeeById(id);
        }

        public EmployeeDto GetEmployeeDtoById(string id)
        {
            return ConvertToEmployeeDto(employeeRepo.GetEmployeeById(id));
        }

        public EmployeeModel GetEmployeeModelById(string id)
        {
            return ConvertToEmployeeModel(employeeRepo.GetEmployeeById(id));
        }

        public void SaveEmployee(Employee EmployeeToSave)
        {
            EmployeeToSave.Id = Guid.NewGuid().ToString();
            employeeRepo.SaveEmployee(EmployeeToSave);
        }

        public void SaveEmployee(EmployeeDto EmployeeToSave)
        {
            EmployeeToSave.Id = Guid.NewGuid().ToString();
            employeeRepo.SaveEmployee(ConvertToEmployee(EmployeeToSave));
        }
    }
}
