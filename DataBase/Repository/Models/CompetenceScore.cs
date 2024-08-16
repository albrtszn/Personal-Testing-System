using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("CompetenceScore")]
public partial class CompetenceScore
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idCompetence")]
    public int IdCompetence { get; set; }

    [Column("idGroup")]
    public int IdGroup { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("numnerPoints")]
    public int NumnerPoints { get; set; }

    [Column("description")]
    [StringLength(4000)]
    public string? Description { get; set; }

    [ForeignKey("IdCompetence")]
    [InverseProperty("CompetenceScores")]
    public virtual Competence IdCompetenceNavigation { get; set; } = null!;

    [ForeignKey("IdGroup")]
    [InverseProperty("CompetenceScores")]
    public virtual GroupPosition IdGroupNavigation { get; set; } = null!;
}
