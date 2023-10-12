using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Models
{
    public class EmployeeResultFSPartModel
    {
        public FirstPartDto? FirstPartId { get; set; }
        public SecondPartDto? SecondPartId { get; set; }
        public bool? IsUserAnswer { get; set; }
    }
}
