﻿using CRUD.implementations;
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
                Number = Convert.ToByte(answerDto.Number),
                ImagePath = answerDto.ImagePath,
                Correct = answerDto.Correct
            };
        }
        public void DeleteAnswerById(int id)
        {
            answerRepo.DeleteAnswerById(id);
        }

        public void DeleteAnswersByQuestion(string idQuestion)
        {
            GetAllAnswers().Where(x => x.IdQuestion.Equals(idQuestion))
                           .ToList()
                           .ForEach(x => DeleteAnswerById(x.Id));
        }

        public List<Answer> GetAllAnswers()
        {
            return answerRepo.GetAllAnswers();
        }

        public List<Answer> GetAnswersByQuestionId(string id)
        {
            return GetAllAnswers().Where(x => x.IdQuestion.Equals(id)).ToList();
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
