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
    public class EmployeeMatchingRepo : IEmployeeMatchingRepo
    {
        private EFDbContext context;
        public EmployeeMatchingRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteEmployeeMatchingById(int id)
        {
            context.EmployeeMatchings.Remove(GetAllEmployeeMatchings().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<EmployeeMatching> GetAllEmployeeMatchings()
        {
            return context.EmployeeMatchings.ToList();
        }

        public EmployeeMatching GetEmployeeMatchingById(int id)
        {
            return GetAllEmployeeMatchings().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveEmployeeMatching(EmployeeMatching EmployeeMatchingToSave)
        {
            context.EmployeeMatchings.Add(EmployeeMatchingToSave);
            context.SaveChanges();
        }
    }
}
