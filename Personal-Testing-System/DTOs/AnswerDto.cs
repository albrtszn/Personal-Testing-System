using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class AnswerDto
    {
        public int? Id { get; set; }

        public string? Text { get; set; }

        public int? IdQuestion { get; set; }

        public bool? Correct { get; set; }
    }
}
