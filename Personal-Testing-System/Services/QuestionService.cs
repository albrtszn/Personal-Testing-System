using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class QuestionService
    {
        private IQuestionRepo QuestionRepo;
        public QuestionService(IQuestionRepo _questionRepo)
        {
            this.QuestionRepo = _questionRepo;
        }

        private QuestionDto ConvertToQuestionDto(Question quest)
        {
            return new QuestionDto
            {
                Id = quest.Id,
                Text = quest.Text,
                IdQuestionType = quest.IdQuestionType,
                Number = Convert.ToInt32(quest.Number),
                IdTest = quest.IdTest,
                ImagePath = quest.ImagePath
            };
        }

        private Question ConvertToQuestion(QuestionDto questDto)
        {
            return new Question
            {
                Id = questDto.Id,
                Text = questDto.Text,
                IdQuestionType = questDto.IdQuestionType,
                Number = Convert.ToByte(questDto.Number),
                IdTest = questDto.IdTest,
                ImagePath = questDto.ImagePath
            };
        }
        public async Task<bool> DeleteQuestionById(string id)
        {
            return await QuestionRepo.DeleteQuestionById(id);
        }

        public async Task<List<Question>> GetAllQuestions()
        {
            return await QuestionRepo.GetAllQuestions();
        }

        public async Task<List<QuestionDto>> GetAllQuestionDtos()
        {
            List<QuestionDto> Questions = new List<QuestionDto>();
            List<Question> list = await QuestionRepo.GetAllQuestions();
            list.ForEach(x => Questions.Add(ConvertToQuestionDto(x)));
            return Questions;
        }

        public async Task<List<Question>> GetQuestionsByTest(string idTest)
        {
            return (await GetAllQuestions()).Where(x => x.IdTest.Equals(idTest)).ToList();
        }

        public async Task<List<QuestionDto>> GetQuestionDtosByTest(string idTest)
        {
            List<QuestionDto> list = new List<QuestionDto>();
            (await GetAllQuestions()).Where(x => x.IdTest.Equals(idTest))
                .ToList()
                .ForEach(x=>list.Add(ConvertToQuestionDto(x)));
            return list;
        }

        public async Task<Question> GetQuestionById(string id)
        {
            return await QuestionRepo.GetQuestionById(id);
        }

        public async Task<bool> SaveQuestion(Question QuestionToSave)
        {
            return await QuestionRepo.SaveQuestion(QuestionToSave);
        }
    }
}
