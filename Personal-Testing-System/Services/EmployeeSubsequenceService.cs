using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class EmployeeSubsequenceService
    {
        private IEmployeeSubsequenceRepo employeeSubsequenceRepo;
        public EmployeeSubsequenceService(IEmployeeSubsequenceRepo _employeeSubsequenceRepo)
        {
            this.employeeSubsequenceRepo = _employeeSubsequenceRepo;
        }
        public void DeleteEmployeeSubsequenceById(int id)
        {
            employeeSubsequenceRepo.DeleteEmployeeSubsequenceById(id);
        }

        public List<EmployeeSubsequence> GetAllEmployeeSubsequences()
        {
            return employeeSubsequenceRepo.GetAllEmployeeSubsequences();
        }

        public EmployeeSubsequence GetEmployeeSubsequenceById(int id)
        {
            return employeeSubsequenceRepo.GetEmployeeSubsequenceById(id);
        }

        public void SaveEmployeeSubsequence(EmployeeSubsequence EmployeeSubsequenceToSave)
        {
            employeeSubsequenceRepo.SaveEmployeeSubsequence(EmployeeSubsequenceToSave);
        }
    }
}
