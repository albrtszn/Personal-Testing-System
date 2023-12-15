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
    public class FirstPartRepo : IFirstPartRepo
    {
        private EFDbContext context;
        public FirstPartRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteFirstPartById(string id)
        {
            context.FirstParts.Remove((await GetAllFirstParts()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FirstPart>> GetAllFirstParts()
        {
            return await context.FirstParts.ToListAsync();
        }

        public async Task<FirstPart> GetFirstPartById(string id)
        {
            return await context.FirstParts.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }        
        async Task<FirstPart> GetTrackFirstPartById(string id)
        {
            return await context.FirstParts.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveFirstPart(FirstPart FirstPartToSave)
        {
            FirstPart? FirstPart = await GetTrackFirstPartById(FirstPartToSave.Id);
            //FirstPart? FirstPart = await context.FirstParts.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(FirstPartToSave.Id));
            if (FirstPart != null && !FirstPartToSave.Id.IsNullOrEmpty())
            {
                /*context.FirstParts.Entry(FirstPartToSave).State = EntityState.Detached;
                context.Set<FirstPart>().Update(FirstPartToSave);*/
                FirstPart.Text = FirstPartToSave.Text;
                FirstPart.IdQuestion = FirstPartToSave.IdQuestion;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.FirstParts.AddAsync(FirstPartToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
