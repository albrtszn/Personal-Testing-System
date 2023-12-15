using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class GlobalConfigureDto
    {
        public int? Id { get; set; }
        public bool? TestingTimeLimit { get; set; }
        public bool? SkippingQuestion { get; set; }
        public bool? EarlyCompletionTesting { get; set; }
        public bool? AdditionalBool { get; set; }
        public int? AdditionalInt { get; set; }
        public string? AdditionalString { get; set; }
    }
}
