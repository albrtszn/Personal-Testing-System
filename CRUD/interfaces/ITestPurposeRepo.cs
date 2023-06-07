using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ITestPurposeRepo
    {
        List<TestPurpose> GetAllTestPurposes();
        TestPurpose GetTestPurposeById(int id);
        void SaveTestPurpose(TestPurpose TestPurposeToSave);
        void DeleteTestPurposeById(int id);
    }
}
