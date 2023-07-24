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
    public class FirstPartRepo : IFirstPartRepo
    {
        private EFDbContext context;
        public FirstPartRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteFirstPartById(string id)
        {
            context.FirstParts.Remove(GetAllFirstParts().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<FirstPart> GetAllFirstParts()
        {
            return context.FirstParts.ToList();
        }

        public FirstPart GetFirstPartById(string id)
        {
            return GetAllFirstParts().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveFirstPArt(FirstPart FirstPartoSave)
        {
            if (!string.IsNullOrEmpty(FirstPartoSave.Id) && GetFirstPartById(FirstPartoSave.Id) != null)
            {
                context.FirstParts.Update(FirstPartoSave);
            }
            else
            {
                context.FirstParts.Add(FirstPartoSave);
            }
            context.SaveChanges();
        }
    }
}
