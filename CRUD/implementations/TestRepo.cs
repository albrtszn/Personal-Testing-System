using CRUD.interfaces;
using DataBase.Repository;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CRUD.implementations
{
    public class TestRepo : ITestRepo
    {
        private EFDbContext context;
        public TestRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteTestById(string id)
        {
            context.Tests.Remove((await GetAllTests()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Test>> GetAllTests()
        {
            return await context.Tests.ToListAsync();
        }

        public async Task<Test> GetTestById(string id)
        {
            return await context.Tests.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveTest(Test TestToSave)
        {
            Test? Test = await GetTestById(TestToSave.Id);
            //Test? Test = await context.Tests.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TestToSave.Id));
            if (Test != null && !TestToSave.Id.IsNullOrEmpty())
            {
                /*context.Tests.Entry(TestToSave).State = EntityState.Detached;
                context.Set<Test>().Update(TestToSave);*/
                Test.Name = TestToSave.Name;
                Test.IdCompetence = TestToSave.IdCompetence;
                Test.Weight = TestToSave.Weight;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.Tests.AddAsync(TestToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
