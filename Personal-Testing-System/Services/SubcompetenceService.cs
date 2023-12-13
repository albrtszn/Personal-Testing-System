using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class SubcompetenceService
    {
        private ISubcompetenceRepo SubcompetenceRepo;
        public SubcompetenceService(ISubcompetenceRepo _SubcompetenceRepo)
        {
            this.SubcompetenceRepo = _SubcompetenceRepo;
        }
        private SubcompetenceDto ConvertToSubcompetenceDto(Subcompetence Subcompetence)
        {
            return new SubcompetenceDto
            {
                Id = Subcompetence.Id,
                Name = Subcompetence.Name,
                Description = Subcompetence.Description
            };
        }
        private Subcompetence ConvertToSubcompetence(SubcompetenceDto SubcompetenceDto)
        {
            return new Subcompetence
            {
                Id = SubcompetenceDto.Id.Value,
                Name = SubcompetenceDto.Name,
                Description = SubcompetenceDto.Description
            };
        }
        public async Task<bool> DeleteSubcompetenceById(int id)
        {
            return await SubcompetenceRepo.DeleteSubcompetenceById(id);
        }

        /*public async Task<bool> DeleteSubcompetencesByQuestion(string idQuestion)
        {
            List<Subcompetence> list = (await GetAllSubcompetences()).Where(x => x.IdQuestion.Equals(idQuestion)).ToList();
            foreach (Subcompetence Subcompetence in list)
            {
                await DeleteSubcompetenceById(Subcompetence.Id);
            }
            return true;
        }*/

        public async Task<List<Subcompetence>> GetAllSubcompetences()
        {
            return await SubcompetenceRepo.GetAllSubcompetences();
        }

        /*public async Task<List<Subcompetence>> GetSubcompetencesByQuestionId(string id)
        {
            return (await GetAllSubcompetences()).Where(x => x.IdQuestion.Equals(id)).ToList();
        }*/

        public async Task<List<SubcompetenceDto>> GetAllSubcompetenceDtos()
        {
            List<SubcompetenceDto> Subcompetences = new List<SubcompetenceDto>();
            List<Subcompetence> list = await SubcompetenceRepo.GetAllSubcompetences();
            list.ForEach(x => Subcompetences.Add(ConvertToSubcompetenceDto(x)));
            return Subcompetences;
        }

        /*public async Task<List<SubcompetenceDto>> GetSubcompetenceDtosByQuestionId(string id)
        {
            return (await GetAllSubcompetenceDtos()).Where(x => x.IdQuestion.Equals(id)).ToList();
        }*/

        public async Task<Subcompetence> GetSubcompetenceById(int id)
        {
            return await SubcompetenceRepo.GetSubcompetenceById(id);
        }
        public async Task<SubcompetenceDto?> GetSubcompetenceDtoById(int id)
        {
            Subcompetence subcompetence = await SubcompetenceRepo.GetSubcompetenceById(id);
            if (subcompetence == null)
                return null;
            return ConvertToSubcompetenceDto(subcompetence);
        }
        public async Task<bool> SaveSubcompetence(Subcompetence SubcompetenceToSave)
        {
            return await SubcompetenceRepo.SaveSubcompetence(SubcompetenceToSave);
        }
        public async Task<bool> SaveSubcompetence(AddSubcompetenceModel SubcompetenceToSave)
        {
            return await SubcompetenceRepo.SaveSubcompetence(new Subcompetence()
            {
                Name = SubcompetenceToSave.Name,
                Description = SubcompetenceToSave.Description
            });
        }
        public async Task<bool> SaveSubcompetence(SubcompetenceDto SubcompetenceToSave)
        {
            return await SubcompetenceRepo.SaveSubcompetence(ConvertToSubcompetence(SubcompetenceToSave));
        }
    }
}
