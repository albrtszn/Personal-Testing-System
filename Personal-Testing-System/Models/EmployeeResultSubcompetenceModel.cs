using System.Collections;

namespace Personal_Testing_System.Models
{
    public class EmployeeResultSubcompetenceModel
    {
        public int? Id { get; set; }
        public int? ScoreFrom { get; set; }
        public int? ScoreTo { get; set; }
        public string? ResultLevel { get; set; }
        public double? NumberPoints { get; set; }
        public List<ResultSubcompetenceModel>? SubcompetenceResults { get; set; }
        public ResultModel? Result { get; set; }
        public EmployeeModel? Employee { get; set; }
    }
}
