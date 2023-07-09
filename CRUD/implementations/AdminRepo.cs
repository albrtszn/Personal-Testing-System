using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.interfaces;

namespace CRUD.implementations
{
    public class AdminRepo : IAdminRepo
    {
        private EFDbContext context;
        public AdminRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteAdminById(int id)
        {
            context.Admins.Remove(GetAllAdmins().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Admin> GetAllAdmins()
        {
            return context.Admins.ToList();
        }

        public Admin GetAdminById(int id)
        {
            return GetAllAdmins().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveAdmin(Admin AdminToSave)
        {
            context.Admins.Add(AdminToSave);
            context.SaveChanges();
        }
    }
}
