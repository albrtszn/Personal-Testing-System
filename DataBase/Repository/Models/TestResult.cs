using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("TestResult")]
public partial class TestResult
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idEmployee")]
    public int? IdEmployee { get; set; }

    [Column("idTest")]
    public int? IdTest { get; set; }

    [Column("startDate")]
    public DateOnly? StartDate { get; set; }

    [Column("startTime")]
    public TimeOnly? StartTime { get; set; }

    [Column("duration")]
    public byte? Duration { get; set; }

    [Column("endTime")]
    public TimeOnly? EndTime { get; set; }

    [InverseProperty("IdTestResultNavigation")]
    public virtual ICollection<EmployeeAnswer> EmployeeAnswers { get; set; } = new List<EmployeeAnswer>();

    [InverseProperty("IdTestResultNavigation")]
    public virtual ICollection<EmployeeMatching> EmployeeMatchings { get; set; } = new List<EmployeeMatching>();

    [InverseProperty("IdTestResultNavigation")]
    public virtual ICollection<EmployeeSubsequence> EmployeeSubsequences { get; set; } = new List<EmployeeSubsequence>();

    [ForeignKey("IdEmployee")]
    [InverseProperty("TestResults")]
    public virtual Employee? IdEmployeeNavigation { get; set; }

    [ForeignKey("IdTest")]
    [InverseProperty("TestResults")]
    public virtual Test? IdTestNavigation { get; set; }
}
