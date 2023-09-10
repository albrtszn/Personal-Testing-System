using DataBase.Repository.Models;
using DataBase.Repository;
using CRUD.interfaces;
using Personal_Testing_System.DTOs;
using CRUD.implementations;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class SubdivisionService
    {
        private ISubdivisionRepo SubdivisionRepo;
        public SubdivisionService(ISubdivisionRepo _subdivisionRepo)
        {
            this.SubdivisionRepo = _subdivisionRepo;
        }

        private Subdivision ConvertToSubdivision(SubdivisionDto subDto)
        {
            return new Subdivision
            {
                Id = subDto.Id.Value,
                Name = subDto.Name
            };
        }

        private SubdivisionDto ConvertToSubdivisionDto(Subdivision sub)
        {
            return new SubdivisionDto
            {
                Id = sub.Id,
                Name = sub.Name
            };
        }

        public async Task<bool> DeleteSubdivisionById(int id)
        {
            return await SubdivisionRepo.DeleteSubdivisionById(id);
        }

        public async Task<List<Subdivision>> GetAllSubdivisions()
        {
            return await SubdivisionRepo.GetAllSubdivisions();
        }

        public async Task<List<SubdivisionDto>> GetAllSubdivisionDtos()
        {
            List<SubdivisionDto> Subdivisions = new List<SubdivisionDto>();
            List<Subdivision> list = await SubdivisionRepo.GetAllSubdivisions();
            list.ForEach(x => Subdivisions.Add(ConvertToSubdivisionDto(x)));
            return Subdivisions;
        }

        public async Task<Subdivision> GetSubdivisionById(int id)
        {
            return await SubdivisionRepo.GetSubdivisionById(id);
        }

        public async Task<SubdivisionDto> GetSubdivisionDtoById(int id)
        {
            return ConvertToSubdivisionDto(await SubdivisionRepo.GetSubdivisionById(id));
        }

        public async Task<bool> SaveSubdivision(Subdivision SubdivisionToSave)
        {
            return await SubdivisionRepo.SaveSubdivision(SubdivisionToSave);
        }

        public async Task<bool> SaveSubdivision(SubdivisionDto SubdivisionDtoToSave)
        {
            return await SubdivisionRepo.SaveSubdivision(ConvertToSubdivision(SubdivisionDtoToSave));
        }

        public async Task<bool> SaveSubdivision(AddSubdivisionModel SubdivisionToSave)
        {
            await SubdivisionRepo.SaveSubdivision(new Subdivision
            {
                Id=SubdivisionToSave.Id.Value,
                Name = SubdivisionToSave.Name
            });
            return true;
        }

        public async Task<bool> SaveSubdivision(SubdivisionModel SubdivisionToSave)
        {
            await SubdivisionRepo.SaveSubdivision(new Subdivision
            {
                Name = SubdivisionToSave.Name
            });
            return true;

        }
    }
}
