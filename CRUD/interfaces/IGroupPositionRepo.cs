using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IGroupPositionRepo
    {
        Task<List<GroupPosition>> GetAllGroupPositions();
        Task<GroupPosition> GetGroupPositionById(int id);
        Task<bool> SaveGroupPosition(GroupPosition GroupPositionToSave);
        Task<bool> DeleteGroupPositionById(int id);
    }
}
