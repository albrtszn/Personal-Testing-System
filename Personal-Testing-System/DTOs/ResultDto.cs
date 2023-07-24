using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class ResultDto
    {
        public string? Id { get; set; } 
        public string? IdTest { get; set; }
        public string? StartDate { get; set; }//DateOnly
        public string? StartTime { get; set; }//TimeOmly
        public int? Duration { get; set; }//byte???
        public string? EndTime { get; set; }//TimeOnly
        public string? Description { get; set; }
    }
}
