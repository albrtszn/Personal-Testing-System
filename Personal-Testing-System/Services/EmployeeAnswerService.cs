using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class EmployeeAnswerService
    {
        private IEmployeeAnswerRepo employeeAnswerRepo;
        public EmployeeAnswerService(IEmployeeAnswerRepo _employeeAnswerRepo)
        {
            this.employeeAnswerRepo = _employeeAnswerRepo;
        }
        public void DeleteEmployeeAnswerById(int id)
        {
            employeeAnswerRepo.DeleteEmployeeAnswerById(id);
        }

        public List<EmployeeAnswer> GetAllEmployeeAnswers()
        {
            return employeeAnswerRepo.GetAllEmployeeAnswers();
        }

        public EmployeeAnswer GetEmployeeAnswerById(int id)
        {
            return employeeAnswerRepo.GetEmployeeAnswerById(id);
        }

        public void SaveEmployeeAnswer(EmployeeAnswer EmployeeAnswerToSave)
        {
            employeeAnswerRepo.SaveEmployeeAnswer(EmployeeAnswerToSave);
        }
    }
}
