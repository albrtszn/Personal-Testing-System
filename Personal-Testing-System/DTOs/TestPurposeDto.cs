using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class TestPurposeDto
    {
        public int Id { get; set; }
        public int? IdEmployee { get; set; }
        public int? IdTest { get; set; }
        public DateTime? DatatimePurpose { get; set; }
    }
}
