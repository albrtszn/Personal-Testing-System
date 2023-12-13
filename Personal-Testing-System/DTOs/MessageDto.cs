using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class MessageDto
    {
        public int? Id { get; set; }
        public string? IdEmployee { get; set; }
        public string? MessageText { get; set; }
        public bool? StatusRead { get; set; }
        public string? DateAndTime { get; set; }
    }
}
