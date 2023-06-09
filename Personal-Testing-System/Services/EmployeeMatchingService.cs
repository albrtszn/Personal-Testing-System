using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class EmployeeMatchingService
    {
        private IEmployeeMatchingRepo employeeMatchingRepo;
        public EmployeeMatchingService(IEmployeeMatchingRepo _employeeMatchingRepo)
        {
            this.employeeMatchingRepo = _employeeMatchingRepo;
        }
        public void DeleteEmployeeMatchingById(int id)
        {
            employeeMatchingRepo.DeleteEmployeeMatchingById(id);
        }

        public List<EmployeeMatching> GetAllEmployeeMatchings()
        {
            return employeeMatchingRepo.GetAllEmployeeMatchings();
        }

        public EmployeeMatching GetEmployeeMatchingById(int id)
        {
            return employeeMatchingRepo.GetEmployeeMatchingById(id);
        }

        public void SaveEmployeeMatching(EmployeeMatching EmployeeMatchingToSave)
        {
            employeeMatchingRepo.SaveEmployeeMatching(EmployeeMatchingToSave);
        }
    }
}
