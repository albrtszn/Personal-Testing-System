using CRUD.interfaces;
using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.implementations
{
    public class TokenAdminRepo : ITokenAdminRepo
    {
        private EFDbContext context;
        public TokenAdminRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public void DeleteTokenAdminById(int id)
        {
            context.TokenAdmins.Remove(GetAllTokenAdmins().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<TokenAdmin> GetAllTokenAdmins()
        {
            return context.TokenAdmins.ToList();
        }

        public TokenAdmin GetTokenAdminById(int id)
        {
            return GetAllTokenAdmins().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveTokenAdmin(TokenAdmin TokenAdminToSave)
        {
            TokenAdmin? TokenAdmin = GetTokenAdminById(TokenAdminToSave.Id);
            if (TokenAdmin != null && TokenAdminToSave.Id != 0)
            {
                //context.TokenAdmins.Update(TokenAdminToSave);
                TokenAdmin.IdAdmin = TokenAdminToSave.IdAdmin;
                TokenAdmin.Token = TokenAdminToSave.Token;
                TokenAdmin.IssuingTime = TokenAdminToSave.IssuingTime;
                TokenAdmin.State = TokenAdminToSave.State;
            }
            else
            {
                context.TokenAdmins.Add(TokenAdminToSave);
            }
            context.SaveChanges();
        }
    }
}
