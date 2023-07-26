using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class AnswerService
    {
        private IAnswerRepo answerRepo;
        public AnswerService(IAnswerRepo _answerRepo)
        {
            this.answerRepo = _answerRepo;
        }
        private AnswerDto ConvertToAnswerDto(Answer answer)
        {
            return new AnswerDto
            {
                IdAnswer = answer.Id,
                IdQuestion = answer.IdQuestion,
                Text = answer.Text,
                ImagePath = answer.ImagePath,
                Correct = answer.Correct
            };
        }
        private Answer ConvertToAnswer(AnswerDto answerDto)
        {
            return new Answer
            {
                Id = (int)answerDto.IdAnswer,
                IdQuestion = answerDto.IdQuestion,
                Text = answerDto.Text,  
                ImagePath= answerDto.ImagePath,
                Correct = answerDto.Correct
            };
        }
        public void DeleteAnswerById(int id)
        {
            answerRepo.DeleteAnswerById(id);
        }

        public List<Answer> GetAllAnswers()
        {
            return answerRepo.GetAllAnswers();
        }

        public List<AnswerDto> GetAllAnswerDtos()
        {
            List<AnswerDto> answers =  new List<AnswerDto>();
            answerRepo.GetAllAnswers().ForEach(x=>answers.Add(ConvertToAnswerDto(x)));
            return answers;
        }

        public List<AnswerDto> GetAnswerDtosByQuestionId(string id)
        {
            return GetAllAnswerDtos().Where(x => x.IdQuestion.Equals(id)).ToList();
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
