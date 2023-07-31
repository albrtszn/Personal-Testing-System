using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class LogDto
    {
        public int Id { get; set; }
        public string? UrlPath { get; set; }
        public string? UserId { get; set; }
        public string? UserIp { get; set; }
        public string? DataTime { get; set; }
        public string? Params { get; set; }
    }
}
