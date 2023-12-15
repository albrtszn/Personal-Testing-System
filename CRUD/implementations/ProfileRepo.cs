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
    public class ProfileRepo : IProfileRepo
    {
        private EFDbContext context;
        public ProfileRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteProfileById(int id)
        {
            context.Profiles.Remove((await GetAllProfiles()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Profile>> GetAllProfiles()
        {
            return await context.Profiles.ToListAsync();
        }

        public async Task<Profile> GetProfileById(int id)
        {
            return await context.Profiles.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }        
        public async Task<Profile> GetTrackProfileById(int id)
        {
            return await context.Profiles.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveProfile(Profile ProfileToSave)
        {
            Profile? Profile = await GetTrackProfileById(ProfileToSave.Id);
            //Profile? Profile = await context.Profiles.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(ProfileToSave.Id));
            if (Profile != null && ProfileToSave.Id != 0)
            {
                /*context.Profiles.Entry(ProfileToSave).State = EntityState.Detached;
                context.Set<Profile>().Update(ProfileToSave);*/
                Profile.Name = ProfileToSave.Name;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Profiles.AddAsync(ProfileToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
