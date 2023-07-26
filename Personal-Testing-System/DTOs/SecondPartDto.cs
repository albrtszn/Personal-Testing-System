using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class SecondPartDto
    {
        public int? IdSecondPart { get; set; }
        public string? Text { get; set; }
        public string? IdFirstPart { get; set; }
    }
}
