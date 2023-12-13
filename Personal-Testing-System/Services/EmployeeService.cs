using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Microsoft.IdentityModel.Tokens;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class EmployeeService
    {
        private IEmployeeRepo EmployeeRepo;
        private SubdivisionService subdivisionRepo;
        public EmployeeService(IEmployeeRepo _employeeRepo, SubdivisionService _subdivisionRepo)
        {
            this.EmployeeRepo = _employeeRepo;
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
                IdSubdivision = employee.IdSubdivision,
                Phone = employee.Phone,
                RegistrationDate = employee.RegistrationDate.ToString()
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
                IdSubdivision = employeeDto.IdSubdivision,
                Phone = employeeDto.Phone,
                RegistrationDate = DateOnly.Parse(employeeDto.RegistrationDate)
            };
        }
        //Model
        private async Task<EmployeeModel?> ConvertToEmployeeModel(Employee employee)
        {
            SubdivisionDto? subdivision = null;
            if (employee != null && employee.IdSubdivision != null)
            {
                subdivision = await subdivisionRepo.GetSubdivisionDtoById(employee.IdSubdivision.Value);
            }
            else
            {
                return null;
            }
            return new EmployeeModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                SecondName = employee.SecondName,
                LastName = employee.LastName,
                Login = employee.Login,
                Password = employee.Password,
                DateOfBirth = employee.DateOfBirth.ToString(),
                Subdivision = subdivision,
                Phone= employee.Phone,
                RegistrationDate = employee.RegistrationDate.ToString()
            };
        }

        private Employee ConvertToEmployee(AddEmployeeModel employeeDto)
        {
            return new Employee
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = employeeDto.FirstName,
                SecondName = employeeDto.SecondName,
                LastName = employeeDto.LastName,
                Login = employeeDto.Login,
                Password = employeeDto.Password,
                DateOfBirth = DateOnly.Parse(employeeDto.DateOfBirth),
                IdSubdivision = employeeDto.IdSubdivision,
                Phone = employeeDto.Phone,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
        };
        }

        private Employee ConvertToEmployee(UpdateEmployeeModel employeeDto)
        {
            string employeeId = Guid.NewGuid().ToString();
            if (employeeDto!=null && !employeeDto.Id.IsNullOrEmpty())
            {
                employeeId = employeeDto.Id;
            }
            return new Employee
            {
                Id = employeeId,
                FirstName = employeeDto.FirstName,
                SecondName = employeeDto.SecondName,
                LastName = employeeDto.LastName,
                Login = employeeDto.Login,
                Password = employeeDto.Password,
                DateOfBirth = DateOnly.Parse(employeeDto.DateOfBirth),
                IdSubdivision = employeeDto.IdSubdivision,
                Phone = employeeDto.Phone,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
            };
        }

        public async Task<bool> DeleteEmployeeById(string id)
        {
            return await EmployeeRepo.DeleteEmployeeById(id);
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await EmployeeRepo.GetAllEmployees();
        }

        public async Task<List<EmployeeDto>> GetAllEmployeeDtos()
        {
            List<EmployeeDto> Employees = new List<EmployeeDto>();
            List<Employee> list = await EmployeeRepo.GetAllEmployees();
            list.ForEach(x => Employees.Add(ConvertToEmployeeDto(x)));
            return Employees;
        }

        public async Task<List<EmployeeModel>> GetAllEmployeeModels()
        { 
            List<EmployeeModel> Employees = new List<EmployeeModel>();
            List<Employee> list = await EmployeeRepo.GetAllEmployees();
            foreach (Employee empl in list)
            {
                Employees.Add(await ConvertToEmployeeModel(empl));
            }
            return Employees;
        }

        public async Task<Employee> GetEmployeeById(string id)
        {
            return await EmployeeRepo.GetEmployeeById(id);
        }

        public async Task<EmployeeDto> GetEmployeeDtoById(string id)
        {
            return ConvertToEmployeeDto(await EmployeeRepo.GetEmployeeById(id));
        }

        public async Task<EmployeeModel> GetEmployeeModelById(string id)
        {
            return await ConvertToEmployeeModel(await EmployeeRepo.GetEmployeeById(id));
        }

        public async Task<bool> SaveEmployee(Employee EmployeeToSave)
        {
            return await EmployeeRepo.SaveEmployee(EmployeeToSave);
        }

        public async Task<bool> SaveEmployee(EmployeeDto EmployeeDtoToSave)
        {
            return await EmployeeRepo.SaveEmployee(ConvertToEmployee(EmployeeDtoToSave));
        }

        public async Task<bool> SaveEmployee(AddEmployeeModel EmployeeDtoToAdd)
        {
            return await EmployeeRepo.SaveEmployee(ConvertToEmployee(EmployeeDtoToAdd));
        }

        public async Task<bool> SaveEmployee(UpdateEmployeeModel EmployeeToSave)
        {
            return await EmployeeRepo.SaveEmployee(ConvertToEmployee(EmployeeToSave));
        }
    }
}
