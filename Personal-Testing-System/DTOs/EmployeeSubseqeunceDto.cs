using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class EmployeeSubseqeunceDto
    {
        public int Id { get; set; }

        public int? IdSubsequence { get; set; }

        public int? IdTestResult { get; set; }

        public byte? Number { get; set; }
    }
}
