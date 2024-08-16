using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class CompetenceScoreDto
    {
        public int? Id { get; set; }
        public int? IdCompetence { get; set; }
        public int? IdGroup { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        public int? NumberPoints { get; set; }

        [StringLength(4000)]
        public string Description { get; set; } = null!;
    }
}
