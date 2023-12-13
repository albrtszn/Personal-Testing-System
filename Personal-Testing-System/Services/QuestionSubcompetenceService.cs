using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class QuestionSubcompetenceService
    {
        private IQuestionSubcompetenceRepo QuestionSubcompetenceRepo;
        public QuestionSubcompetenceService(IQuestionSubcompetenceRepo _QuestionSubcompetenceRepo)
        {
            this.QuestionSubcompetenceRepo = _QuestionSubcompetenceRepo;
        }
        private QuestionSubcompetenceDto ConvertToQuestionSubcompetenceDto(QuestionSubcompetence QuestionSubcompetence)
        {
            return new QuestionSubcompetenceDto
            {
                Id = QuestionSubcompetence.Id,
                IdQuestion = QuestionSubcompetence.IdQuestion,
                IdSubcompetence = QuestionSubcompetence.IdSubcompetence
            };
        }
        private QuestionSubcompetence ConvertToQuestionSubcompetence(QuestionSubcompetenceDto QuestionSubcompetenceDto)
        {
            return new QuestionSubcompetence
            {
                Id = QuestionSubcompetenceDto.Id.Value,
                IdQuestion = QuestionSubcompetenceDto.IdQuestion,
                IdSubcompetence = QuestionSubcompetenceDto.IdSubcompetence
            };
        }
        public async Task<bool> DeleteQuestionSubcompetenceById(int id)
        {
            return await QuestionSubcompetenceRepo.DeleteQuestionSubcompetenceById(id);
        }

        /*public async Task<bool> DeleteQuestionSubcompetencesByQuestion(string idQuestion)
        {
            List<QuestionSubcompetence> list = (await GetAllQuestionSubcompetences()).Where(x => x.IdQuestion.Equals(idQuestion)).ToList();
            foreach (QuestionSubcompetence QuestionSubcompetence in list)
            {
                await DeleteQuestionSubcompetenceById(QuestionSubcompetence.Id);
            }
            return true;
        }*/

        public async Task<List<QuestionSubcompetence>> GetAllQuestionSubcompetences()
        {
            return await QuestionSubcompetenceRepo.GetAllQuestionSubcompetences();
        }

        /*public async Task<List<QuestionSubcompetence>> GetQuestionSubcompetencesByQuestionId(string id)
        {
            return (await GetAllQuestionSubcompetences()).Where(x => x.IdQuestion.Equals(id)).ToList();
        }*/

        public async Task<List<QuestionSubcompetenceDto>> GetAllQuestionSubcompetenceDtos()
        {
            List<QuestionSubcompetenceDto> QuestionSubcompetences = new List<QuestionSubcompetenceDto>();
            List<QuestionSubcompetence> list = await QuestionSubcompetenceRepo.GetAllQuestionSubcompetences();
            list.ForEach(x => QuestionSubcompetences.Add(ConvertToQuestionSubcompetenceDto(x)));
            return QuestionSubcompetences;
        }

        /*public async Task<List<QuestionSubcompetenceDto>> GetQuestionSubcompetenceDtosByQuestionId(string id)
        {
            return (await GetAllQuestionSubcompetenceDtos()).Where(x => x.IdQuestion.Equals(id)).ToList();
        }*/

        public async Task<QuestionSubcompetence> GetQuestionSubcompetenceById(int id)
        {
            return await QuestionSubcompetenceRepo.GetQuestionSubcompetenceById(id);
        }
        public async Task<QuestionSubcompetenceDto?> GetQuestionSubcompetenceDtoById(int id)
        {
            QuestionSubcompetence model = await QuestionSubcompetenceRepo.GetQuestionSubcompetenceById(id);
            if (model == null)
                return null;
            return ConvertToQuestionSubcompetenceDto(model);
        }

        public async Task<QuestionSubcompetence?> GetQuestionSubcompetenceByQuestionId(string id)
        {
            return (await GetAllQuestionSubcompetences()).ToList().FirstOrDefault(x => x != null && x.IdQuestion != null && x.IdQuestion.Equals(id));
        }

        public async Task<bool> SaveQuestionSubcompetence(QuestionSubcompetence QuestionSubcompetenceToSave)
        {
            return await QuestionSubcompetenceRepo.SaveQuestionSubcompetence(QuestionSubcompetenceToSave);
        }
        public async Task<bool> SaveQuestionSubcompetence(AddQuestionSubcompetenceModel QuestionSubcompetenceToSave)
        {
            return await QuestionSubcompetenceRepo.SaveQuestionSubcompetence(new QuestionSubcompetence()
            {
                IdQuestion = QuestionSubcompetenceToSave.IdQuestion,
                IdSubcompetence = QuestionSubcompetenceToSave.IdSubcompetence
            });
        }
        public async Task<bool> SaveQuestionSubcompetence(QuestionSubcompetenceDto QuestionSubcompetenceToSave)
        {
            return await QuestionSubcompetenceRepo.SaveQuestionSubcompetence(ConvertToQuestionSubcompetence(QuestionSubcompetenceToSave));
        }

    }
}
