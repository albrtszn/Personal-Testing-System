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
    public class GroupPositionRepo : IGroupPositionRepo
    {
        private EFDbContext context;
        public GroupPositionRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteGroupPositionById(int id)
        {
            context.GroupPositions.Remove((await GetAllGroupPositions()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<GroupPosition>> GetAllGroupPositions()
        {
            return await context.GroupPositions.ToListAsync();
        }

        public async Task<GroupPosition> GetGroupPositionById(int id)
        {
            return await context.GroupPositions.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }     
        public async Task<GroupPosition> GetTrackGroupPositionById(int id)
        {
            return await context.GroupPositions.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveGroupPosition(GroupPosition GroupPositionToSave)
        {
            GroupPosition? GroupPosition = await GetTrackGroupPositionById(GroupPositionToSave.Id);
            //GroupPosition? GroupPosition = await context.GroupPositions.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(GroupPositionToSave.Id));
            if (GroupPosition != null && GroupPositionToSave.Id != 0)
            {
                /*context.GroupPositions.Entry(GroupPositionToSave).State = EntityState.Detached;
                context.Set<GroupPosition>().Update(GroupPositionToSave);*/
                GroupPosition.Name = GroupPositionToSave.Name;
                GroupPosition.IdProfile = GroupPositionToSave.IdProfile;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.GroupPositions.AddAsync(GroupPositionToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
