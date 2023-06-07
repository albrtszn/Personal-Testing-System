using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class EmployeeAnswerRepo : IEmployeeAnswerRepo
    {
        private EFDbContext context;
        public EmployeeAnswerRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteEmployeeAnswerById(int id)
        {
            context.EmployeeAnswers.Remove(GetAllEmployeeAnswers().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<EmployeeAnswer> GetAllEmployeeAnswers()
        {
            return context.EmployeeAnswers.ToList();
        }

        public EmployeeAnswer GetEmployeeAnswerById(int id)
        {
            return GetAllEmployeeAnswers().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveEmployeeAnswer(EmployeeAnswer EmployeeAnswerToSave)
        {
            context.EmployeeAnswers.Add(EmployeeAnswerToSave);
            context.SaveChanges();
        }
    }
}
