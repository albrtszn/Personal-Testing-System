using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class CompetenceCoeffDto
    {
        public int? Id { get; set; }
        public int? IdCompetence { get; set; }
        public int? IdGroup { get; set; }
        public decimal? Coefficient { get; set; }
    }
}
