namespace Personal_Testing_System.DTOs
{
    public class CreateTestDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? IdCompetence { get; set; }
        public List<CreateQuestionDto> Questions { get; set; }
    }
}
