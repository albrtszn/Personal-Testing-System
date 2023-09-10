using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Log", Schema = "fitpsuon_fitpsuon")]
public partial class Log
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("urlPath")]
    [StringLength(50)]
    public string? UrlPath { get; set; }

    [Column("userId")]
    [StringLength(50)]
    public string? UserId { get; set; }

    [Column("userIp")]
    [StringLength(50)]
    public string? UserIp { get; set; }

    [Column("dataTime", TypeName = "datetime")]
    public DateTime? DataTime { get; set; }

    [Column("params")]
    [StringLength(150)]
    public string? Params { get; set; }
}
