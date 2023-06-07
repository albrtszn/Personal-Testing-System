using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Question")]
public partial class Question
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    [StringLength(500)]
    public string? Text { get; set; }

    [Column("idQuestionType")]
    public int? IdQuestionType { get; set; }

    [InverseProperty("IdQuestionNavigation")]
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [InverseProperty("IdQuestionNavigation")]
    public virtual ICollection<FirstPart> FirstParts { get; set; } = new List<FirstPart>();

    [ForeignKey("IdQuestionType")]
    [InverseProperty("Questions")]
    public virtual QuestionType? IdQuestionTypeNavigation { get; set; }

    [InverseProperty("IdQuestionNavigation")]
    public virtual ICollection<Subsequence> Subsequences { get; set; } = new List<Subsequence>();

    [ForeignKey("IdQuestion")]
    [InverseProperty("IdQuestions")]
    public virtual ICollection<Test> IdTests { get; set; } = new List<Test>();
}
