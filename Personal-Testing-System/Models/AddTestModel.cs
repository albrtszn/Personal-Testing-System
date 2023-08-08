namespace Personal_Testing_System.Models
{
    public class AddTestModel
    {
        public string? Name { get; set; }
        public int? CompetenceId { get; set; }
        public List<AddQuestionModel>? Questions { get; set; }
    }
}
