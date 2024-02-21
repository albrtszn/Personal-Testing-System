using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.Models
{
    public class AddCompetenceScoreModel
    {
        public int? IdCompetence { get; set; }
        public int? IdGroup { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        public int? NumberPoints { get; set; }

        [StringLength(1500)]
        public string Description { get; set; } = null;
    }
}
