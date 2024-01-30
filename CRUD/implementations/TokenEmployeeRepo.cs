using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class TokenEmployeeRepo : ITokenEmployeeRepo
    {
        private EFDbContext context;
        public TokenEmployeeRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteTokenEmployeeById(int id)
        {
            context.TokenEmployees.Remove((await GetAllTokenEmployees()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TokenEmployee>> GetAllTokenEmployees()
        {
            return await context.TokenEmployees.ToListAsync();
        }

        public async Task<TokenEmployee> GetTokenEmployeeById(int id)
        {
            return await context.TokenEmployees.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }      
        public async Task<TokenEmployee> GetTrackTokenEmployeeById(int id)
        {
            return await context.TokenEmployees.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveTokenEmployee(TokenEmployee TokenEmployeeToSave)
        {
            TokenEmployee? TokenEmployee = await GetTrackTokenEmployeeById(TokenEmployeeToSave.Id);
            //TokenEmployee? TokenEmployee = await context.TokenEmployees.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TokenEmployeeToSave.Id));
            if (TokenEmployee != null && TokenEmployeeToSave.Id != 0)
            {
                /*context.TokenEmployees.Entry(TokenEmployeeToSave).State = EntityState.Detached;
                context.Set<TokenEmployee>().Update(TokenEmployeeToSave);*/
                TokenEmployee.IdEmployee = TokenEmployeeToSave.IdEmployee;
                TokenEmployee.Token = TokenEmployeeToSave.Token;
                TokenEmployee.IssuingTime = TokenEmployeeToSave.IssuingTime;
                TokenEmployee.State = TokenEmployeeToSave.State;
                TokenEmployee.ConnectionId = TokenEmployeeToSave.ConnectionId;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.TokenEmployees.AddAsync(TokenEmployeeToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
