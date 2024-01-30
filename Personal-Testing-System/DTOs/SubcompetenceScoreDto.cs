using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class SubcompetenceScoreDto
    {
        public int? Id { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string? Description { get; set; }
        public int? IdSubcompetence { get; set; }
        public int? NumberPoints { get; set; }
    }
}
