using CRUD.implementations;
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

        public async Task<bool> DeleteTokenAdminById(int id)
        {
            return await TokenAdminRepo.DeleteTokenAdminById(id);
        }

        public void DeleteExpiredTokenAdmins(string timeToExpire)
        {
            /*GetAllTokenAdmins().Where(x => x.IdQuestion.Equals(idQuestion)).ToList()
                           .ForEach(x => DeleteTokenAdminById(x.Id));*/
        }

        public async Task<List<TokenAdmin>> GetAllTokenAdmins()
        {
            return await TokenAdminRepo.GetAllTokenAdmins();
        }

        public async Task<TokenAdmin> GetTokenAdminById(int id)
        {
            return await TokenAdminRepo.GetTokenAdminById(id);
        }

        public async Task<TokenAdmin?> GetTokenAdminByAdminId(string id)
        {
            return (await GetAllTokenAdmins()).Find(x => x != null && x.IdAdmin != null && x.IdAdmin.Equals(id));
        }

        public async Task<TokenAdmin?> GetTokenAdminByToken(string token)
        {
            return (await TokenAdminRepo.GetAllTokenAdmins())
                                    .Find(x => x.Token.Equals(token));
        }

        public async Task<bool> SaveTokenAdmin(TokenAdmin TokenAdminToSave)
        {
            return await TokenAdminRepo.SaveTokenAdmin(TokenAdminToSave);
        }
    }
}
