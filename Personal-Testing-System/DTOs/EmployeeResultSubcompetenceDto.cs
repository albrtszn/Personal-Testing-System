using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeResultSubcompetenceDto
    {
        public int Id { get; set; }
        public string? IdResult { get; set; }
        public int? IdSubcompetence { get; set; }
        public int Result { get; set; }
    }
}
