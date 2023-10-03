using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Models
{
    public class EmployeeModel
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? RegistrationDate { get; set; }
        public SubdivisionDto? Subdivision { get; set; }
    }
}
