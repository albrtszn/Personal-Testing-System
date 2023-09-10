using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace CRUD.implementations
{
    public class AdminRepo : IAdminRepo
    {
        private EFDbContext context;
        public AdminRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteAdminById(string id)
        {
            context.Admins.Remove((await GetAllAdmins()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Admin>> GetAllAdmins()
        {
            return await context.Admins.ToListAsync();
        }

        public async Task<Admin> GetAdminById(string id)
        {
            return await context.Admins.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveAdmin(Admin AdminToSave)
        {
            Admin? admin = await GetAdminById(AdminToSave.Id);
            //Admin? admin = await context.Admins.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(AdminToSave.Id));
            if (admin != null && !AdminToSave.Id.IsNullOrEmpty())
            {
                /*context.Admins.Entry(AdminToSave).State = EntityState.Detached;
                context.Set<Admin>().Update(AdminToSave);*/
                admin.FirstName = AdminToSave.FirstName;
                admin.SecondName = AdminToSave.SecondName;
                admin.LastName = AdminToSave.LastName;
                admin.Login = AdminToSave.Login;
                admin.Password = AdminToSave.Password;
                admin.IdSubdivision = AdminToSave.IdSubdivision;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Admins.AddAsync(AdminToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
