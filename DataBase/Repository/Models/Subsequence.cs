using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Subsequence")]
public partial class Subsequence
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    [StringLength(500)]
    public string? Text { get; set; }

    [Column("idQuestion")]
    [StringLength(50)]
    public string? IdQuestion { get; set; }

    [Column("number")]
    public byte? Number { get; set; }

    [InverseProperty("IdSubsequenceNavigation")]
    public virtual ICollection<EmployeeSubsequence> EmployeeSubsequences { get; set; } = new List<EmployeeSubsequence>();

    [ForeignKey("IdQuestion")]
    [InverseProperty("Subsequences")]
    public virtual Question? IdQuestionNavigation { get; set; }
}
