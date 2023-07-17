using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeAnswerDto
    {
        public int Id { get; set; }
        public int? IdAnswer { get; set; }

        public string? IdResult { get; set; }
    }
}
