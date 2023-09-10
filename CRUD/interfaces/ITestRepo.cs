using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ITestRepo
    {
        Task<List<Test>> GetAllTests();
        Task<Test> GetTestById(string id);
        Task<bool> SaveTest(Test TestToSave);
        Task<bool> DeleteTestById(string id);
    }
}
