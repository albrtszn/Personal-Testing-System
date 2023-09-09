namespace Personal_Testing_System.Models
{
    public class AnswerModel
    {
        public int? IdAnswer { get; set; }
        public int? Number { get; set; }
        public int? Weight { get; set; }
        public string? Text { get; set; }
        public string? IdQuestion { get; set; }
        public bool? Correct { get; set; }
        public string? Base64Image { get; set; }
        public string? ImagePath { get; set; }
    }
}
