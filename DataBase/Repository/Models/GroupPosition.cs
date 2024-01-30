using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

public partial class GroupPosition
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("idProfile")]
    public int? IdProfile { get; set; }

    [InverseProperty("IdGroupPositionsNavigation")]
    public virtual ICollection<CompetenciesForGroup> CompetenciesForGroups { get; set; } = new List<CompetenciesForGroup>();

    [ForeignKey("IdProfile")]
    [InverseProperty("GroupPositions")]
    public virtual Profile? IdProfileNavigation { get; set; }

    [InverseProperty("IdGroupPositionsNavigation")]
    public virtual ICollection<Subdivision> Subdivisions { get; set; } = new List<Subdivision>();

    [InverseProperty("IdGroupNavigation")]
    public virtual ICollection<СompetenceСoeff> СompetenceСoeffs { get; set; } = new List<СompetenceСoeff>();
}
