using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("QuestionSubcompetence")]
[Index("IdQuestion", Name = "UQ__Employee__1196F46400CDB90A", IsUnique = true)]
public partial class QuestionSubcompetence
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idQuestion")]
    [StringLength(50)]
    public string? IdQuestion { get; set; }

    [Column("idSubcompetence")]
    public int? IdSubcompetence { get; set; }

    [ForeignKey("IdQuestion")]
    [InverseProperty("QuestionSubcompetence")]
    public virtual Question? IdQuestionNavigation { get; set; }

    [ForeignKey("IdSubcompetence")]
    [InverseProperty("QuestionSubcompetences")]
    public virtual Subcompetence? IdSubcompetenceNavigation { get; set; }
}
