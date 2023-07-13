namespace Personal_Testing_System.Models
{
    public class TestResultModel
    {
        public string? TestId { get; set; }
        public string? EmployeeId { get; set; }
        public string? startDate { get; set; }//DateOnly
        public string? startTime { get; set; }//TimeOnly
        public string? endTime { get; set; }//DateOnly
        public string? description { get; set; }
        public List<QuestionResultModel>? Questions { get; set; }

    }
}
