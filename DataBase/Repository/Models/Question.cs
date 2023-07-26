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
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [Column("text")]
    [StringLength(500)]
    public string? Text { get; set; }

    [Column("idQuestionType")]
    public int? IdQuestionType { get; set; }

    [Column("idTest")]
    [StringLength(50)]
    public string? IdTest { get; set; }

    [Column("imagePath")]
    [StringLength(150)]
    public string? ImagePath { get; set; }

    [InverseProperty("IdQuestionNavigation")]
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [InverseProperty("IdQuestionNavigation")]
    public virtual ICollection<FirstPart> FirstParts { get; set; } = new List<FirstPart>();

    [ForeignKey("IdQuestionType")]
    [InverseProperty("Questions")]
    public virtual QuestionType? IdQuestionTypeNavigation { get; set; }

    [ForeignKey("IdTest")]
    [InverseProperty("Questions")]
    public virtual Test? IdTestNavigation { get; set; }

    [InverseProperty("IdQuestionNavigation")]
    public virtual ICollection<Subsequence> Subsequences { get; set; } = new List<Subsequence>();
}
