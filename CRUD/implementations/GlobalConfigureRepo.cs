using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRUD.implementations
{
    public class GlobalConfigureRepo : IGlobalConfigureRepo
    {
        private readonly EFDbContext context;
        public GlobalConfigureRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteGlobalConfigureById(int id)
        {
            context.GlobalConfigures.Remove((await GetAllGlobalConfigures()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<GlobalConfigure>> GetAllGlobalConfigures()
        {
            return await context.GlobalConfigures.ToListAsync();
        }

        public async Task<GlobalConfigure> GetGlobalConfigureById(int id)
        {
            return await context.GlobalConfigures.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        public async Task<GlobalConfigure> GetTrackGlobalConfigureById(int id)
        {
            return await context.GlobalConfigures.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveGlobalConfigure(GlobalConfigure GlobalConfigureToSave)
        {
            GlobalConfigure? GlobalConfigure = await GetTrackGlobalConfigureById(GlobalConfigureToSave.Id);
            //GlobalConfigure? GlobalConfigure = await context.GlobalConfigures.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(GlobalConfigureToSave.Id));
            if (GlobalConfigure != null && GlobalConfigureToSave.Id != 0)
            {
                /*context.GlobalConfigures.Entry(GlobalConfigureToSave).State = EntityState.Detached;
                context.Set<GlobalConfigure>().Update(GlobalConfigureToSave);*/
                GlobalConfigure.TestingTimeLimit = GlobalConfigureToSave.TestingTimeLimit;
                GlobalConfigure.SkippingQuestion = GlobalConfigureToSave.SkippingQuestion;
                GlobalConfigure.EarlyCompletionTesting = GlobalConfigureToSave.EarlyCompletionTesting;
                GlobalConfigure.AdditionalBool = GlobalConfigureToSave.AdditionalBool;
                GlobalConfigure.AdditionalInt = GlobalConfigureToSave.AdditionalInt;
                GlobalConfigure.AdditionalString = GlobalConfigureToSave.AdditionalString;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.GlobalConfigures.AddAsync(GlobalConfigureToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
