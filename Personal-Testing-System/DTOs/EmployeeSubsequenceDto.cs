using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeSubsequenceDto
    {
        public int Id { get; set; }

        public int? IdSubsequence { get; set; }

        public string? IdResult { get; set; }

        public int? Number { get; set; }
    }
}
