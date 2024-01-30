using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("СompetenceСoeff")]
public partial class СompetenceСoeff
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idCompetence")]
    public int? IdCompetence { get; set; }

    [Column("idGroup")]
    public int? IdGroup { get; set; }

    [Column("coefficient", TypeName = "decimal(3, 2)")]
    public decimal? Coefficient { get; set; }

    [ForeignKey("IdCompetence")]
    [InverseProperty("СompetenceСoeffs")]
    public virtual Competence? IdCompetenceNavigation { get; set; }

    [ForeignKey("IdGroup")]
    [InverseProperty("СompetenceСoeffs")]
    public virtual GroupPosition? IdGroupNavigation { get; set; }
}
