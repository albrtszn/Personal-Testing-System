using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Models
{
    public class PurposeAdminModel
    {
        public int? Id { get; set; }
        public EmployeeModel? Employee { get; set; }
        public TestGetModel? Test { get; set; }
        public string? DatatimePurpose { get; set; }
    }
}
