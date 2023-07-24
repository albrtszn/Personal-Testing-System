using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Personal_Testing_System.DTOs
{
    public class SubdivisionDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }
    }
}
