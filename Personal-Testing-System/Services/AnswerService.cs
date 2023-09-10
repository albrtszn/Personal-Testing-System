using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class AnswerService
    {
        private IAnswerRepo AnswerRepo;
        public AnswerService(IAnswerRepo _answerRepo)
        {
            this.AnswerRepo = _answerRepo;
        }
        private AnswerDto ConvertToAnswerDto(Answer answer)
        {
            return new AnswerDto
            {
                IdAnswer = answer.Id,
                IdQuestion = answer.IdQuestion,
                Text = answer.Text,
                Weight = answer.Weight,
                Number = Convert.ToInt32(answer.Number),
                ImagePath = answer.ImagePath,
                Correct = answer.Correct
            };
        }
        private Answer ConvertToAnswer(AnswerDto answerDto)
        {
            return new Answer
            {
                Id = answerDto.IdAnswer.Value,
                IdQuestion = answerDto.IdQuestion,
                Text = answerDto.Text,
                Weight= answerDto.Weight,
                Number = Convert.ToByte(answerDto.Number),
                ImagePath = answerDto.ImagePath,
                Correct = answerDto.Correct
            };
        }
        public async Task<bool> DeleteAnswerById(int id)
        {
            return await AnswerRepo.DeleteAnswerById(id);
        }

        public async Task<bool> DeleteAnswersByQuestion(string idQuestion)
        {
            List<Answer>list = (await GetAllAnswers()).Where(x => x.IdQuestion.Equals(idQuestion)).ToList();
            foreach (Answer answer in list)
            {
                await DeleteAnswerById(answer.Id);
            }
            return true;
        }

        public async Task<List<Answer>> GetAllAnswers()
        {
            return await AnswerRepo.GetAllAnswers();
        }

        public async Task<List<Answer>> GetAnswersByQuestionId(string id)
        {
            return (await GetAllAnswers()).Where(x => x.IdQuestion.Equals(id)).ToList();
        }

        public async Task<List<AnswerDto>> GetAllAnswerDtos()
        {
            List<AnswerDto> Answers = new List<AnswerDto>();
            List<Answer> list = await AnswerRepo.GetAllAnswers();
            list.ForEach(x => Answers.Add(ConvertToAnswerDto(x)));
            return Answers;
        }

        public async Task<List<AnswerDto>> GetAnswerDtosByQuestionId(string id)
        {
            return (await GetAllAnswerDtos()).Where(x => x.IdQuestion.Equals(id)).ToList();
        }

        public async Task<Answer> GetAnswerById(int id)
        {
            return await AnswerRepo.GetAnswerById(id);
        }

        public async Task<bool> SaveAnswer(Answer AnswerToSave)
        {
            return await AnswerRepo.SaveAnswer(AnswerToSave);
        }
    }
}
