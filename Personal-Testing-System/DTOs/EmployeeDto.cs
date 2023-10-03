using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeDto
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? DateOfBirth { get; set; }
        public int? IdSubdivision { get; set; }
        public string? Phone { get; set; }
        public string? RegistrationDate { get; set; }
    }
}
