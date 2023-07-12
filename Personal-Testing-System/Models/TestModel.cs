using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Models
{
    public class TestModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public CompetenceDto? Competence { get; set; }
        public List<QuestionModel>? Questions { get; set; }
    }
}
