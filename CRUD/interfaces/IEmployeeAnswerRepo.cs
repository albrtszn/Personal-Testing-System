using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IEmployeeAnswerRepo
    {
        Task<List<EmployeeAnswer>> GetAllEmployeeAnswers();
        Task<EmployeeAnswer> GetEmployeeAnswerById(int id);
        Task<bool> SaveEmployeeAnswer(EmployeeAnswer EmployeeAnswerToSave);
        Task<bool> DeleteEmployeeAnswerById(int id);
    }
}
