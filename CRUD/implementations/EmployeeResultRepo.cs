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
    public class EmployeeResultRepo : IEmployeeResultRepo
    {
        private EFDbContext context;
        public EmployeeResultRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteEmployeeResultById(int id)
        {
            context.EmployeeResults.Remove(GetAllEmployeeResults().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<EmployeeResult> GetAllEmployeeResults()
        {
            return context.EmployeeResults.ToList();
        }

        public EmployeeResult GetEmployeeResultById(string id)
        {
            return GetAllEmployeeResults().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveEmployeeResult(EmployeeResult EmployeeResultToSave)
        {
            context.EmployeeResults.Add(EmployeeResultToSave);
            context.SaveChanges();
        }
    }
}
