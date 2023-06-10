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
    public class TestRepo : ITestRepo
    {
        private EFDbContext context;
        public TestRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteTestById(int id)
        {
            context.Tests.Remove(GetAllTests().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Test> GetAllTests()
        {
            return context.Tests.ToList();
        }

        public Test GetTestById(string id)
        {
            return GetAllTests().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveTest(Test TestToSave)
        {
            context.Tests.Add(TestToSave);
            context.SaveChanges();
        }
    }
}
