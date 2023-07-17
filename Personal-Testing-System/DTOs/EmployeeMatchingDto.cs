using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeMatchingDto
    {
        public int Id { get; set; }

        public string? IdFirstPart { get; set; }

        public int? IdSecondPart { get; set; }

        public string? IdResult { get; set; }
    }
}
