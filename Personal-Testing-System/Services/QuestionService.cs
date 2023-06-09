using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class QuestionService
    {
        private IQuestionRepo questionRepo;
        public QuestionService(IQuestionRepo _questionRepo)
        {
            this.questionRepo = _questionRepo;
        }
        public void DeleteQuestionById(int id)
        {
            questionRepo.DeleteQuestionById(id);
        }

        public List<Question> GetAllQuestions()
        {
            return questionRepo.GetAllQuestions();
        }

        public Question GetQuestionById(int id)
        {
            return questionRepo.GetByQuestionId(id);
        }

        public void SaveQuestion(Question QuestionToSave)
        {
            questionRepo.SaveQuestion(QuestionToSave);
        }
    }
}
