using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.Identity.Client;
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
        public void DeleteTokenEmployeeById(int id)
        {
            context.TokenEmployees.Remove(GetAllTokenEmployees().FirstOrDefault(x => x.Id.Equals(id)));
            context.SaveChanges();
        }

        public List<TokenEmployee> GetAllTokenEmployees()
        {
            return context.TokenEmployees.ToList();
        }

        public TokenEmployee? GetTokenEmployeeById(int id)
        {
            return GetAllTokenEmployees().FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveTokenEmployee(TokenEmployee TokenEmployeeToSave)
        {
            TokenEmployee? TokenEmployee = GetTokenEmployeeById(TokenEmployeeToSave.Id);
            if (TokenEmployee != null && TokenEmployeeToSave.Id != 0)
            {
                //context.TokenEmployees.Update(TokenEmployeeToSave);
                TokenEmployee.IdEmployee = TokenEmployeeToSave.IdEmployee;
                TokenEmployee.Token = TokenEmployeeToSave.Token;
                TokenEmployee.IssuingTime = TokenEmployeeToSave.IssuingTime;
                TokenEmployee.State = TokenEmployeeToSave.State;
            }
            else
            {
                context.TokenEmployees.Add(TokenEmployeeToSave);
            }
            context.SaveChanges();
        }
    }
}
