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

        public void DeleteTokenEmployeeById(int id)
        {
            TokenEmployeeRepo.DeleteTokenEmployeeById(id);
        }

        public void DeleteExpiredTokenEmployees(string timeToExpire)
        {
            /*GetAllTokenEmployees().Where(x => x.IdQuestion.Equals(idQuestion)).ToList()
                           .ForEach(x => DeleteTokenEmployeeById(x.Id));*/
        }

        public List<TokenEmployee> GetAllTokenEmployees()
        {
            return TokenEmployeeRepo.GetAllTokenEmployees();
        }

        public TokenEmployee? GetTokenEmployeeById(int id)
        {
            return TokenEmployeeRepo.GetTokenEmployeeById(id);
        }

        public TokenEmployee? GetTokenEmployeeByEmployeeId(string id)
        {
            return GetAllTokenEmployees().FirstOrDefault(x => x.IdEmployee.Equals(id));
        }

        public TokenEmployee? GetTokenEmployeeByToken(string token)
        {
            return TokenEmployeeRepo.GetAllTokenEmployees()
                                    .Find(x => x.Token.Equals(token));
        }

        public void SaveTokenEmployee(TokenEmployee TokenEmployeeToSave)
        {
            TokenEmployeeRepo.SaveTokenEmployee(TokenEmployeeToSave);
        }
    }
}
