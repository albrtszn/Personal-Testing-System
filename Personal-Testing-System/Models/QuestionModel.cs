namespace Personal_Testing_System.Models
{
    public class QuestionModel
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public string? ImagePath { get; set; }
        public string? Base64Image { get; set; }
        public int? IdQuestionType { get; set; }
        public List<object>? Answers { get; set; }//AnswerDto, SubsequenceDto, FirstPartDto, SecondPartDto
    }
}
