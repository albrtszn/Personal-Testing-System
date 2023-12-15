using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class TestPurposeDto
    {
        public int? Id { get; set; }
        public string? IdEmployee { get; set; }
        public string? IdTest { get; set; }
        public string? DatatimePurpose { get; set; }
        public int? Timer{ get; set; }
    }
}
