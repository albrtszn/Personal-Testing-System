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
    public class SecondPartRepo : ISecondPartRepo
    {
        private EFDbContext context;
        public SecondPartRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteSecondPartById(int id)
        {
            context.SecondParts.Remove(GetAllSecondParts().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<SecondPart> GetAllSecondParts()
        {
            return context.SecondParts.ToList();
        }

        public SecondPart GetSecondPartById(int id)
        {
            return GetAllSecondParts().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveSecondPart(SecondPart SecondPartToSave)
        {
            SecondPart? sp = GetSecondPartById(SecondPartToSave.Id);
            if (sp != null && SecondPartToSave.Id != 0)
            {
                //context.SecondParts.Update(SecondPartToSave);
                sp.Text = SecondPartToSave.Text;
                sp.IdFirstPart = SecondPartToSave.IdFirstPart;
            }
            else
            {
                context.SecondParts.Add(SecondPartToSave);
            }
            context.SaveChanges();
        }
    }
}
