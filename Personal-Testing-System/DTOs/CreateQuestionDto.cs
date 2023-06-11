namespace Personal_Testing_System.DTOs
{
    public class CreateQuestionDto
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public int? IdQuestionType { get; set; }
        public List<Object>? Answers { get; set; }
    }
}
