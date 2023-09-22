using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class QuestionDto
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public int? IdQuestionType { get; set; }
        public int? Number { get; set; }
        public int? Weight { get; set; }
        public string? IdTest { get; set; }
        public string? ImagePath { get; set; }
    }
}
