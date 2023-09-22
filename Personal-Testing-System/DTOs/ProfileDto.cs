using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class ProfileDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }
}
