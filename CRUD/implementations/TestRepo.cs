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
        public void DeleteTestById(string id)
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
            Test? test = GetTestById(TestToSave.Id);
            if (test!=null && !string.IsNullOrEmpty(TestToSave.Id) )
            {
                //context.Tests.Update(TestToSave);
                test.Name = TestToSave.Name;
                test.IdCompetence = TestToSave.IdCompetence;
                test.Weight = TestToSave.Weight;
            }
            else
            {
                context.Tests.Add(TestToSave);
            }
            context.SaveChanges();
        }
    }
}
