using CRUD.interfaces;
using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;
using Personal_Testing_System.Models;

namespace Personal_Testing_System.Services
{
    public class TestScoreService
    {
        private ITestScoreRepo TestScoreRepo;
        public TestScoreService(ITestScoreRepo _TestScoreRepo)
        {
            this.TestScoreRepo = _TestScoreRepo;
        }
        private TestScoreDto ConvertToTestScoreDto(TestScore TestScore)
        {
            return new TestScoreDto
            {
                Id = TestScore.Id,
                MinValue = TestScore.MinValue,
                MaxValue = TestScore.MaxValue,
                Name = TestScore.Name,
                Description = TestScore.Description,
                IdTest = TestScore.IdTest,
                Recommend = TestScore.Recommend
            };
        }
        private TestScore ConvertToTestScore(TestScoreDto TestScoreDto)
        {
            return new TestScore
            {
                Id = TestScoreDto.Id.Value,
                MinValue = TestScoreDto.MinValue.Value,
                MaxValue = TestScoreDto.MaxValue.Value,
                Name = TestScoreDto.Name,
                Description = TestScoreDto.Description,
                IdTest = TestScoreDto.IdTest,
                Recommend = TestScoreDto.Recommend.Value
            };
        }
        public async Task<bool> DeleteTestScoreById(int id)
        {
            return await TestScoreRepo.DeleteTestScoreById(id);
        }

        public async Task<bool> DeleteTestScoresByTest(string idTest)
        {
            List<TestScore> list = (await GetAllTestScores()).Where(x =>
                x!=null && 
                x.IdTest != null &&
                x.IdTest.Equals(idTest))
                .ToList();

            foreach (TestScore TestScore in list)
            {
                await DeleteTestScoreById(TestScore.Id);
            }
            return true;
        }

        public async Task<List<TestScore>> GetAllTestScores()
        {
            return await TestScoreRepo.GetAllTestScores();
        }

        public async Task<List<TestScore>> GetTestScoresByTest(string idTest)
        {
            return (await GetAllTestScores()).Where(x =>
                x != null &&
                x.IdTest != null &&
                x.IdTest.Equals(idTest))
                .ToList();
        }

        public async Task<List<TestScoreDto>> GetAllTestScoreDtos()
        {
            List<TestScoreDto> TestScores = new List<TestScoreDto>();
            List<TestScore> list = await TestScoreRepo.GetAllTestScores();
            list.ForEach(x => TestScores.Add(ConvertToTestScoreDto(x)));
            return TestScores;
        }

        public async Task<List<TestScoreDto>> GetTestScoreDtosByTest(string idTest)
        {
            return (await GetAllTestScoreDtos()).Where(x =>
                x != null &&
                x.IdTest != null &&
                x.IdTest.Equals(idTest))
                .ToList();
        }

        public async Task<TestScore> GetTestScoreById(int id)
        {
            return await TestScoreRepo.GetTestScoreById(id);
        }

        public async Task<bool> SaveTestScore(TestScore TestScoreToSave)
        {
            return await TestScoreRepo.SaveTestScore(TestScoreToSave);
        }       
        public async Task<bool> SaveTestScore(TestScoreDto TestScoreToSave)
        {
            return await TestScoreRepo.SaveTestScore(ConvertToTestScore(TestScoreToSave));
        }

        public async Task<bool> SaveTestScore(AddTestScoreModel TestScoreToSave)
        {
            return await TestScoreRepo.SaveTestScore(new TestScore()
            {
                MinValue = TestScoreToSave.MinValue.Value,
                MaxValue = TestScoreToSave.MaxValue.Value,
                Name = TestScoreToSave.Name,
                Description = TestScoreToSave.Description,
                IdTest = TestScoreToSave.IdTest,
                Recommend = TestScoreToSave.Recommend.Value
            });
        }
    }
}
