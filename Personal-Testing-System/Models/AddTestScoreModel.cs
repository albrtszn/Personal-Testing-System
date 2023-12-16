namespace Personal_Testing_System.Models
{
    public class AddTestScoreModel
    {
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? IdTest { get; set; }
        public bool? Recommend { get; set; }
    }
}
