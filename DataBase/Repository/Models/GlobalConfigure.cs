using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("GlobalConfigure")]
public partial class GlobalConfigure
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("testingTimeLimit")]
    public bool TestingTimeLimit { get; set; }

    [Column("skippingQuestion")]
    public bool SkippingQuestion { get; set; }

    [Column("earlyCompletionTesting")]
    public bool EarlyCompletionTesting { get; set; }

    [Column("additionalBool")]
    public bool? AdditionalBool { get; set; }

    [Column("additionalInt")]
    public int? AdditionalInt { get; set; }

    [Column("additionalString")]
    [StringLength(500)]
    public string? AdditionalString { get; set; }
}
