using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class QuestionSubcompetenceDto
    {
        public int? Id { get; set; }
        public string? IdQuestion { get; set; }
        public int? IdSubcompetence { get; set; }
    }
}
