using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class SubsequenceRepo : ISubsequenceRepo
    {
        private EFDbContext context;
        public SubsequenceRepo(EFDbContext _context)
        {
            this.context = _context;
        }

        public void DeleteSubsequenceById(int id)
        {
            context.Subsequences.Remove(GetAllSubSequences().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Subsequence> GetAllSubSequences()
        {
            return context.Subsequences.ToList();
        }

        public Subsequence GetSubSequenceById(int id)
        {
            return GetAllSubSequences().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveSubsequence(Subsequence SubsequenceToSave)
        {
            context.Subsequences.Add(SubsequenceToSave);
            context.SaveChanges();
        }
    }
}
