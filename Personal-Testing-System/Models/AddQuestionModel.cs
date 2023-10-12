namespace Personal_Testing_System.Models
{
    public class AddQuestionModel
    {
        public string? Text { get; set; }
        public string? ImagePath { get; set; }
        public int? IdQuestionType { get; set; }
        public int? Weight { get; set; }
        public List<object>? Answers { get; set; }
    }
}
