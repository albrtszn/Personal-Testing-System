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
    public class TestResultRepo : ITestResultRepo
    {
        private EFDbContext context;
        public TestResultRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteTestResultById(int id)
        {
            context.TestResults.Remove(GetAllTestResults().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<TestResult> GetAllTestResults()
        {
            return context.TestResults.ToList();
        }

        public TestResult GetTestResultById(int id)
        {
            return GetAllTestResults().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveTestResult(TestResult TestResultToSave)
        {
            context.TestResults.Add(TestResultToSave);
            context.SaveChanges();
        }
    }
}
