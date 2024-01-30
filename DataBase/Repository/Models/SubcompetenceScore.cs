using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("SubcompetenceScore")]
public partial class SubcompetenceScore
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("minValue")]
    public int? MinValue { get; set; }

    [Column("maxValue")]
    public int? MaxValue { get; set; }

    [Column("description")]
    [StringLength(500)]
    public string? Description { get; set; }

    [Column("idSubcompetence")]
    public int? IdSubcompetence { get; set; }

    [Column("numberPoints")]
    public int? NumberPoints { get; set; }

    [ForeignKey("IdSubcompetence")]
    [InverseProperty("SubcompetenceScores")]
    public virtual Subcompetence? IdSubcompetenceNavigation { get; set; }
}
