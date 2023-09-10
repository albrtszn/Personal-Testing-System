using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class SubsequenceRepo : ISubsequenceRepo
    {
        private EFDbContext context;
        public SubsequenceRepo(EFDbContext _context)
        {
            this.context = _context;
        }

        public async Task<bool> DeleteSubsequenceById(int id)
        {
            context.Subsequences.Remove((await GetAllSubsequences()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Subsequence>> GetAllSubsequences()
        {
            return await context.Subsequences.ToListAsync();
        }

        public async Task<Subsequence> GetSubsequenceById(int id)
        {
            return await context.Subsequences.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveSubsequence(Subsequence SubsequenceToSave)
        {
            Subsequence? Subsequence = await GetSubsequenceById(SubsequenceToSave.Id);
            //Subsequence? Subsequence = await context.Subsequences.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(SubsequenceToSave.Id));
            if (Subsequence != null && SubsequenceToSave.Id != 0)
            {
                /*context.Subsequences.Entry(SubsequenceToSave).State = EntityState.Detached;
                context.Set<Subsequence>().Update(SubsequenceToSave);*/
                Subsequence.Text = SubsequenceToSave.Text;
                Subsequence.IdQuestion = SubsequenceToSave.IdQuestion;
                Subsequence.Number = SubsequenceToSave.Number;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Subsequences.AddAsync(SubsequenceToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
