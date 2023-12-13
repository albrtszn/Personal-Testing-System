using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IMessageRepo
    {
        Task<List<Message>> GetAllMessages();
        Task<Message> GetMessageById(int id);
        Task<bool> SaveMessage(Message MessageToSave);
        Task<bool> DeleteMessageById(int id);
    }
}
