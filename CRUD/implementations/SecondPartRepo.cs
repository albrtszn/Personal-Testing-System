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
    public class SecondPartRepo : ISecondPartRepo
    {
        private EFDbContext context;
        public SecondPartRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteSecondPartById(int id)
        {
            context.SecondParts.Remove((await GetAllSecondParts()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SecondPart>> GetAllSecondParts()
        {
            return await context.SecondParts.ToListAsync();
        }

        public async Task<SecondPart> GetSecondPartById(int id)
        {
            return await context.SecondParts.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }      
        public async Task<SecondPart> GetTrackSecondPartById(int id)
        {
            return await context.SecondParts.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveSecondPart(SecondPart SecondPartToSave)
        {
            SecondPart? sp = await GetTrackSecondPartById(SecondPartToSave.Id);
            //SecondPart? SecondPart = await context.SecondParts.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(SecondPartToSave.Id));
            if (sp != null && SecondPartToSave.Id != 0)
            {
                /*context.SecondParts.Entry(SecondPartToSave).State = EntityState.Detached;
                context.Set<SecondPart>().Update(SecondPartToSave);*/
                sp.Text = SecondPartToSave.Text;
                sp.IdFirstPart = SecondPartToSave.IdFirstPart;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.SecondParts.AddAsync(SecondPartToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
