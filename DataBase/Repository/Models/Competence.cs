using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Competence")]
public partial class Competence
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [InverseProperty("IdCompetenceNavigation")]
    public virtual ICollection<CompetenceScore> CompetenceScores { get; set; } = new List<CompetenceScore>();

    [InverseProperty("IdCompetenceNavigation")]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();

    [InverseProperty("IdCompetenceNavigation")]
    public virtual ICollection<СompetenceСoeff> СompetenceСoeffs { get; set; } = new List<СompetenceСoeff>();
}
