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
        List<Subsequence> GetAllSubSequences();
        Subsequence GetSubsequenceById(int id);
        void SaveSubsequence(Subsequence SubsequenceToSave);
        void DeleteSubsequenceById(int id);
    }
}
