using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("ElployeeResultSubcompetence")]
public partial class ElployeeResultSubcompetence
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idResult")]
    [StringLength(50)]
    public string? IdResult { get; set; }

    [Column("idSubcompetence")]
    public int? IdSubcompetence { get; set; }

    [Column("result")]
    public int Result { get; set; }

    [ForeignKey("IdResult")]
    [InverseProperty("ElployeeResultSubcompetences")]
    public virtual Result? IdResultNavigation { get; set; }

    [ForeignKey("IdSubcompetence")]
    [InverseProperty("ElployeeResultSubcompetences")]
    public virtual Subcompetence? IdSubcompetenceNavigation { get; set; }
}
