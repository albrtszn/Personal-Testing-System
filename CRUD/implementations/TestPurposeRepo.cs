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
    public class TestPurposeRepo : ITestPurposeRepo
    {
        private EFDbContext context;
        public TestPurposeRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteTestPurposeById(int id)
        {
            context.TestPurposes.Remove(GetAllTestPurposes().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<TestPurpose> GetAllTestPurposes()
        {
            return context.TestPurposes.ToList();
        }

        public TestPurpose GetTestPurposeById(int id)
        {
            return GetAllTestPurposes().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveTestPurpose(TestPurpose TestPurposeToSave)
        {
            TestPurpose? purpose = GetTestPurposeById(TestPurposeToSave.Id);
            if (purpose != null)
            {
                purpose.IdEmployee = TestPurposeToSave.IdEmployee;
                purpose.IdTest = TestPurposeToSave.IdTest;
                purpose.DatatimePurpose = TestPurposeToSave.DatatimePurpose;
            }
            else
            {
                context.TestPurposes.Add(TestPurposeToSave);
            }
            context.SaveChanges();
        }
    }
}
