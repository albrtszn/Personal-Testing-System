using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("TestScore")]
public partial class TestScore
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("minValue")]
    public int MinValue { get; set; }

    [Column("maxValue")]
    public int MaxValue { get; set; }

    [Column("description")]
    [StringLength(1500)]
    public string? Description { get; set; }

    [Column("idTest")]
    [StringLength(50)]
    public string? IdTest { get; set; }

    [Column("recommend")]
    public bool Recommend { get; set; }

    [Column("numberPoints")]
    public int NumberPoints { get; set; }

    [Column("name")]
    [StringLength(70)]
    public string? Name { get; set; }

    [ForeignKey("IdTest")]
    [InverseProperty("TestScores")]
    public virtual Test? IdTestNavigation { get; set; }
}
