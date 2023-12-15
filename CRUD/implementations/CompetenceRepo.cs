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
    public class CompetenceRepo : ICompetenceRepo
    {
        private EFDbContext context;
        public CompetenceRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteCompetenceById(int id)
        {
            context.Competences.Remove((await GetAllCompetences()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Competence>> GetAllCompetences()
        {
            return await context.Competences.ToListAsync();
        }

        public async Task<Competence> GetCompetenceById(int id)
        {
            return await context.Competences.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }        
        public async Task<Competence> GetTrackCompetenceById(int id)
        {
            return await context.Competences.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveCompetence(Competence CompetenceToSave)
        {
            Competence? competence = await GetTrackCompetenceById(CompetenceToSave.Id);
            //Competence? Competence = await context.Competences.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(CompetenceToSave.Id));
            if (competence != null && CompetenceToSave.Id !=0)
            {
                /*context.Competences.Entry(CompetenceToSave).State = EntityState.Detached;
                context.Set<Competence>().Update(CompetenceToSave);*/
                competence.Name = CompetenceToSave.Name;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Competences.AddAsync(CompetenceToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }

    }
}
