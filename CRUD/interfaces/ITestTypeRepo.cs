using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ITestTypeRepo
    {
        List<TestType> GetAllTestTypes();
        TestType GetTestTypeById(int id);
        void SaveTestType(TestType TestTypeToSave);
        void DeleteTestTypeById(int id);
    }
}
