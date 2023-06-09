using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class QuestionTypeService
    {
        private IQuestionTypeRepo questionTypeRepo;
        public QuestionTypeService(IQuestionTypeRepo _questionTypeRepo)
        {
            this.questionTypeRepo = _questionTypeRepo;
        }
        public void DeleteQuestionTypeById(int id)
        {
            questionTypeRepo.DeleteQuestionTypeById(id);
        }

        public List<QuestionType> GetAllQuestionTypes()
        {
            return questionTypeRepo.GetAllQuestionTypes();
        }

        public QuestionType GetQuestionTypeById(int id)
        {
            return questionTypeRepo.GetQuestionTypeById(id);
        }

        public void SaveQuestionType(QuestionType QuestionTypeToSave)
        {
            questionTypeRepo.SaveQuestionType(QuestionTypeToSave);
        }
    }
}
