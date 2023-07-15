using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Models
{
    public class CreateTestModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int? CompetenceId { get; set; }
        public List<QuestionModel>? Questions { get; set; }
    }
}
