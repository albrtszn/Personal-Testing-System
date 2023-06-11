﻿using System;
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

    [Column("idTestType")]
    public int? IdTestType { get; set; }

    [ForeignKey("IdTestType")]
    [InverseProperty("Tests")]
    public virtual TestType? IdTestTypeNavigation { get; set; }

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<TestPurpose> TestPurposes { get; set; } = new List<TestPurpose>();

    [InverseProperty("IdTestNavigation")]
    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
