using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("EmployeeSubsequence")]
public partial class EmployeeSubsequence
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idSubsequence")]
    public int? IdSubsequence { get; set; }

    [Column("idResult")]
    [StringLength(50)]
    public string? IdResult { get; set; }

    [Column("number")]
    public byte? Number { get; set; }

    [ForeignKey("IdResult")]
    [InverseProperty("EmployeeSubsequences")]
    public virtual Result? IdResultNavigation { get; set; }

    [ForeignKey("IdSubsequence")]
    [InverseProperty("EmployeeSubsequences")]
    public virtual Subsequence? IdSubsequenceNavigation { get; set; }
}
