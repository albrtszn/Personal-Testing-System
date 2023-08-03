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

        public void SaveTokenAdmin(TokenAdmin TokenAdminToSave)
        {
            TokenAdminRepo.SaveTokenAdmin(TokenAdminToSave);
        }
    }
}
