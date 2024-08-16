namespace Personal_Testing_System.Models
{
    public class UpdateQuestionInTest
    {
        public string? IdQuestion { get; set; }
        public string? IdTest { get; set; }
        public string? Text { get; set; }
        public string? ImagePath { get; set; }
        public int? Number { get; set; }
        public int? Weight { get; set; }
        public int? IdQuestionType { get; set; }
        public List<object>? Answers { get; set; }
    }
}
