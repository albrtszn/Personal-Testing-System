namespace Personal_Testing_System.Models
{
    public class TestResultModel
    {
        public string? TestId { get; set; }
        public string? EmployeeId { get; set; }
        public string? StartDate { get; set; }//DateOnly
        public string? StartTime { get; set; }//TimeOnly
        public string? EndTime { get; set; }//DateOnly
        public string? Description { get; set; }
        public List<QuestionResultModel>? Questions { get; set; }
    }
}
