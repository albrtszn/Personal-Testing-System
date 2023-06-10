using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeMartchingDto
    {
        public int Id { get; set; }

        public int? IdFirstPart { get; set; }

        public int? IdSecondPart { get; set; }

        public int? IdTestResult { get; set; }
    }
}
