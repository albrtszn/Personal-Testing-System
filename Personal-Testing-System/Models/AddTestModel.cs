namespace Personal_Testing_System.Models
{
    public class AddTestModel
    {
        public string? Name { get; set; }
        public int? Weight { get; set; }
        public int? CompetenceId { get; set; }
        public string? Description { get; set; }
        public string? Instruction { get; set; }
        public List<AddQuestionModel>? Questions { get; set; }
    }
}
