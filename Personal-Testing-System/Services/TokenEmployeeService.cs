using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Services
{
    public class TokenEmployeeService
    {
        private ITokenEmployeeRepo TokenEmployeeRepo;
        public TokenEmployeeService(ITokenEmployeeRepo _TokenEmployeeRepo)
        {
            this.TokenEmployeeRepo = _TokenEmployeeRepo;
        }

        public async Task<bool> DeleteTokenEmployeeById(int id)
        {
            return await TokenEmployeeRepo.DeleteTokenEmployeeById(id);
        }

        public void DeleteExpiredTokenEmployees(string timeToExpire)
        {
            /*GetAllTokenEmployees().Where(x => x.IdQuestion.Equals(idQuestion)).ToList()
                           .ForEach(x => DeleteTokenEmployeeById(x.Id));*/
        }

        public async Task<List<TokenEmployee>> GetAllTokenEmployees()
        {
            return await TokenEmployeeRepo.GetAllTokenEmployees();
        }

        public async Task<TokenEmployee> GetTokenEmployeeById(int id)
        {
            return await TokenEmployeeRepo.GetTokenEmployeeById(id);
        }

        public async Task<TokenEmployee?> GetTokenEmployeeByEmployeeId(string id)
        {
            return (await GetAllTokenEmployees()).Find(x => x!= null && x.IdEmployee != null && x.IdEmployee.Equals(id));;
        }

        public async Task<TokenEmployee?> GetTokenEmployeeByToken(string token)
        {
            return (await TokenEmployeeRepo.GetAllTokenEmployees())
                                    .Find(x => x.Token.Equals(token));
        }

        public async Task<bool> SaveTokenEmployee(TokenEmployee TokenEmployeeToSave)
        {
            return await TokenEmployeeRepo.SaveTokenEmployee(TokenEmployeeToSave);
        }
    }
}
