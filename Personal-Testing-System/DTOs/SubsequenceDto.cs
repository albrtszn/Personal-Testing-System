using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class SubsequenceDto
    {
        public int IdSubsequence { get; set; }
        public string? Text { get; set; }
        public string? IdQuestion { get; set; }
        public int? Number { get; set; }
    }
}
