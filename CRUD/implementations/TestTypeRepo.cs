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
    public class TestTypeRepo : ITestTypeRepo
    {
        private EFDbContext context;
        public TestTypeRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteTestTypeById(int id)
        {
            context.TestTypes.Remove(GetAllTestTypes().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<TestType> GetAllTestTypes()
        {
            return context.TestTypes.ToList();
        }

        public TestType GetTestTypeById(int id)
        {
            return GetAllTestTypes().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveTestType(TestType TestTypeToSave)
        {
            context.TestTypes.Add(TestTypeToSave);
            context.SaveChanges();
        }
    }
}
