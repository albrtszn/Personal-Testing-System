using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class ResultRepo : IResultRepo
    {
        private EFDbContext context;
        public ResultRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteResultById(string id)
        {
            context.Results.Remove(GetAllResults().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Result> GetAllResults()
        {
            return context.Results.ToList();
        }

        public Result GetResultById(string id)
        {
            return GetAllResults().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveResult(Result ResultToSave)
        {
            context.Results.Add(ResultToSave);
            context.SaveChanges();
        }
    }
}
