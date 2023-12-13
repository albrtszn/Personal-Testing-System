using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Models
{
    public class ResultSubcompetenceModel
    {
        public int Id { get; set; }
        public string? IdResult { get; set; }
        public SubcompetenceDto? Subcompetence{ get; set; }
        public int Result { get; set; }
    }
}
