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
    public class CompetenceScoreRepo : ICompetenceScoreRepo
    {
        private readonly EFDbContext context;
        public CompetenceScoreRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteCompetenceScoreById(int id)
        {
            context.CompetenceScores.Remove((await GetAllCompetenceScores()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CompetenceScore>> GetAllCompetenceScores()
        {
            return await context.CompetenceScores.ToListAsync();
        }

        public async Task<CompetenceScore?> GetCompetenceScoreById(int id)
        {
            return await context.CompetenceScores.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        public async Task<CompetenceScore?> GetTrackCompetenceScoreById(int id)
        {
            return await context.CompetenceScores.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveCompetenceScore(CompetenceScore CompetenceScoreToSave)
        {
            CompetenceScore? CompetenceScore = await GetTrackCompetenceScoreById(CompetenceScoreToSave.Id);
            //CompetenceScore? CompetenceScore = await context.CompetenceScores.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(CompetenceScoreToSave.Id));
            if (CompetenceScore != null && CompetenceScoreToSave.Id != 0)
            {
                /*context.CompetenceScores.Entry(CompetenceScoreToSave).State = EntityState.Detached;
                context.Set<CompetenceScore>().Update(CompetenceScoreToSave);*/
                CompetenceScore.IdCompetence = CompetenceScoreToSave.IdCompetence;
                CompetenceScore.IdGroup = CompetenceScoreToSave.IdGroup;
                CompetenceScore.Name = CompetenceScoreToSave.Name;
                CompetenceScore.NumnerPoints = CompetenceScoreToSave.NumnerPoints;
                CompetenceScore.Description = CompetenceScoreToSave.Description;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.CompetenceScores.AddAsync(CompetenceScoreToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
