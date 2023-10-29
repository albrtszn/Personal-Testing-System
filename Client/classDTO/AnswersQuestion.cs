using System.Windows.Media.Imaging;

namespace Client.classDTO
{
    public class AnswersQuestion
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public string Base64Image { get; set; }
        public int Number { get; set; }
        public int IdQuestionType { get; set; }
        public AnswersAnswer[] Answers { get; set; }
        public BitmapImage Img { get; set; }
    }
}