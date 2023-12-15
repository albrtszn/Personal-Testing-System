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
    public class MessageRepo : IMessageRepo
    {
        private readonly EFDbContext context;
        public MessageRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteMessageById(int id)
        {
            context.Messages.Remove((await GetAllMessages()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Message>> GetAllMessages()
        {
            return await context.Messages.ToListAsync();
        }

        public async Task<Message> GetMessageById(int id)
        {
            return await context.Messages.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }     
        public async Task<Message> GetTrackMessageById(int id)
        {
            return await context.Messages.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveMessage(Message MessageToSave)
        {
            Message? Message = await GetTrackMessageById(MessageToSave.Id);
            //Message? Message = await context.Messages.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(MessageToSave.Id));
            if (Message != null && MessageToSave.Id != 0)
            {
                /*context.Messages.Entry(MessageToSave).State = EntityState.Detached;
                context.Set<Message>().Update(MessageToSave);*/
                Message.IdEmployee = MessageToSave.IdEmployee;
                Message.MessageText = MessageToSave.MessageText;
                Message.StatusRead = MessageToSave.StatusRead;
                Message.DateAndTie = MessageToSave.DateAndTie;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Messages.AddAsync(MessageToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
