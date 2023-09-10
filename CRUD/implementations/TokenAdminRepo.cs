using CRUD.interfaces;
using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRUD.implementations
{
    public class TokenAdminRepo : ITokenAdminRepo
    {
        private EFDbContext context;
        public TokenAdminRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteTokenAdminById(int id)
        {
            context.TokenAdmins.Remove((await GetAllTokenAdmins()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TokenAdmin>> GetAllTokenAdmins()
        {
            return await context.TokenAdmins.ToListAsync();
        }

        public async Task<TokenAdmin> GetTokenAdminById(int id)
        {
            return await context.TokenAdmins.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveTokenAdmin(TokenAdmin TokenAdminToSave)
        {
            TokenAdmin? TokenAdmin = await GetTokenAdminById(TokenAdminToSave.Id);
            //TokenAdmin? TokenAdmin = await context.TokenAdmins.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TokenAdminToSave.Id));
            if (TokenAdmin != null && TokenAdminToSave.Id != 0)
            {
                /*context.TokenAdmins.Entry(TokenAdminToSave).State = EntityState.Detached;
                context.Set<TokenAdmin>().Update(TokenAdminToSave);*/
                TokenAdmin.IdAdmin = TokenAdminToSave.IdAdmin;
                TokenAdmin.Token = TokenAdminToSave.Token;
                TokenAdmin.IssuingTime = TokenAdminToSave.IssuingTime;
                TokenAdmin.State = TokenAdminToSave.State;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.TokenAdmins.AddAsync(TokenAdminToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
