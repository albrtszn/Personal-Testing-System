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
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [Column("weight")]
    public int? Weight { get; set; }

    [Column("idCompetence")]
    public int? IdCompetence { get; set; }

    [Column("description")]
    [StringLength(600)]
    public string? Description { get; set; }

    [Column("instruction")]
    [StringLength(1000)]
    public string? Instruction { get; set; }

    [Column("generation")]
    public bool? Generation { get; set; }

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<CompetenciesForGroup> CompetenciesForGroups { get; set; } = new List<CompetenciesForGroup>();

    [ForeignKey("IdCompetence")]
    [InverseProperty("Tests")]
    public virtual Competence? IdCompetenceNavigation { get; set; }

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<TestPurpose> TestPurposes { get; set; } = new List<TestPurpose>();

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<TestScore> TestScores { get; set; } = new List<TestScore>();
}
