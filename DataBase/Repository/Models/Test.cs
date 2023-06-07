using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Test")]
public partial class Test
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [Column("idTestType")]
    public int? IdTestType { get; set; }

    [ForeignKey("IdTestType")]
    [InverseProperty("Tests")]
    public virtual TestType? IdTestTypeNavigation { get; set; }

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<TestPurpose> TestPurposes { get; set; } = new List<TestPurpose>();

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();

    [ForeignKey("IdTest")]
    [InverseProperty("IdTests")]
    public virtual ICollection<Question> IdQuestions { get; set; } = new List<Question>();
}
