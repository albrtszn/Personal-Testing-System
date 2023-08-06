using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class TokenAdminService
    {
        private ITokenAdminRepo TokenAdminRepo;
        public TokenAdminService(ITokenAdminRepo _TokenAdminRepo)
        {
            this.TokenAdminRepo = _TokenAdminRepo;
        }

        public void DeleteTokenAdminById(int id)
        {
            TokenAdminRepo.DeleteTokenAdminById(id);
        }

        public void DeleteExpiredTokenAdmins(string timeToExpire)
        {
            //GetAllTokenAdmins().Where(x => x.IssuingTime).ToList();
        }

        public List<TokenAdmin> GetAllTokenAdmins()
        {
            return TokenAdminRepo.GetAllTokenAdmins();
        }

        public TokenAdmin GetTokenAdminById(int id)
        {
            return TokenAdminRepo.GetTokenAdminById(id);
        }

        public TokenAdmin? GetTokenEmployeeByAdminId(string id)
        {
            return GetAllTokenAdmins().Find(x => x.IdAdmin.Equals(id));
        }

        public TokenAdmin? GetTokenAdminByToken(string token)
        {
            return GetAllTokenAdmins().Find(x => x.Token.Equals(token));
        }

        public void SaveTokenAdmin(TokenAdmin TokenAdminToSave)
        {
            TokenAdminRepo.SaveTokenAdmin(TokenAdminToSave);
        }
    }
}
