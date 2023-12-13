using CRUD.interfaces;
using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CRUD.implementations
{
    public class SubcompetenceRepo : ISubcompetenceRepo
    {
        private readonly EFDbContext context;
        public SubcompetenceRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteSubcompetenceById(int id)
        {
            context.Subcompetences.Remove((await GetAllSubcompetences()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Subcompetence>> GetAllSubcompetences()
        {
            return await context.Subcompetences.ToListAsync();
        }

        public async Task<Subcompetence> GetSubcompetenceById(int id)
        {
            return await context.Subcompetences.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveSubcompetence(Subcompetence SubcompetenceToSave)
        {
            Subcompetence? Subcompetence = await GetSubcompetenceById(SubcompetenceToSave.Id);
            //Subcompetence? Subcompetence1 = await context.Subcompetences.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(SubcompetenceToSave.Id));
            if (Subcompetence != null && SubcompetenceToSave.Id != 0)
            {
                /*context.Subcompetences.Entry(SubcompetenceToSave).State = EntityState.Detached;
                context.Set<Subcompetence>().Update(SubcompetenceToSave);*/
                Subcompetence.Name = SubcompetenceToSave.Name;
                Subcompetence.Description = SubcompetenceToSave.Description;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Subcompetences.AddAsync(SubcompetenceToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
