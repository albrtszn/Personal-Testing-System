using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeResultDto
    {
        public int? Id { get; set; }
        public int? ScoreFrom { get; set; }
        public int? ScoreTo { get; set; }
        public string? IdResult { get; set; }
        public string? IdEmployee { get; set; }
    }
}
