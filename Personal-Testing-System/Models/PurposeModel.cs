using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Models
{
    public class PurposeModel
    {
        public int? Id { get; set; }
        public string? IdEmployee { get; set; }
        public TestGetModel? Test { get; set; }
        public string? DatatimePurpose { get; set; }
        public int? Timer { get; set; }
    }
}
