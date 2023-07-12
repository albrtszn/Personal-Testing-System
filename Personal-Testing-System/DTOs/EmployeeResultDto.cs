using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeResultDto
    {
        public string? Id { get; set; }
        public string? IdResult { get; set; }
        public int? IdEmployee { get; set; }
    }
}
