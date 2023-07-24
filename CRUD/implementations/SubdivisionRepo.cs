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
    public class SubdivisionRepo : ISubdivisionRepo
    {
        private EFDbContext context;
        public SubdivisionRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteSubdivisionById(int id)
        {
            context.Subdivisions.Remove(GetAllSubdivisions().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Subdivision> GetAllSubdivisions()
        {
            return context.Subdivisions.ToList();
        }

        public Subdivision GetSubdivisionById(int id)
        {
            return GetAllSubdivisions().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveSubdivision(Subdivision SubdivisionToSave)
        {
            if (SubdivisionToSave.Id != 0 && GetSubdivisionById(SubdivisionToSave.Id) != null)
            {
                //context.Subdivisions.Update(SubdivisionToSave);
                GetSubdivisionById(SubdivisionToSave.Id).Name = SubdivisionToSave.Name;
            }
            else
            {
                context.Subdivisions.Add(SubdivisionToSave);
            }
            context.SaveChanges();
        }
    }
}
