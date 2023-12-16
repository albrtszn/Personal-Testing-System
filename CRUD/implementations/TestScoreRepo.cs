using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRUD.implementations
{
    public class TestScoreRepo : ITestScoreRepo
    {
        private readonly EFDbContext context;
        public TestScoreRepo(EFDbContext _context)
        {
            this.context = _context;
        }
        public async Task<bool> DeleteTestScoreById(int id)
        {
            context.TestScores.Remove((await GetAllTestScores()).FirstOrDefault(x => x.Id.Equals(id)));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TestScore>> GetAllTestScores()
        {
            return await context.TestScores.ToListAsync();
        }

        public async Task<TestScore> GetTestScoreById(int id)
        {
            return await context.TestScores.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        public async Task<TestScore> GetTrackTestScoreById(int id)
        {
            return await context.TestScores.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> SaveTestScore(TestScore TestScoreToSave)
        {
            TestScore? TestScore = await GetTrackTestScoreById(TestScoreToSave.Id);
            //TestScore? TestScore = await context.TestScores.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TestScoreToSave.Id));
            if (TestScore != null && TestScoreToSave.Id != 0)
            {
                /*context.TestScores.Entry(TestScoreToSave).State = EntityState.Detached;
                context.Set<TestScore>().Update(TestScoreToSave);*/
                TestScore.MinValue = TestScoreToSave.MinValue;
                TestScore.MaxValue = TestScoreToSave.MaxValue;
                TestScore.Name = TestScoreToSave.Name;
                TestScore.Description = TestScoreToSave.Description;
                TestScore.IdTest = TestScoreToSave.IdTest;
                TestScore.Recommend = TestScoreToSave.Recommend;

                await context.SaveChangesAsync();
            }
            else
            {
                await context.TestScores.AddAsync(TestScoreToSave);
                await context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
