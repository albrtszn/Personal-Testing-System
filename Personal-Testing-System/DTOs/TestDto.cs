using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class TestDto
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public int? Weight { get; set; }
        public int? IdCompetence { get; set; }
        public string? Description { get; set; }
        public string? Instruction { get; set; }
        public bool? Generation { get; set; }
    }
}
