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
    public class CompetenceCoeffRepo : ICompetenceCoeffRepo
    {
        private readonly EFDbContext context;
        public CompetenceCoeffRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteCompetenceCoeffById(int id)
        {
            context.СompetenceСoeffs.Remove((await GetAllCompetenceCoeffs()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<СompetenceСoeff>> GetAllCompetenceCoeffs()
        {
            return await context.СompetenceСoeffs.ToListAsync();
        }

        public async Task<СompetenceСoeff> GetCompetenceCoeffById(int id)
        {
            return await context.СompetenceСoeffs.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        public async Task<СompetenceСoeff> GetTrackCompetenceCoeffById(int id)
        {
            return await context.СompetenceСoeffs.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveCompetenceCoeff(СompetenceСoeff CompetenceCoeffToSave)
        {
            СompetenceСoeff? CompetenceCoeff = await GetTrackCompetenceCoeffById(CompetenceCoeffToSave.Id);
            //CompetenceCoeff? CompetenceCoeff = await context.CompetenceCoeffs.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(CompetenceCoeffToSave.Id));
            if (CompetenceCoeff != null && CompetenceCoeffToSave.Id != 0)
            {
                /*context.CompetenceCoeffs.Entry(CompetenceCoeffToSave).State = EntityState.Detached;
                context.Set<CompetenceCoeff>().Update(CompetenceCoeffToSave);*/
                CompetenceCoeff.IdCompetence = CompetenceCoeffToSave.IdCompetence;
                CompetenceCoeff.IdGroup = CompetenceCoeffToSave.IdGroup;
                CompetenceCoeff.Coefficient = CompetenceCoeffToSave.Coefficient;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.СompetenceСoeffs.AddAsync(CompetenceCoeffToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
