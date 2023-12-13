using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Message")]
public partial class Message
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idEmployee")]
    [StringLength(50)]
    public string? IdEmployee { get; set; }

    [Column("messageText")]
    [StringLength(500)]
    public string? MessageText { get; set; }

    [Column("statusRead")]
    public bool StatusRead { get; set; }

    [Column("dateAndTie", TypeName = "datetime")]
    public DateTime DateAndTie { get; set; }

    [ForeignKey("IdEmployee")]
    [InverseProperty("Messages")]
    public virtual Employee? IdEmployeeNavigation { get; set; }
}
