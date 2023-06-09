using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class AnswerService
    {
        private IAnswerRepo answerRepo;
        public AnswerService(IAnswerRepo _answerRepo)
        {
            this.answerRepo = _answerRepo;
        }
        public void DeleteAnswerById(int id)
        {
            answerRepo.DeleteAnswerById(id);
        }

        public List<Answer> GetAllAnswers()
        {
            return answerRepo.GetAllAnswers();
        }

        public Answer GetAnswerById(int id)
        {
            return answerRepo.GetAnswerById(id);
        }

        public void SaveAnswer(Answer AnswerToSave)
        {
            answerRepo.SaveAnswer(AnswerToSave);
        }
    }
}
