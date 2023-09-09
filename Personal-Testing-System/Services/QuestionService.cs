using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class QuestionService
    {
        private IQuestionRepo questionRepo;
        public QuestionService(IQuestionRepo _questionRepo)
        {
            this.questionRepo = _questionRepo;
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

        public void DeleteQuestionById(string id)
        {
            questionRepo.DeleteQuestionById(id);
        }

        public List<Question> GetAllQuestions()
        {
            return questionRepo.GetAllQuestions();
        }

        public List<Question> GetQuestionsByTest(string idTest)
        {
            return GetAllQuestions().Where(x => x.IdTest.Equals(idTest)).ToList();
        }

        public List<QuestionDto> GetQuestionDtosByTest(string idTest)
        {
            List<QuestionDto> list = new List<QuestionDto>();
            GetAllQuestions().Where(x => x.IdTest.Equals(idTest))
                .ToList()
                .ForEach(x=>list.Add(ConvertToQuestionDto(x)));
            return list;
        }

        public Question GetQuestionById(string id)
        {
            return questionRepo.GetQuestionById(id);
        }

        public void SaveQuestion(Question QuestionToSave)
        {
            questionRepo.SaveQuestion(QuestionToSave);
        }
    }
}
