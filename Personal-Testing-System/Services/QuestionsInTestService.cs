using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class QuestionsInTestService
    {
        private IQuestionsInTestRepo questionsInTestRepo;
        public QuestionsInTestService(IQuestionsInTestRepo _questionsInTestRepo)
        {
            this.questionsInTestRepo = _questionsInTestRepo;
        }
        public void DeleteTestById(int id)
        {
            questionsInTestRepo.DeleteQuestionInTestById(id);
        }

        public List<QuestionsInTest> GetAllQuestionsInTests()
        {
            return questionsInTestRepo.GetAllQuestionsInTests();
        }

        public QuestionsInTest GetQuestionsInTestById(int id)
        {
            return questionsInTestRepo.GetQuestionInTestById(id);
        }

        public void SaveQuestionsInTest(QuestionsInTest QuestionsInTestToSave)
        {
            questionsInTestRepo.SaveQustionInTest(QuestionsInTestToSave);
        }
    }
}
