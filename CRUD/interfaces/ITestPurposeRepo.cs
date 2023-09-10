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
        Task<List<TestPurpose>> GetAllTestPurposes();
        Task<TestPurpose> GetTestPurposeById(int id);
        Task<bool> SaveTestPurpose(TestPurpose TestPurposeToSave);
        Task<bool> DeleteTestPurposeById(int id);
    }
}
