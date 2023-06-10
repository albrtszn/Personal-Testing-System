using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeAnswerDto
    {
        public int Id { get; set; }
        [Column("idAnswer")]
        public int? IdAnswer { get; set; }

        public int? IdTestResult { get; set; }
    }
}
