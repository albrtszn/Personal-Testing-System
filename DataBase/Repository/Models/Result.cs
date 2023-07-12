using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Result")]
public partial class Result
{
    [Key]
    [Column("id")]
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [Column("idTest")]
    [StringLength(50)]
    public string? IdTest { get; set; }

    [Column("scoreFrom")]
    public int? ScoreFrom { get; set; }

    [Column("scoreTo")]
    public int? ScoreTo { get; set; }

    [Column("startDate")]
    public DateOnly? StartDate { get; set; }

    [Column("startTime")]
    public TimeOnly? StartTime { get; set; }

    [Column("duration")]
    public byte? Duration { get; set; }

    [Column("endTime")]
    public TimeOnly? EndTime { get; set; }

    [Column("description")]
    [StringLength(3000)]
    public string? Description { get; set; }

    [InverseProperty("IdResultNavigation")]
    public virtual ICollection<EmployeeAnswer> EmployeeAnswers { get; set; } = new List<EmployeeAnswer>();

    [InverseProperty("IdResultNavigation")]
    public virtual ICollection<EmployeeMatching> EmployeeMatchings { get; set; } = new List<EmployeeMatching>();

    [InverseProperty("IdResultNavigation")]
    public virtual ICollection<EmployeeResult> EmployeeResults { get; set; } = new List<EmployeeResult>();

    [InverseProperty("IdResultNavigation")]
    public virtual ICollection<EmployeeSubsequence> EmployeeSubsequences { get; set; } = new List<EmployeeSubsequence>();

    [ForeignKey("IdTest")]
    [InverseProperty("Results")]
    public virtual Test? IdTestNavigation { get; set; }
}
