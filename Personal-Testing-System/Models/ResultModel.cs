namespace Personal_Testing_System.Models
{
    public class ResultModel
    {
        public string? Id { get; set; }
        public TestGetModel? Test { get; set; }
        public string? StartDate { get; set; }//DateOnly
        public string? StartTime { get; set; }//TimeOmly
        public int? Duration { get; set; }//byte???
        public string? EndTime { get; set; }//TimeOnly
        public string? Description { get; set; }
    }
}
