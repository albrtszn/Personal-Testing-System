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
        List<Test> GetAllTests();
        Test GetTestById(string id);
        void SaveTest(Test TestToSave);
        void DeleteTestById(int id);
    }
}
