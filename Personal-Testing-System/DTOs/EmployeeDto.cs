using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? SecondName { get; set; }

        public string? LastName { get; set; }

        public int? IdSubdivision { get; set; }
    }
}
