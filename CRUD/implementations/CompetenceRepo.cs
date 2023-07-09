using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class CompetenceRepo : ICompetenceRepo
    {
        private EFDbContext context;
        public CompetenceRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteCompetenceById(int id)
        {
            context.Competences.Remove(GetAllCompetences().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Competence> GetAllCompetences()
        {
            return context.Competences.ToList();
        }

        public Competence GetCompetenceById(int id)
        {
            return GetAllCompetences().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveCompetence(Competence CompetenceToSave)
        {
            context.Competences.Add(CompetenceToSave);
            context.SaveChanges();
        }
    }
}
