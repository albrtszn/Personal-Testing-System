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
    public class EmployeeSubsequenceRepo : IEmployeeSubsequenceRepo
    {
        private EFDbContext context;
        public EmployeeSubsequenceRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteEmployeeSubsequenceById(int id)
        {
            context.EmployeeSubsequences.Remove(GetAllEmployeeSubsequences().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<EmployeeSubsequence> GetAllEmployeeSubsequences()
        {
            return context.EmployeeSubsequences.ToList();
        }

        public EmployeeSubsequence GetEmployeeSubsequenceById(int id)
        {
            return GetAllEmployeeSubsequences().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveEmployeeSubsequence(EmployeeSubsequence EmployeeSubsequenceToSave)
        {
            context.EmployeeSubsequences.Add(EmployeeSubsequenceToSave);
            context.SaveChanges();
        }
    }
}
