using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class SecondPartDto
    {
        public int Id { get; set; }

        public string? Text { get; set; }

        public int? IdFirstPart { get; set; }
    }
}
