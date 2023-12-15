using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class CompetenciesForGroupRepo : ICompetenciesForGroupRepo
    {
        private EFDbContext context;
        public CompetenciesForGroupRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteCompetenciesForGroupById(int id)
        {
            context.CompetenciesForGroups.Remove((await GetAllCompetenciesForGroups()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CompetenciesForGroup>> GetAllCompetenciesForGroups()
        {
            return await context.CompetenciesForGroups.ToListAsync();
        }

        public async Task<CompetenciesForGroup> GetCompetenciesForGroupById(int id)
        {
            return await context.CompetenciesForGroups.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }       
        public async Task<CompetenciesForGroup> GettrackCompetenciesForGroupById(int id)
        {
            return await context.CompetenciesForGroups.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveCompetenciesForGroup(CompetenciesForGroup CompetenciesForGroupToSave)
        {
            CompetenciesForGroup? CompetenciesForGroup = await GettrackCompetenciesForGroupById(CompetenciesForGroupToSave.Id);
            //CompetenciesForGroup? CompetenciesForGroup = await context.CompetenciesForGroups.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(CompetenciesForGroupToSave.Id));
            if (CompetenciesForGroup != null && CompetenciesForGroupToSave.Id != 0)
            {
                /*context.CompetenciesForGroups.Entry(CompetenciesForGroupToSave).State = EntityState.Detached;
                context.Set<CompetenciesForGroup>().Update(CompetenciesForGroupToSave);*/
                CompetenciesForGroup.IdTest = CompetenciesForGroupToSave.IdTest;
                CompetenciesForGroup.IdGroupPositions = CompetenciesForGroupToSave.IdGroupPositions;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.CompetenciesForGroups.AddAsync(CompetenciesForGroupToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
