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
    public class SubcompetenceScoreRepo : ISubcompetenceScoreRepo
    {
        private readonly EFDbContext context;
        public SubcompetenceScoreRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteSubcompetenceScoreById(int id)
        {
            context.SubcompetenceScores.Remove((await GetAllSubcompetenceScores()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SubcompetenceScore>> GetAllSubcompetenceScores()
        {
            return await context.SubcompetenceScores.ToListAsync();
        }

        public async Task<SubcompetenceScore> GetSubcompetenceScoreById(int id)
        {
            return await context.SubcompetenceScores.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        public async Task<SubcompetenceScore> GetTrackSubcompetenceScoreById(int id)
        {
            return await context.SubcompetenceScores.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveSubcompetenceScore(SubcompetenceScore SubcompetenceScoreToSave)
        {
            SubcompetenceScore? SubcompetenceScore = await GetTrackSubcompetenceScoreById(SubcompetenceScoreToSave.Id);
            //SubcompetenceScore? SubcompetenceScore = await context.SubcompetenceScores.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(SubcompetenceScoreToSave.Id));
            if (SubcompetenceScore != null && SubcompetenceScoreToSave.Id != 0)
            {
                /*context.SubcompetenceScores.Entry(SubcompetenceScoreToSave).State = EntityState.Detached;
                context.Set<SubcompetenceScore>().Update(SubcompetenceScoreToSave);*/
                SubcompetenceScore.MinValue = SubcompetenceScoreToSave.MinValue;
                SubcompetenceScore.MaxValue = SubcompetenceScoreToSave.MaxValue;
                SubcompetenceScore.Description = SubcompetenceScoreToSave.Description;
                SubcompetenceScore.IdSubcompetence = SubcompetenceScoreToSave.IdSubcompetence;
                SubcompetenceScore.NumberPoints = SubcompetenceScoreToSave.NumberPoints;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.SubcompetenceScores.AddAsync(SubcompetenceScoreToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
