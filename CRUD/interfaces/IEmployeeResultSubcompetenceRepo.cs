using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IEmployeeResultSubcompetenceRepo
    {
        Task<List<ElployeeResultSubcompetence>> GetAllEmployeeResultSubcompetences();
        Task<ElployeeResultSubcompetence> GetEmployeeResultSubcompetenceById(int id);
        Task<bool> SaveEmployeeResultSubcompetence(ElployeeResultSubcompetence EmployeeResultSubcompetenceToSave);
        Task<bool> DeleteEmployeeResultSubcompetenceById(int id);
    }
}
