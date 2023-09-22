using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class CompetenciesForGroupDto
    {
        public int? Id { get; set; }
        public string? IdTest { get; set; }
        public int? IdGroupPositions { get; set; }
    }
}
