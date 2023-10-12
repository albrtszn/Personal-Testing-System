using DataBase.Repository.Models;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Models
{
    public class TestModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int? Weight { get; set; }
        public bool? Generation { get; set; }
        public string? Description { get; set; }
        public string? Instruction { get; set; }
        public int? CompetenceId { get; set; }
        public List<QuestionModel>? Questions { get; set; }
    }
}
