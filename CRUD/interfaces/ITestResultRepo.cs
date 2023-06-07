using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ITestResultRepo
    {
        List<TestResult> GetAllTestResults();
        TestResult GetTestResultById(int id);
        void SaveTestResult(TestResult TestResultToSave);
        void DeleteTestResultById(int id);
    }
}
