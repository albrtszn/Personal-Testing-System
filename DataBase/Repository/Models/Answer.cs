using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Answer")]
public partial class Answer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    [StringLength(500)]
    public string? Text { get; set; }

    [Column("idQuestion")]
    public int? IdQuestion { get; set; }

    [Column("correct")]
    public bool? Correct { get; set; }

    [InverseProperty("IdAnswerNavigation")]
    public virtual ICollection<EmployeeAnswer> EmployeeAnswers { get; set; } = new List<EmployeeAnswer>();

    [ForeignKey("IdQuestion")]
    [InverseProperty("Answers")]
    public virtual Question? IdQuestionNavigation { get; set; }
}
