using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CRUD.implementations
{
    public class AdminRepo : IAdminRepo
    {
        private EFDbContext context;
        public AdminRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteAdminById(string id)
        {
            context.Admins.Remove(GetAllAdmins().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<Admin> GetAllAdmins()
        {
            return context.Admins.ToList();
        }

        public Admin GetAdminById(string id)
        {
            return GetAllAdmins().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveAdmin(Admin AdminToSave)
        {
            Admin? admin = GetAdminById(AdminToSave.Id);
            if (admin != null && !AdminToSave.Id.IsNullOrEmpty())
            {
                //context.Admins.Update(AdminToSave);
                admin.FirstName = AdminToSave.FirstName;
                admin.SecondName = AdminToSave.SecondName;
                admin.LastName = AdminToSave.LastName;
                admin.Login = AdminToSave.Login;
                admin.Password = AdminToSave.Password;
                admin.IdSubdivision = AdminToSave.IdSubdivision;
            }
            else
            {
                context.Admins.Add(AdminToSave);
            }
            context.SaveChanges();
        }
    }
}
