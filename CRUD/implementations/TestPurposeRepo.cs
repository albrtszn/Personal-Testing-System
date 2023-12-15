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

namespace CRUD.implementations
{
    public class TestPurposeRepo : ITestPurposeRepo
    {
        private EFDbContext context;
        public TestPurposeRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteTestPurposeById(int id)
        {
            context.TestPurposes.Remove((await GetAllTestPurposes()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TestPurpose>> GetAllTestPurposes()
        {
            return await context.TestPurposes.ToListAsync();
        }

        public async Task<TestPurpose> GetTestPurposeById(int id)
        {
            return await context.TestPurposes.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }      
        public async Task<TestPurpose> GetTrackTestPurposeById(int id)
        {
            return await context.TestPurposes.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveTestPurpose(TestPurpose TestPurposeToSave)
        {
            TestPurpose? TestPurpose = await GetTrackTestPurposeById(TestPurposeToSave.Id);
            //TestPurpose? TestPurpose = await context.TestPurposes.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TestPurposeToSave.Id));
            if (TestPurpose != null && TestPurposeToSave.Id != 0)
            {
                /*context.TestPurposes.Entry(TestPurposeToSave).State = EntityState.Detached;
                context.Set<TestPurpose>().Update(TestPurposeToSave);*/
                TestPurpose.IdEmployee = TestPurposeToSave.IdEmployee;
                TestPurpose.IdTest = TestPurposeToSave.IdTest;
                TestPurpose.DatatimePurpose = TestPurposeToSave.DatatimePurpose;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.TestPurposes.AddAsync(TestPurposeToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
