﻿using DataBase.Repository.Models;
using DataBase.Repository;
using CRUD.interfaces;
using Personal_Testing_System.DTOs;
using CRUD.implementations;

namespace Personal_Testing_System.Services
{
    public class SubdivisionService
    {
        private ISubdivisionRepo subdivisionRepo;
        public SubdivisionService(ISubdivisionRepo _subdivisionRepo)
        {
            this.subdivisionRepo = _subdivisionRepo;
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

        public void DeleteSubdivisionById(int id)
        {
            subdivisionRepo.DeleteSubdivisionById(id);
        }

        public List<Subdivision> GetAllSubdivisions()
        {
            return subdivisionRepo.GetAllSubdivisions();
        }

        public List<SubdivisionDto> GetAllSubdivisionDtos()
        {
            List<SubdivisionDto> Subdivisions = new List<SubdivisionDto>();
            subdivisionRepo.GetAllSubdivisions().ForEach(x => Subdivisions.Add(ConvertToSubdivisionDto(x)));
            return Subdivisions;
        }

        public Subdivision GetSubdivisionById(int id)
        {
            return subdivisionRepo.GetSubdivisionById(id);
        }

        public SubdivisionDto GetSubdivisionDtoById(int id)
        {
            return ConvertToSubdivisionDto(subdivisionRepo.GetSubdivisionById(id));
        }

        public void SaveSubdivision(Subdivision SubdivisionToSave)
        {
            subdivisionRepo.SaveSubdivision(SubdivisionToSave);
        }

        public void SaveSubdivisionDto(SubdivisionDto SubdivisionToSave)
        {
            subdivisionRepo.SaveSubdivision(ConvertToSubdivision(SubdivisionToSave));
        }
    }
}
