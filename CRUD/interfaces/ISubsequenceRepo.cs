using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface ISubsequenceRepo
    {
        Task<List<Subsequence>> GetAllSubsequences();
        Task<Subsequence> GetSubsequenceById(int id);
        Task<bool> SaveSubsequence(Subsequence SubsequenceToSave);
        Task<bool> DeleteSubsequenceById(int id);
    }
}
