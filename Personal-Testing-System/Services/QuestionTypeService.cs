using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;

namespace Personal_Testing_System.Services
{
    public class QuestionTypeService
    {
        private IQuestionTypeRepo QuestionTypeRepo;
        public QuestionTypeService(IQuestionTypeRepo _questionTypeRepo)
        {
            this.QuestionTypeRepo = _questionTypeRepo;
        }
        public async Task<bool> DeleteQuestionTypeById(int id)
        {
            return await QuestionTypeRepo.DeleteQuestionTypeById(id);
        }

        public async Task<List<QuestionType>> GetAllQuestionTypes()
        {
            return await QuestionTypeRepo.GetAllQuestionTypes();
        }

        public async Task<QuestionType> GetQuestionTypeById(int id)
        {
            return await QuestionTypeRepo.GetQuestionTypeById(id);
        }

        public async Task<bool> SaveQuestionType(QuestionType QuestionTypeToSave)
        {
            return await QuestionTypeRepo.SaveQuestionType(QuestionTypeToSave);
        }
    }
}
