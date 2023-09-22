using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("CompetenciesForGroup")]
public partial class CompetenciesForGroup
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idTest")]
    [StringLength(50)]
    public string? IdTest { get; set; }

    [Column("idGroupPositions")]
    public int? IdGroupPositions { get; set; }

    [ForeignKey("IdGroupPositions")]
    [InverseProperty("CompetenciesForGroups")]
    public virtual GroupPosition? IdGroupPositionsNavigation { get; set; }

    [ForeignKey("IdTest")]
    [InverseProperty("CompetenciesForGroups")]
    public virtual Test? IdTestNavigation { get; set; }
}
