using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<bool> DeleteSubdivisionById(int id)
        {
            context.Subdivisions.Remove((await GetAllSubdivisions()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Subdivision>> GetAllSubdivisions()
        {
            return await context.Subdivisions.ToListAsync();
        }

        public async Task<Subdivision> GetSubdivisionById(int id)
        {
            return await context.Subdivisions.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveSubdivision(Subdivision SubdivisionToSave)
        {
            Subdivision? Subdivision = await GetSubdivisionById(SubdivisionToSave.Id);
            //Subdivision? Subdivision = await context.Subdivisions.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(SubdivisionToSave.Id));
            if (Subdivision != null && SubdivisionToSave.Id != 0)
            {
                /*context.Subdivisions.Entry(SubdivisionToSave).State = EntityState.Detached;
                context.Set<Subdivision>().Update(SubdivisionToSave);*/
                Subdivision.Name = SubdivisionToSave.Name;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Subdivisions.AddAsync(SubdivisionToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
