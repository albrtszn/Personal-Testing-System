using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Testing_System.DTOs
{
    public class TestResultDto
    {
        public int Id { get; set; }

        public int? IdEmployee { get; set; }

        public int? IdTest { get; set; }

        public DateOnly? StartDate { get; set; }

        public TimeOnly? StartTime { get; set; }

        public byte? Duration { get; set; }

        public TimeOnly? EndTime { get; set; }
    }
}
