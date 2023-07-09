using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class TestResultService
    {
        private IResultRepo resultRepo;
        public TestResultService(IResultRepo _testResultRepo)
        {
            this.resultRepo = _testResultRepo;
        }
        public void DeleteById(int id)
        {
            resultRepo.DeleteResultById(id);
        }

        public List<Result> GetAllResults()
        {
            return resultRepo.GetAllResults(); 
        }

        public Result GetResultById(int id)
        {
            return resultRepo.GetResultById(id);
        }

        public void SaveResult(Result ResultToSave)
        {
            resultRepo.SaveResult(ResultToSave);
        }
    }
}
