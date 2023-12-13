namespace Personal_Testing_System.Models
{
    public class EmployeeResultAnswersModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int? ScoreFrom { get; set; }
        public int? ScoreTo { get; set; }
        public bool? Generation { get; set; }
        public string? Description { get; set; }
        public string? Instruction { get; set; }
        public int? CompetenceId { get; set; }
        public List<QuestionModel>? Questions { get; set; }
    }
}
